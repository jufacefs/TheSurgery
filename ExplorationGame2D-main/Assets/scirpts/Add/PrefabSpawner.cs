using UnityEngine;
using System.Collections.Generic;

public class PrefabSpawner : MonoBehaviour
{
    // Declare a public variable to set the Prefab in the editor
    [SerializeField] protected GameObject prefab;

    // Declare a public variable so that you can set GameObject B in the editor
    [SerializeField] protected GameObject parentGameObject;

    [SerializeField] protected float scaleMultiplier = 0.8f;

    protected GameObject currentParent;

    // Detecting generated subclasses
    protected HashSet<GameObject> generatedObjects = new HashSet<GameObject>();

    protected virtual void Start()
    {
        // Initialize currentParent to parentGameObject
        if (parentGameObject != null)
        {
            currentParent = parentGameObject;
        }
        else
        {
            Debug.LogError("parentGameObject is null.");
        }
    }

    // The Update function is called every frame
    protected virtual void Update()
    {
        // Detecting left mouse button clicks
        if (Input.GetMouseButtonDown(0))
        {
            detection();
        }
    }

    protected virtual void detection()
    {
        if (prefab == null || parentGameObject == null)
        {
            Debug.LogError("Prefab or Parent GameObject is null.");
            return;
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            // If the clicked object is parentGameObject
            if (hit.transform.gameObject == parentGameObject)
            {
                // Check if a subclass has already been generated
                if (generatedObjects.Contains(parentGameObject))
                {
                    Debug.Log("Parent GameObject has already generated children, not generating again.");
                    return;
                }

                currentParent = parentGameObject;
                Debug.Log("Hit parentGameObject, setting currentParent and instantiating prefab");
                InstantiatePrefab();
                generatedObjects.Add(parentGameObject);
            }
            // If the clicked object is currentParent or its child
            else if (hit.transform.gameObject == currentParent || hit.transform.IsChildOf(currentParent.transform))
            {
                // Check if a subclass has already been generated
                if (generatedObjects.Contains(hit.transform.gameObject))
                {
                    Debug.Log("This GameObject has already generated children, not generating again.");
                    return;
                }

                currentParent = hit.transform.gameObject;
                Debug.Log("Hit currentParent or its child, setting currentParent and instantiating prefab");
                InstantiatePrefab();
                generatedObjects.Add(hit.transform.gameObject);
            }
        }
    }

    // Function to instantiate Prefab
    protected virtual void InstantiatePrefab()
    {
        if (prefab == null || currentParent == null)
        {
            Debug.LogError("Prefab or currentParent is null. Cannot instantiate.");
            return;
        }

        // Instantiate Prefab at the location of currentParent
        GameObject prefabInstance = Instantiate(prefab, currentParent.transform.position, Quaternion.identity);
        Debug.Log("Instantiated prefab at " + currentParent.transform.position);

        // Adjust the position and scale of instanced objects
        prefabInstance.transform.SetParent(currentParent.transform, false);
        prefabInstance.transform.localPosition = Vector3.zero;
        prefabInstance.transform.localScale = Vector3.one;

        // Create a temporary list to store the child objects
        List<Transform> children = new List<Transform>();

        // Traverse the sub-objects of Prefab and add them to a temporary list
        foreach (Transform child in prefabInstance.transform)
        {
            children.Add(child);
            Debug.Log("Added child: " + child.name);

            // Make sure that newly created sub-objects are above all existing sub-objects
            SpriteRenderer childRenderer = child.GetComponent<SpriteRenderer>();
            if (childRenderer != null)
            {
                SpriteRenderer parentRenderer = currentParent.GetComponent<SpriteRenderer>();
                if (parentRenderer != null)
                {
                    childRenderer.sortingOrder = parentRenderer.sortingOrder + 1;
                }
            }
        }

        // Sets the child object as the child of currentParent and adjusts its position and rotation
        foreach (Transform child in children)
        {
            child.SetParent(currentParent.transform, false);
            Debug.Log("Set parent of " + child.name + " to " + currentParent.name);

            // Reduce the scale of the sub-object to 0.8 times the current object
            child.localScale = currentParent.transform.localScale * scaleMultiplier;
            Debug.Log("Scaled child: " + child.name + " to " + child.localScale);
        }

        // Destroy the Prefab instance itself
        Destroy(prefabInstance);
        Debug.Log("Destroyed prefab instance");
    }
}