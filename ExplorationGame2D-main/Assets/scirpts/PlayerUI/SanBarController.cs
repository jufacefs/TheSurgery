using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SanBarController : MonoBehaviour
{
    // Start is called before the first frame update
    public Image sanImg;
    public Image sanEffectImg;

    public float maxSan = 100f;
    public float curSan;
    public float buffTime;

    private Coroutine updateCoroutine;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateSanBar()
    {
        sanImg.fillAmount = curSan/maxSan;

        if (updateCoroutine != null )
        {
            StopCoroutine( updateCoroutine );
        }

        updateCoroutine = StartCoroutine(updateSanEffect());
    }

    private IEnumerator updateSanEffect()
    {
        float effectLength = sanEffectImg.fillAmount - sanImg.fillAmount;
        float elapsedTime = 0;

        if (elapsedTime < buffTime && effectLength != 0)
        {
            elapsedTime += Time.deltaTime;
            sanEffectImg.fillAmount = Mathf.Lerp(sanImg.fillAmount + effectLength, sanImg.fillAmount, elapsedTime / buffTime);
            yield return null;
        }
        sanEffectImg.fillAmount = sanImg.fillAmount;
    }

    public void setSan(float san)
    {
        curSan = Mathf.Clamp(san, 0, maxSan);
        UpdateSanBar();

        if(curSan <= 0)
        {
            //is dead
        }
    }
}
