using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class dialoguePopController : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI uiText;
    public float delayTime = 0.05f;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowDialogue(string dialogue)
    {
        StopAllCoroutines(); // 停止之前的协程，防止重叠
        Debug.Log(dialogue + "should show on the pop");
        StartCoroutine(TypeText(dialogue, delayTime));
    }

    private IEnumerator TypeText(string fullText, float delay)
    {
        uiText.text = "";
        foreach (char c in fullText)
        {
            uiText.text += c;
            yield return new WaitForSeconds(delay);
        }
    }


}
