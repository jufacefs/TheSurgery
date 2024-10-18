using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AudioClip clip;
    void Start()
    {
        SoundManager.Instance.playSound(clip);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
