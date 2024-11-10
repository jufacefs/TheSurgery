using UnityEngine;

/// <summary>

/// 实现相机热浪效果

/// </summary>

[ExecuteInEditMode]     // 编辑状态下执行该类

[RequireComponent(typeof(Camera))]  // 要求有相机

public class WaveWarp : MonoBehaviour
{

    public Shader curShader;                // shader变量参数

    public Texture noise;                   // 噪点图片

    [Range(0.0f, 1.0f)]

    public float grayScaleAmount = 0.1f;    // 主图扭曲范围参数

    [Range(0.0f, 1.0f)]

    public float DistortTimeFactor = 0.15f; // 主图扭曲热浪动态变化速度参数

    private Material curMaterial;           // 材质参数

    public Material material                // 材质参数属性

    {

        get

        {

            // 材质为空，新建材质，并赋予热浪 shader

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

        // 判断系统是否支持图像特效

        if (SystemInfo.supportsImageEffects == false)

        {

            enabled = false;

            return;

        }

        // 判断当前shader 是否为空，是否支持

        if (curShader != null && curShader.isSupported == false)

        {

            enabled = false;

        }

    }

    /// <summary>

    /// 图片渲染函数

    /// Camera的一个回调（message），他会在camera执行渲染时候被调用

    /// </summary>

    /// <param name="sourceTexture">原图片</param>

    /// <param name="destTexture">目标图片</param>

    void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)

    {

        // 判断当前shader是否为空

        // 不为空则设置控制赋值对用的shader参数，进行渲染

        // 为空则不带 Material 渲染

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

        // 保证值不越界在[0,1]范围

        grayScaleAmount = Mathf.Clamp(grayScaleAmount, 0.0f, 1.0f);

    }

    void OnDisable()

    {

        // 失效时销毁新建的材质

        if (curMaterial != null)

        {

            DestroyImmediate(curMaterial);

        }

    }

}

