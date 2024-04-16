using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSprite : MonoBehaviour
{
    public Sprite sp1, sp2,sp3,sp4;
    public DialogueManager DialogueManager;

    
    private void Update()
    {
        if (DialogueManager.score < 0)
        {
            GetComponent<SpriteRenderer>().sprite = sp1;
        }
        if (DialogueManager.score <-1)
        {
            GetComponent<SpriteRenderer>().sprite = sp2;
        }
        if (DialogueManager.score <-2)
        {
            GetComponent<SpriteRenderer>().sprite = sp3;
        }
        if (DialogueManager.score < -3)
        {
            GetComponent<SpriteRenderer>().sprite = sp4;
        }

    }
}
