using UnityEngine;

/// <summary>

/// ʵ���������Ч��

/// </summary>

[ExecuteInEditMode]     // �༭״̬��ִ�и���

[RequireComponent(typeof(Camera))]  // Ҫ�������

public class WaveWarp : MonoBehaviour
{

    public Shader curShader;                // shader��������

    public Texture noise;                   // ���ͼƬ

    [Range(0.0f, 1.0f)]

    public float grayScaleAmount = 0.1f;    // ��ͼŤ����Χ����

    [Range(0.0f, 1.0f)]

    public float DistortTimeFactor = 0.15f; // ��ͼŤ�����˶�̬�仯�ٶȲ���

    private Material curMaterial;           // ���ʲ���

    public Material material                // ���ʲ�������

    {

        get

        {

            // ����Ϊ�գ��½����ʣ����������� shader

            if (curMaterial == null)

            {

                curMaterial = new Material(curShader);

                curMaterial.hideFlags = HideFlags.HideAndDontSave;

            }

            return curMaterial;

        }

    }

    void Start()

    {

        // �ж�ϵͳ�Ƿ�֧��ͼ����Ч

        if (SystemInfo.supportsImageEffects == false)

        {

            enabled = false;

            return;

        }

        // �жϵ�ǰshader �Ƿ�Ϊ�գ��Ƿ�֧��

        if (curShader != null && curShader.isSupported == false)

        {

            enabled = false;

        }

    }

    /// <summary>

    /// ͼƬ��Ⱦ����

    /// Camera��һ���ص���message����������cameraִ����Ⱦʱ�򱻵���

    /// </summary>

    /// <param name="sourceTexture">ԭͼƬ</param>

    /// <param name="destTexture">Ŀ��ͼƬ</param>

    void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)

    {

        // �жϵ�ǰshader�Ƿ�Ϊ��

        // ��Ϊ�������ÿ��Ƹ�ֵ���õ�shader������������Ⱦ

        // Ϊ���򲻴� Material ��Ⱦ

        if (curShader != null)

        {

            material.SetFloat("_LuminosityAmount", grayScaleAmount);

            material.SetFloat("_DistortTimeFactor", DistortTimeFactor);

            material.SetTexture("_NoiseTex", noise);

            Graphics.Blit(sourceTexture, destTexture, material);

        }

        else

        {

            Graphics.Blit(sourceTexture, destTexture);

        }

    }

    void Update()

    {

        // ��ֵ֤��Խ����[0,1]��Χ

        grayScaleAmount = Mathf.Clamp(grayScaleAmount, 0.0f, 1.0f);

    }

    void OnDisable()

    {

        // ʧЧʱ�����½��Ĳ���

        if (curMaterial != null)

        {

            DestroyImmediate(curMaterial);

        }

    }

}

