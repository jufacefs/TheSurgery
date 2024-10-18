using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenButton : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isInitialized = false;
    public static bool isFullScreen;
    void Start()
    {
        isInitialized = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInitialized)
        {
            isFullScreen = Settings.isMute == 1;
            transform.GetComponent<Toggle>().isOn = isFullScreen;
            isInitialized = true;
        }
    }

    public void onClick()
    {
        isFullScreen = !isFullScreen;
        transform.GetComponent<Toggle>().isOn = isFullScreen;
        Screen.fullScreen = !Screen.fullScreen;
        Settings.isFullScreen = Settings.isFullScreen == 1 ? 0 : 1;
    }
}
