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
        SpriteRenderer parentRenderer = GetComponent<SpriteRenderer>();
        int parentSortingOrder = parentRenderer != null ? parentRenderer.sortingOrder : 0;
        for (int i = 0; i < numberOfItems; i++)
        {
            int pos;
            do
            {
                pos = Random.Range(0, ChildrenLocations.Length);
            }
            while (PreviousPositions.Contains(pos));
            PreviousPositions.Add(pos);

            GameObject child = Instantiate(itemPrefab, ChildrenLocations[pos].position, Quaternion.identity, transform);
            child.transform.localScale = new Vector3(parentWidth * childSizeMultiplier, parentHeight * childSizeMultiplier, 1);

            SpriteRenderer childRenderer = child.GetComponent<SpriteRenderer>();
            if (childRenderer != null)
            {
                childRenderer.sortingLayerName = "Foreground"; // suppose "forground" is on the sorting layer of the parent
                childRenderer.sortingOrder = 1; // make sure this value is bigger than the sorting layer value of parent
            }

                children.Add(child);

        }
    }
}