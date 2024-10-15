using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Settings : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isActive = false;
    public Vector2 curResolution;
    public bool curIsFullScreen;
    public GameObject generalPanel;
    public GameObject controlsPanel;

    void Start()
    {
        curResolution = new Vector2(1920, 1080);
        curIsFullScreen = true;


        updateScreen();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateScreen()
    {
        Screen.SetResolution((int)curResolution.x, (int)curResolution.y, curIsFullScreen);
    }
}
