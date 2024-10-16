using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class buttonKeyController : MonoBehaviour
{
    // Start is called before the first frame update
    static List<GameObject> buttons;
    private bool isInitialized = false;
    void Start()
    {
        if(buttons == null)
        {
            buttons = new List<GameObject>();
        }
        buttons.Add(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isInitialized)
        {
            string curKeyName = Settings.controlKeys[gameObject.name];
            if(curKeyName != null && curKeyName!="")
            {
                Settings.setButtonText(gameObject,curKeyName);
                isInitialized = true;
            }
        }
    }
}
