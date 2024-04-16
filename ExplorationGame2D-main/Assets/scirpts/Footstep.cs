using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footstep : MonoBehaviour

{
    public GameObject footstep;
    public GameObject RunSound;

    // Start is called before the first frame update
    void Start()
    {
        footstep.SetActive(false);
        RunSound.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (
            Input.GetKey("a") || Input.GetKey("d")
            || Input.GetKey("left") ||Input.GetKey("right"))
        {
            footsteps();
        }

        

        if (
            Input.GetKeyUp("a") || Input.GetKeyUp("d")||
            Input.GetKeyUp("left") || Input.GetKeyUp("right"))
        {
            StopFootsteps();
        }

    }

    void footsteps()
    {
        if (DialogueManager.score < 3)
        {
            footstep.SetActive(true);
        }
        if (DialogueManager.score >= 3)
        {
            RunSound.SetActive(true);
        }

    }

    void StopFootsteps()
    {
        if (DialogueManager.score < 3)
        {
            footstep.SetActive(false);
        }
        if(DialogueManager.score>=3)
        {
            RunSound.SetActive(false);
        }
    }
}