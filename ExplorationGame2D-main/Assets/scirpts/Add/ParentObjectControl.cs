using UnityEngine;
using System.Collections.Generic;

public class ParentObjectControl : MonoBehaviour
{
    // Public fields to adjust in the Unity editor
    public GameObject itemPrefab;  // Prefab for child objects
    public Transform[] ChildrenLocations;  // Positions where children can be instantiated
    public List<int> usedPositions = new List<int>();  // Tracks positions that have been used

    // Configurable range of how many children to generate
    public int minItems = 1;
    public int maxItems = 5;
    public float childSizeMultiplier = 0.1f;  // Scale factor for child objects relative to the parent size

    private Collider2D parentCollider;  // Collider component of the parent
    private float parentWidth;  // Width of the parent collider
    private float parentHeight;  // Height of the parent collider
    private bool hasGenerated = false;  // Flag to ensure children are only generated once

    void Start()
    {
        // Initialize collider and dimensions on start
        parentCollider = GetComponent<BoxCollider2D>();
        parentWidth = parentCollider.bounds.size.x;
        parentHeight = parentCollider.bounds.size.y;
    }

    void Update()
    {
        // Listen for mouse input to trigger child generation
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
        usedPositions.Clear();
        int numberOfItems = Random.Range(minItems, Mathf.Min(maxItems + 1, ChildrenLocations.Length));

        for (int i = 0; i < numberOfItems; i++)
        {
            int pos;
            do
            {
                pos = Random.Range(0, ChildrenLocations.Length);
            } while (usedPositions.Contains(pos));
            usedPositions.Add(pos);

            GameObject child = Instantiate(itemPrefab, ChildrenLocations[pos].position, Quaternion.identity, transform);
            child.transform.localScale = new Vector3(parentWidth * childSizeMultiplier, parentHeight * childSizeMultiplier, 1);
            SetChildRenderingOrder(child, GetComponent<SpriteRenderer>().sortingOrder + 1);

            // Make sure every child object has a collider
            Collider2D childCollider = child.GetComponent<Collider2D>();
            if (childCollider == null)
            {
                child.AddComponent<BoxCollider2D>();  // Add collider
            }

            // Adjust Z coordinate
            Vector3 position = child.transform.position;
            position.z = -0.1f;  // Make sure it's within click range
            child.transform.position = position;
        }
    }


    void SetChildRenderingOrder(GameObject child, int sortingOrder)
    {
        // Set the rendering layer and order to ensure children appear above parent
        SpriteRenderer childRenderer = child.GetComponent<SpriteRenderer>();
        if (childRenderer != null)
        {
            childRenderer.sortingLayerName = "Foreground";
            childRenderer.sortingOrder = sortingOrder;
        }
    }
}