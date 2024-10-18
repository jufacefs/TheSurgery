using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionDropdown : MonoBehaviour
{
    private bool isInitialized;
    private string resolution;
    public Dropdown dropdown;
    // Start is called before the first frame update
    void Start()
    {
        isInitialized = false;
        dropdown.onValueChanged.AddListener(dropdownOnChanged);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInitialized)
        {
            isInitialized=true;
            resolution = Settings.curResolution;
            for(int i =0;i<dropdown.options.Count;i++)
            {
                if(resolution == dropdown.options[i].text)
                {
                    dropdown.value = i; break;
                }
            }
        }
    }

    public void dropdownOnChanged(int value)
    {
        resolution = dropdown.options[value].text;
        Settings.curResolution = resolution;
        (int,int) res = Settings.resMap[resolution];
        Screen.SetResolution(res.Item1, res.Item2, Settings.isFullScreen == 1);
    }
}
