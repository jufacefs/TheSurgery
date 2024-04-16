using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThingsEaten : MonoBehaviour
{
    public int NumThingsEaten = 0;

    public Text EatenOutput;
    void Update()
    {
        EatenOutput.text = "Eaten: " + NumThingsEaten;
    }

    public void EatThings()
    {

        Debug.Log("shows in function ThingsEaten");
        NumThingsEaten++;
    }
    public void UsePotion()
    {
        NumThingsEaten --;
    }
}