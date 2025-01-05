using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class fakePlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera mainCamera;
    public float rotationAngle = 20f;      // ��ת�Ƕ�
    public float animationDuration = 0.3f;   // ��������ʱ��

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
        // ��������ͷ��Ұ�ĸ߶ȺͿ��
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraWidth = cameraHeight * screenAspect;

        // ��ȡSprite��ԭʼ�ߴ�
        spriteRenderer = GetComponent<SpriteRenderer>();
        float spriteHeight = spriteRenderer.sprite.bounds.size.y;
        float spriteWidth = spriteRenderer.sprite.bounds.size.x;

        // �������ű�����ʹ��Sprite�ĸ߶�������ͷ��Ұ�߶�һ��
        float scaleFactor = cameraHeight / spriteHeight;
        Vector3 newScale = new Vector3(scaleFactor, scaleFactor, 1f);
        targetScale = newScale;
        initialScale = newScale;

        // ����Sprite�ĳ�ʼλ��������ͷ�Ҳ������
        float spriteHalfWidth = (initialScale.x) / 2f;
        float cameraRightEdge = mainCamera.transform.position.x + (cameraWidth / 2f);
        float initialX = cameraRightEdge + spriteHalfWidth + 1f; // ����ƫ��1��λ��ȷ������Ұ��
        float initialY = mainCamera.transform.position.y - cameraHeight / 2;
        Vector3 initialPos = new Vector3(initialX, initialY, 0); // z����΢ƫǰ
        initialPosition = initialPos;

        // Ŀ��λ��Ϊ����ͷ��Ұ�ڵ�����
        //targetPosition = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z - 1f);
        targetPosition = initialPos;
        // ���ó�ʼ״̬
        transform.position = initialPosition;
        transform.localScale = initialScale;
        transform.rotation = Quaternion.identity;
        spriteRenderer.enabled = false; // ��ʼ����
    }
    public void appear()
    {
        StartCoroutine(AppearCoroutine());
    }

    private IEnumerator AppearCoroutine()
    {
        spriteRenderer.enabled = true; // ��ʾSprite

        float elapsedTime = 0f;

        // ���Ŵ�0��Ŀ������
        Vector3 startScale = initialScale;
        Vector3 endScale = targetScale;

        // λ�ôӳ�ʼλ�õ�Ŀ��λ��
        Vector3 startPos = initialPosition;
        Vector3 endPos = targetPosition;

        // ��ת��0��-rotationAngle�ȣ���ʱ�룩
        Quaternion startRot = Quaternion.identity;
        Quaternion endRot = Quaternion.Euler(0, 0, rotationAngle);

        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;

            // ��ֵ����
            transform.localScale = Vector3.Lerp(startScale, endScale, t);

            // ��ֵλ��
            transform.position = Vector3.Lerp(startPos, endPos, t);

            // ��ֵ��ת
            transform.rotation = Quaternion.Lerp(startRot, endRot, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ȷ������״̬
        transform.localScale = endScale;
        transform.position = endPos;
        transform.rotation = endRot;
    }
}
