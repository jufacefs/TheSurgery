using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnClick : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AudioClip clip;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound()
    {
        SoundManager.Instance.playSound(clip);
    }
}
