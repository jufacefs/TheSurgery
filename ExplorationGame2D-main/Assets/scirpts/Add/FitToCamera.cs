using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitToCamera : MonoBehaviour
{
    public Camera mainCamera;  // 引用主摄像机
    private ParentObjectControl parentObjectControl;  // 引用ParentObjectControl

    private Vector3 originalPosition;  // 摄像机的原始位置
    private float originalSize;  // 摄像机的原始大小
    private bool isCameraMoving = false;  // 相机是否正在移动

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        // 保存摄像机的初始位置和大小
        originalPosition = mainCamera.transform.position;
        originalSize = mainCamera.orthographicSize;

        parentObjectControl = FindObjectOfType<ParentObjectControl>();
    }

    void OnMouseDown()
    {
        if (isCameraMoving || !Input.GetMouseButton(0))  // 如果相机正在移动或不是鼠标左键点击，则不做处理
            return;

        // 确认点击的是 Collider2D
        if (GetComponent<Collider2D>() == null)
            return;

        // 计算需要的相机大小以适应该对象
        float requiredSize = CalculateRequiredSize();
        // 计算该对象的中心位置
        Vector3 targetPosition = transform.position - (transform.forward * 10);  // 你可能需要根据实际情况调整这里的值

        StartCoroutine(MoveAndZoomCamera(targetPosition, requiredSize));
    }

    void Update()
    {
        if (isCameraMoving || !Input.GetMouseButtonUp(1))  // 如果相机正在移动或不是鼠标右键释放，则不做处理
            return;

        StartCoroutine(MoveAndZoomCamera(originalPosition, originalSize)); // 恢复摄像机的初始位置和大小
    }

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
                // 如果精灵的宽高比大于屏幕的宽高比，则以宽度为基准调整大小
                return spriteWidth / screenAspect / 2.0f;
            }
            else
            {
                // 如果高度是限制因素
                return spriteHeight / 2.0f;
            }
        }
        return originalSize; // 如果没有精灵渲染器，返回原始大小
    }

    System.Collections.IEnumerator MoveAndZoomCamera(Vector3 targetPosition, float targetSize)
    {
        isCameraMoving = true;
        Vector3 velocity = Vector3.zero;
        float sizeVelocity = 0f;
        float smoothTime = 0.3f;

        while (Vector3.Distance(mainCamera.transform.position, targetPosition) > 0.01f ||
               Mathf.Abs(mainCamera.orthographicSize - targetSize) > 0.01f)
        {
            mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, targetPosition, ref velocity, smoothTime);
            mainCamera.orthographicSize = Mathf.SmoothDamp(mainCamera.orthographicSize, targetSize, ref sizeVelocity, smoothTime);
            yield return null;
        }

        mainCamera.transform.position = targetPosition;
        mainCamera.orthographicSize = targetSize;
        isCameraMoving = false;

        if (Mathf.Abs(mainCamera.orthographicSize - originalSize) < 0.01f)
        {
            Debug.Log("Camera returned to original size, hiding generated objects.");
            parentObjectControl.HideGeneratedObjects(); 
        }
    }
}
