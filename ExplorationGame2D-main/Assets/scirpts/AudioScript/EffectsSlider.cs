using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class EffectsSlider : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Slider _slider;
    private bool isInitialized = false;
    void Start()
    {
        isInitialized = false;
        _slider.onValueChanged.AddListener(val => SoundManager.Instance.ChangeEffectsVolume(val));
    }


    // Update is called once per frame
    void Update()
    {
        if (!isInitialized)
        {
            isInitialized = true;
            _slider.value = Settings.EffectsVolume;
        }
    }
}
