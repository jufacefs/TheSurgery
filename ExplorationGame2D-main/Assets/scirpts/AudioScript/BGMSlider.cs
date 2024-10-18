using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGMSlider : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    private bool isInitialized = false;
    void Start()
    {
        isInitialized = false;
        _slider.onValueChanged.AddListener(val => SoundManager.Instance.ChangeBGMVolume(val));
    }

    // Update is called once per frame
    void Update()
    {
        if(!isInitialized) 
        { 
            isInitialized = true;
            _slider.value = Settings.BGMVolume;
        }
    }
}
