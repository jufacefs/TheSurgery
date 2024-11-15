using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using TreeEditor;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization.Components;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class ZoomingController : MonoBehaviour
{
    // Start is called before the first frame update
    public static string spritePath = $"Sprites/ZoomingItems/";
    public static string jsonPath = $"Resources/json/";
    public Camera mainCamera;
    public static ZoomingController Instance;
    public LocalizeStringEvent textEvent;
    public GameObject textUI;

    public Vector3 oriCameraPos;
    private Vector3 originalPosition;  //the initial postion of the main camera
    private float originalSize;  // initial size of main camera
    private bool isCameraMoving = false;
    private bool isInfiniteTree = false;
    private bool isShowingText = false;
    private bool isZooming = false;
    private float smoothTime = 0.3f;
    private float ScaleFactor = 0.1f;
    private GameObject parentObj;
    private TreeNode rootNode;
    private TreeNode CurrentNode;
    private int layerCnt = 0;


    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        if(isZooming && Input.GetMouseButton(1) && !isShowingText)
        {
            exitZooming();
        }

    }

    static Sprite getSprite(string name)
    {
        //path = Path.Combine(Application.dataPath, spritePath);
        string path = Path.Combine(spritePath, name);
        Sprite loadedSprite = Resources.Load<Sprite>(path);

        if (loadedSprite == null)
        {
            Debug.LogWarning("Sprite not found at path: " + path);
        }else
        {
            Debug.Log("Sprite loaded successfully from AssetDatabase.");
        }

        return loadedSprite;
    }

    [System.Serializable]
    private class TreeNodeWrapper
    {
        public string name;
        public List<TreeNodeWrapper> children;
        public float posx;
        public float posy;
        public string text;

        public TreeNode ToTreeNode(TreeNode parent = null)
        {
            Debug.Log(name + " treenode loading");
            TreeNode node = new TreeNode(name) { parent = parent,sprite = getSprite(name),posx = posx,posy = posy,text = text};
            foreach (var child in children)
            {
                node.AddChild(child.ToTreeNode(node));
            }
            return node;
        }
    }

    public void LoadTree(string fileName)
    {
        // 从 StreamingAssets 目录中读取 JSON 文件内容
        string path = Path.Combine(Application.dataPath, jsonPath);
        string _jsonPath = Path.Combine(path, $"{fileName}");
        if (File.Exists(_jsonPath))
        {
            string jsonText = File.ReadAllText(_jsonPath);
            rootNode = JsonUtility.FromJson<TreeNodeWrapper>(jsonText).ToTreeNode();
            Debug.Log("Tree loaded successfully.");
        }
        else
        {
            Debug.LogWarning("Failed to load JSON file from StreamingAssets.");
            Debug.Log(_jsonPath);
        }
    }

    public void ParentClicked(string name,GameObject obj)
    {
        if (isCameraMoving || isShowingText)
        {
            return;
        }
        Debug.Log("parentClicked has been called");
        layerCnt = 1;
        isZooming = true;
        LoadTree(name);
        CurrentNode = rootNode;
        originalSize = mainCamera.orthographicSize;
        originalPosition = mainCamera.transform.position;
        parentObj = obj;
        obj.GetComponent<IsZoomingParent>().setClickable(false);

        showChildrenSprites(CurrentNode, obj);
        ClickedObj(obj);

    }

    public void ChildrenClicked(GameObject obj)
    {
        if (isCameraMoving || isShowingText)
        {
            return;
        }
        TreeNode nextNode = null;
        int cap = CurrentNode.children.Count;
        Sprite s = obj.GetComponent<SpriteRenderer>().sprite;
        for (int i = 0; i < cap; i += 1)
        {
            if (CurrentNode.children[i].sprite == s)
            {
                nextNode = CurrentNode.children[i];
            }
            CurrentNode.children[i].obj.GetComponent<IsZoomingParent>().setClickable(false);
        }

        if(nextNode.children.Count == 0)//is leaf node
        {
            
            for (int i = 0; i < cap; i += 1)
            {
                CurrentNode.children[i].obj.GetComponent<IsZoomingParent>().setClickable(true);
            }
            
            SetLocalizedText(nextNode.text);
            isShowingText = true;
            Debug.Log("we are clicking a leaf");
            clickToClose.beginListening(true);
            textUI.SetActive(true);
            
        }
        else
        {
            layerCnt += 1;
            
            CurrentNode = nextNode;
            showChildrenSprites(CurrentNode, obj);
            ClickedObj(obj);
        }
        
    }

    public void closeTextUI()
    {
        textUI.SetActive(false);
        isShowingText = false;
    }
    void ClickedObj(GameObject obj)
    {
        if (isCameraMoving || !Input.GetMouseButton(0))
            return;
        if (obj.GetComponent<PolygonCollider2D>() == null)
            return;
        float requiredSize = CalculateRequiredSize(obj);
        Vector3 targetPosition = obj.transform.position - (obj.transform.forward * 10);  // might need to change the values

        StartCoroutine(MoveAndZoomCamera(targetPosition, requiredSize));
        
        
    }

    void SetLocalizedText(string localizationKey)
    {
        textEvent.StringReference.TableEntryReference = localizationKey; // 设置条目键值
        textEvent.RefreshString(); // 刷新显示的字符串
    }

    void showChildrenSprites(TreeNode n,GameObject parent)
    {
        foreach(TreeNode c in n.children)
        {
            GameObject child = new GameObject(c.name);
            child.transform.localScale = new Vector3(0, 0, 0);
            child.transform.SetParent(parent.transform);
            SpriteRenderer spriteRenderer = child.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = c.sprite;
            spriteRenderer.sortingOrder = layerCnt;
            child.AddComponent<IsZoomingParent>();
            child.transform.localPosition = new Vector3(c.posx, c.posy, 0);

            c.obj = child;

            StartCoroutine(ShowNewChild(child));
        }
    }

    System.Collections.IEnumerator ShowNewChild(GameObject obj)
    {
        Vector3 velocity = Vector3.zero;
        Vector3 targetScale = new Vector3(ScaleFactor, ScaleFactor, ScaleFactor);
        while (Vector3.Distance(obj.transform.localScale, targetScale) > 0.001f)
        {
            obj.transform.localScale = Vector3.SmoothDamp(obj.transform.localScale, targetScale, ref velocity, smoothTime);
            yield return null;
        }
    }

    System.Collections.IEnumerator shinkObj(GameObject obj)
    {
        Vector3 velocity = Vector3.zero;
        Vector3 targetScale = new Vector3(0, 0, 0);
        while (Vector3.Distance(obj.transform.localScale, targetScale) > 0.001f)
        {
            obj.transform.localScale = Vector3.SmoothDamp(obj.transform.localScale, targetScale, ref velocity, smoothTime);
            yield return null;
        }
        Destroy(obj);
    }

    void exitZooming()
    {
        if (isCameraMoving || !Input.GetMouseButton(1))
        {
            Debug.Log("try to exit but isCameraMoving");
            return;
        }
            

        parentObj.GetComponent<IsZoomingParent>().setClickable(true);
        DestroyAllChildren(parentObj.transform);
        StartCoroutine(MoveAndZoomCamera(originalPosition, originalSize));
        isZooming = false;
    }

    void DestroyAllChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            StartCoroutine(shinkObj(parent.GetChild(i).gameObject));
        }
    }


    //calculate the required camera size to fit the shots
    float CalculateRequiredSize(GameObject obj)
    {
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            float spriteHeight = spriteRenderer.bounds.size.y;
            float spriteWidth = spriteRenderer.bounds.size.x;
            float screenAspect = Screen.width / (float)Screen.height;
            float spriteAspect = spriteWidth / spriteHeight;

            if (spriteAspect > screenAspect)
            {
                //   change camera size based on sprite width
                //return spriteWidth / screenAspect / 2.0f;
                return spriteWidth / screenAspect / 2.0f;
            }
            else
            {

                //return spriteHeight / 2.0f;
                return spriteHeight / 2.0f;
            }
        }
        return originalSize; // if there's no sprite renderer, return to the original size
    }

    System.Collections.IEnumerator MoveAndZoomCamera(Vector3 targetPosition, float targetSize)
    {
        isCameraMoving = true;
        Vector3 velocity = Vector3.zero;
        float sizeVelocity = 0f;
        

        while (Vector3.Distance(mainCamera.transform.position, targetPosition) > 0.001f ||
               Mathf.Abs(mainCamera.orthographicSize - targetSize) > 0.001f)
        {
            mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, targetPosition, ref velocity, smoothTime);
            mainCamera.orthographicSize = Mathf.SmoothDamp(mainCamera.orthographicSize, targetSize, ref sizeVelocity, smoothTime);
            yield return null;
        }

        //mainCamera.transform.position = targetPosition;
        //mainCamera.orthographicSize = targetSize;
        isCameraMoving = false;

        //Debug.Log("Camera returned to original size.");

        //  if (Mathf.Abs(mainCamera.orthographicSize - originalSize) < 0.01f)
        //  {
        //      Debug.Log("Camera returned to original size, hiding generated objects.");
        //      parentObjectControl.HideAllGeneratedObjects();
        //  }
    }
    void ResetToOriginalState()
    {
        isCameraMoving = true; // Prevent other movements while resetting
        mainCamera.transform.position = originalPosition;
        mainCamera.orthographicSize = originalSize;
        Debug.Log("Camera returned to original size, hiding generated objects.");
        HideAllGeneratedObjects();
        isCameraMoving = false;
    }

    public static string getJsonPath()
    {
        return jsonPath;
    }

    public void HideAllGeneratedObjects()
    {

    }

    [System.Serializable]
    public class TreeNode
    {
        public string name;
        public float posx;
        public float posy;
        public Sprite sprite;
        public GameObject obj;
        public string text;
        public List<TreeNode> children = new List<TreeNode>();
        [System.NonSerialized] public TreeNode parent;

        // Constructor
        public TreeNode(string name)
        {
            this.name = name;
            posx = 0;
            posy = 0;
            this.sprite = null;
        }
        public TreeNode(string name, Sprite sprite)
        {
            this.name = name;
            posx = 0;
            posy = 0;
            this.sprite = sprite;
        }

        public TreeNode(string name,float x,float y)
        {
            this.name = name;
            posx = x;
            posy = y;
        }

        // Add child node
        public void AddChild(TreeNode child)
        {
            child.parent = this;
            children.Add(child);
        }

        // Get parent node
        public TreeNode GetParent()
        {
            return parent;
        }

        // Get children nodes
        public List<TreeNode> GetChildren()
        {
            return children;
        }
    }

}
