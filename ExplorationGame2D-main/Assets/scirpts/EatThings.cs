using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatThings : MonoBehaviour
{
    public GameManager GM;

    public void OnTriggerEnter2D(Collider2D TheThingThatWalkedIntoMe)
    {
        if (TheThingThatWalkedIntoMe.name == "Player")
        {
            Debug.Log("You've eaten something");
            GM.LoseScore(1);
            Destroy(gameObject);
        }
    }
}
