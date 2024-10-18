using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static SoundManager Instance;
    private bool isInitialized = false;

    [SerializeField] private AudioSource musicSouse, effectSource;
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isInitialized)
        {
            effectSource.volume = Settings.EffectsVolume;
            musicSouse.volume = Settings.BGMVolume;
            effectSource.mute = Settings.isMute == 1;
            musicSouse.mute = Settings.isMute == 1;
        }
    }

    public void playSound(AudioClip clip)
    {
        effectSource.PlayOneShot(clip);
    }

    public void ChangeMasterVolume(float value)
    {
        AudioListener.volume = value;
    }

    public void ChangeEffectsVolume(float value)
    {
        effectSource.volume = value;
        Settings.EffectsVolume = value;
    }

    public void ChangeBGMVolume(float value)
    {
        musicSouse.volume = value;
        Settings.BGMVolume = value;
    }

    public void toggleMute()
    {
        effectSource.mute = !effectSource.mute;
        musicSouse.mute = !musicSouse.mute;
        Settings.isMute = Settings.isMute == 1 ? 0:1;
    }
}
