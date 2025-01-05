using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class fakePlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera mainCamera;
    public float rotationAngle = 20f;      // 旋转角度
    public float animationDuration = 0.3f;   // 动画持续时间

    private Vector3 targetScale;
    private Vector3 initialScale;
    private Vector3 targetPosition;
    private Vector3 initialPosition;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        if(mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void initiate()
    {
        // 计算摄像头视野的高度和宽度
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraWidth = cameraHeight * screenAspect;

        // 获取Sprite的原始尺寸
        spriteRenderer = GetComponent<SpriteRenderer>();
        float spriteHeight = spriteRenderer.sprite.bounds.size.y;
        float spriteWidth = spriteRenderer.sprite.bounds.size.x;

        // 计算缩放比例，使得Sprite的高度与摄像头视野高度一致
        float scaleFactor = cameraHeight / spriteHeight;
        Vector3 newScale = new Vector3(scaleFactor, scaleFactor, 1f);
        targetScale = newScale;
        initialScale = newScale;

        // 设置Sprite的初始位置在摄像头右侧的外面
        float spriteHalfWidth = (initialScale.x) / 2f;
        float cameraRightEdge = mainCamera.transform.position.x + (cameraWidth / 2f);
        float initialX = cameraRightEdge + spriteHalfWidth + 1f; // 额外偏移1单位，确保在视野外
        float initialY = mainCamera.transform.position.y - cameraHeight / 2;
        Vector3 initialPos = new Vector3(initialX, initialY, 0); // z轴稍微偏前
        initialPosition = initialPos;

        // 目标位置为摄像头视野内的中心
        //targetPosition = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z - 1f);
        targetPosition = initialPos;
        // 设置初始状态
        transform.position = initialPosition;
        transform.localScale = initialScale;
        transform.rotation = Quaternion.identity;
        spriteRenderer.enabled = false; // 初始隐藏
    }
    public void appear()
    {
        StartCoroutine(AppearCoroutine());
    }

    private IEnumerator AppearCoroutine()
    {
        spriteRenderer.enabled = true; // 显示Sprite

        float elapsedTime = 0f;

        // 缩放从0到目标缩放
        Vector3 startScale = initialScale;
        Vector3 endScale = targetScale;

        // 位置从初始位置到目标位置
        Vector3 startPos = initialPosition;
        Vector3 endPos = targetPosition;

        // 旋转从0到-rotationAngle度（逆时针）
        Quaternion startRot = Quaternion.identity;
        Quaternion endRot = Quaternion.Euler(0, 0, rotationAngle);

        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;

            // 插值缩放
            transform.localScale = Vector3.Lerp(startScale, endScale, t);

            // 插值位置
            transform.position = Vector3.Lerp(startPos, endPos, t);

            // 插值旋转
            transform.rotation = Quaternion.Lerp(startRot, endRot, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保最终状态
        transform.localScale = endScale;
        transform.position = endPos;
        transform.rotation = endRot;
    }
}
