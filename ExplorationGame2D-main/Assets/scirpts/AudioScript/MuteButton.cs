using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    private bool isInitialized = false;
    public static bool isMutting;
    public Toggle toggle;
    // Start is called before the first frame update
    void Start()
    {
        isInitialized = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInitialized)
        {
            isMutting = Settings.isMute == 1;
            transform.GetComponent<Toggle>().isOn = isMutting;
            isInitialized = true;
        }
    }

    public void onClick()
    {
        SoundManager.Instance.toggleMute();
        isMutting = !isMutting;
        //toggle.isOn = isMutting;
    }
}
