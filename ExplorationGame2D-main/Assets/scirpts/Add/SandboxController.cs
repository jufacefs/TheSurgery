using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandboxController : MonoBehaviour
{
    public GameObject parentPrefab; 
    public GameObject childPrefab; 

    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            // see if the parent is clicked
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // if parent clicked, generate childern
                if (hit.collider.gameObject.CompareTag("ParentObject"))
                {
                    //instantiate children at where it's clicked
                    InstantiateChildObject(hit.collider.transform.position);
                }
            }
        }
    }

    private void InstantiateChildObject(Vector3 position)
    {
        
        GameObject childObject = Instantiate(childPrefab, position, Quaternion.identity);
        //add colliders to children
        childObject.AddComponent<BoxCollider>();
    }

    // Can directly setup different parents and children in Unity inspector.
}
