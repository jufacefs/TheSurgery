using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitToCamera : MonoBehaviour
{
    public Camera mainCamera;
    private ParentObjectControl parentObjectControl;

    private Vector3 originalPosition;  //the initial postion of the main camera
    private float originalSize;  // initial size of main camera
    private bool isCameraMoving = false;
    


    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        // keep record of the initial 
        originalPosition = mainCamera.transform.position;
        originalSize = mainCamera.orthographicSize;

        parentObjectControl = FindObjectOfType<ParentObjectControl>();
    }

    void Update()
    {
        if (isCameraMoving || !Input.GetMouseButtonUp(1))  // if the camera is moving or not right mouse up, do nothing
            return;
        ResetToOriginalState(); // Reset to original state when right mouse button is released

    //StartCoroutine(MoveAndZoomCamera(originalPosition, originalSize)); // recover the original size and location
}

    void OnMouseDown()
    {
        if (isCameraMoving || !Input.GetMouseButton(0))
            return;


        if (GetComponent<Collider2D>() == null)
            return;

        float requiredSize = CalculateRequiredSize();
        Vector3 targetPosition = transform.position - (transform.forward * 10);  // might need to change the values

        StartCoroutine(MoveAndZoomCamera(targetPosition, requiredSize));
    }


    //calculate the required camera size to fit the shots
    float CalculateRequiredSize()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            float spriteHeight = spriteRenderer.bounds.size.y;
            float spriteWidth = spriteRenderer.bounds.size.x;
            float screenAspect = Screen.width / (float)Screen.height;
            float spriteAspect = spriteWidth / spriteHeight;

            if (spriteAspect > screenAspect)
            {
                //   change camera size based on sprite width
                return spriteWidth / screenAspect / 2.0f;
            }
            else
            {

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
        float smoothTime = 0.3f;

        while (Vector3.Distance(mainCamera.transform.position, targetPosition) > 0.1f &&
               Mathf.Abs(mainCamera.orthographicSize - targetSize) > 0.1f)
        {
            mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, targetPosition, ref velocity, smoothTime);
            mainCamera.orthographicSize = Mathf.SmoothDamp(mainCamera.orthographicSize, targetSize, ref sizeVelocity, smoothTime);
            yield return null;
        }

        mainCamera.transform.position = targetPosition;
        mainCamera.orthographicSize = targetSize;
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
        parentObjectControl.HideAllGeneratedObjects();
        isCameraMoving = false;
    }
}