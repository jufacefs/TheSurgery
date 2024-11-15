using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickToClose : MonoBehaviour
{
    // Start is called before the first frame update
    private static bool beginListening1;
    private static bool beginListening2;
    void Start()
    {
        beginListening1 = false;
        beginListening2 = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (beginListening1 && Input.GetMouseButtonDown(0))
        {
            beginListening2 = true;
        }
        if (beginListening1 && beginListening2)
        {
            beginListening1 = false;
            beginListening2 = false;
            ZoomingController.Instance.textUI.SetActive(false);
        }

    }

    public void closeTextUI() {
        ZoomingController.Instance.closeTextUI();
    }

    static public void beginListening(bool b)
    {
        beginListening1 = b;
    }

    
}
