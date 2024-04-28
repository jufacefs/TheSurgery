using UnityEngine;
using System.Collections.Generic;

public class ParentObjectControl : MonoBehaviour
{
    public GameObject itemPrefab;
    public Transform[] ChildrenLocations;
    public List<int> PreviousPositions = new List<int>();


    //two public fields in Unity interface where the user can type in the range of random children number
    public int minItems = 1; 
    public int maxItems = 5; 
    public float childSizeMultiplier = 0.1f; // children to parent in scale
    private Collider2D parentCollider;
    private float parentWidth;
    private float parentHeight;
    private bool hasGenerated = false;


    void Start()
    {
        parentCollider = GetComponent<BoxCollider2D>();
        parentWidth = parentCollider.bounds.size.x;
        parentHeight = parentCollider.bounds.size.y;

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !hasGenerated)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider == parentCollider)
            {
                GenerateChildrenWithinParent();
                hasGenerated = true;
            }
        }
    }

    void GenerateChildrenWithinParent()
    {
        int numberOfItems = Random.Range(minItems, maxItems + 1); 
        List<GameObject> children = new List<GameObject>();

        for (int i = 0; i < numberOfItems; i++)
        {
            // generate new children
            
            SpriteRenderer parentRenderer = GetComponent<SpriteRenderer>(); 
            int parentSortingOrder = parentRenderer != null ? parentRenderer.sortingOrder : 0;
            GameObject child = GenerateChild(itemPrefab, transform, parentWidth, parentHeight, childSizeMultiplier, children, ChildrenLocations, parentSortingOrder);

            SetChildRenderingOrder(child);  
            children.Add(child);
        }
    }

    void SetChildRenderingOrder(GameObject child)
    {
        SpriteRenderer childRenderer = child.GetComponent<SpriteRenderer>();
        if (childRenderer != null)
        {
            childRenderer.sortingLayerName = "Foreground"; // suppose "forground" is on the sorting layer of the parent
            childRenderer.sortingOrder = 1; // make sure this value is bigger than the sorting layer value of parent
        }
    }
    public GameObject GenerateChild(GameObject itemPrefab, Transform parent, float parentWidth, float parentHeight, float childSizeMultiplier, List<GameObject> existingChildren, Transform[] ChildrenLocations, int parentSortingOrder = 0)
    {

        PreviousPositions.Clear();
        GameObject child = Instantiate(itemPrefab, parent);
        float scaledChildWidth = parentWidth * childSizeMultiplier;
        float scaledChildHeight = parentHeight * childSizeMultiplier;
        child.transform.localScale = new Vector3(scaledChildWidth, scaledChildHeight, 1);

        // render the children first then the parents
        SpriteRenderer childRenderer = child.GetComponent<SpriteRenderer>();
        if (childRenderer != null)
        {
            childRenderer.sortingOrder = parentSortingOrder + 1;
        }

        // update the collider's border
        //child.GetComponent<BoxCollider2D>().size = new Vector2(scaledChildWidth, scaledChildHeight);
        int pos = Random.Range(0, ChildrenLocations.Length);
        while (PreviousPositions.Contains(pos))
        {
            pos = Random.Range(0, ChildrenLocations.Length);
            Debug.LogError("In while loop");


        }
        PreviousPositions.Add(pos);
        child.transform.position = ChildrenLocations[pos].position;

        //if (!isPlaced)
        //{
        //    Destroy(child); // if cannot be placed, destroy child
        //}
        //else
        //{
        existingChildren.Add(child); // add to child list when placed
        //}

        return child;
    }

    //private bool PositionChild(GameObject child, float width, float height, float parentWidth, float parentHeight, List<GameObject> existingChildren, Transform[] ChildrenLocations)
    //{
    //    BoxCollider2D childCollider = child.GetComponent<BoxCollider2D>();
    //    if (childCollider == null)
    //    {
    //        Debug.LogError("Child object is missing BoxCollider2D component!");
    //        return false;
    //    }

    //    bool placed = false;
    //    int attempts = 0;
        

        //while (!placed && attempts < 100)
        //{
        //    float minX = -parentWidth / 2 + width / 2;
        //    float maxX = parentWidth / 2 - width / 2;
        //    float minY = -parentHeight / 2 + height / 2;
        //    float maxY = parentHeight / 2 - height / 2;

        //    float posX = Random.Range(minX, maxX);
        //    float posY = Random.Range(minY, maxY);
        //    Vector3 potentialPosition = new Vector3(posX, posY, -1);

        //    childCollider.enabled = false; // disable collider to prevent overlayed checking for potential positions
        //    child.transform.localPosition = potentialPosition;
        //    childCollider.enabled = true; 

        //    if (!IsColliding(childCollider, existingChildren))
        //    {
        //        placed = true;
        //    }

        //    attempts++;
        //}

    //    if (!placed)
    //    {
    //        Debug.LogError("Failed to place child object without collision!");
    //    }

    //    return placed;
    //}

    private bool IsColliding(BoxCollider2D childCollider, List<GameObject> existingChildren)
    {
        foreach (GameObject existingChild in existingChildren)
        {
            BoxCollider2D existingCollider = existingChild.GetComponent<BoxCollider2D>();
            if (existingCollider != null && childCollider.bounds.Intersects(existingCollider.bounds))
            {
                return true;
            }
        }
        return false;
    }
}
