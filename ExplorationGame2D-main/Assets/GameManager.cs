using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int score = 0;

    public Text PotionsOutput;
    void Update()
    {
        PotionsOutput.text = "Eaten: " + score;
    }

    public void LoseScore(int number)

    {
        Debug.Log("you've lost 1 point");
        score-=number;
        //if ( score <-1){
        //    Player.ChangeSprite

        //}
    }
    public void AddScore(int number)
    {
        score++;
    }
}