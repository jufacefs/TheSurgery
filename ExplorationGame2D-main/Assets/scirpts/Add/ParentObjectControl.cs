using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParentObjectControl : MonoBehaviour
{
    public GameObject[] children;
    public GameObject itemPrefab; // 用于生成的子GameObject的预制件
    public string[] spritePaths = new string[] { "Sprites/cone", "Sprites/mySprite2" }; // 存放Sprite路径的数组
    public int maxItems = 5; // 最多生成的项目数量
    private Collider2D parentCollider;

    public bool hasGenerated = false; // 标记是否已生成子对象

    public List<GameObject> generatedObjects = new List<GameObject>();


    void Start()
    {
        parentCollider = GetComponent<Collider2D>();
        ValueManager.Instance.ResetValue();
        ToggleChildrenColliders(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            HandleClick(hit.collider);
        }
    }

    void HandleClick(Collider2D collider)
    {
        if (collider == parentCollider)
        {
            ValueManager.Instance.IncrementValue();
            ActivateChildrenColliders(ValueManager.Instance.currentValue);
            ToggleChildrenColliders(ValueManager.Instance.currentValue >= 2);

            if (!hasGenerated)
            {
                GenerateRandomSprites();
                hasGenerated = true;
            }
            else
            {
                ShowGeneratedObjects();
            }


            Debug.Log("Parent clicked, children collider state updated based on currentValue.");

        }
        else
        {
            CheckChildrenColliders(collider);
        }
    }

    void CheckChildrenColliders(Collider2D collider)
    {
        foreach (GameObject child in children)
        {
            Collider2D childCollider = child.GetComponent<Collider2D>();
            if (collider == childCollider && childCollider.enabled)
            {
                Debug.Log(child.name + " child clicked.");
                PerformChildAction(child);
                break;
            }
        }
    }


    public void ActivateChildrenColliders(int activeCount)
    {
        for (int i = 0; i < children.Length; i++)
        {
            Collider2D childCollider = children[i].GetComponent<Collider2D> ();
            // Enable the collider if it is within the active count range, otherwise disable it
            childCollider.enabled = i < activeCount;
        }
    }


    void PerformChildAction(GameObject child)
    {
        child.GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void ToggleChildrenColliders(bool enable)
    {
        foreach (GameObject child in children)
        {
            Collider2D childCollider = child.GetComponent<Collider2D>();
            childCollider.enabled = enable;
        }
    }

    void GenerateRandomSprites()
    {
        for (int i = 0; i < Random.Range(0, maxItems + 1); i++)
        {
            GameObject newItem = Instantiate(itemPrefab, transform);
            newItem.transform.localPosition = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);

            int spriteIndex = Random.Range(0, spritePaths.Length);
            Sprite newSprite = Resources.Load<Sprite>(spritePaths[spriteIndex]);
            if (newSprite == null)
            {
                Debug.LogError("Failed to load sprite. Check path and sprite settings.");
            }
            else
            {
                newItem.GetComponent<SpriteRenderer>().sprite = newSprite;
            }

            generatedObjects.Add(newItem);
        }
    }

    public void ShowGeneratedObjects()
    {
        foreach (GameObject obj in generatedObjects)
        {
            obj.SetActive(true);
        }
    }

    public void HideGeneratedObjects()
    {
        Debug.Log("Hiding " + generatedObjects.Count + " objects.");
        foreach (GameObject obj in generatedObjects)
        {
            obj.SetActive(false);
        }
    }


}
