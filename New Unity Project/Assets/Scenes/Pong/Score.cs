﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text Scoreboard;
    private GameObject minge;
    private int Bat_1_Score = 0;
    private int Bat_2_Score = 0;
    // Start is called before the first frame update
    void Start()
    {
        minge = GameObject.Find("Ball");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(minge.transform.position.x);
        if (minge.transform.position.x >= 14f && Bat_1_Score<5)
        {
            Bat_1_Score++;
           
        }
        if (minge.transform.position.x <= -14f&& Bat_2_Score<5)
        {
            Bat_2_Score++;
          
        }

        if (Bat_1_Score < 5 && Bat_2_Score < 5)
        {
            Scoreboard.text = Bat_1_Score + " - " + Bat_2_Score;
            
        }
        else
        {
            if (Bat_1_Score >= 5)
                Scoreboard.text = "LEFT WINS";
            else
                Scoreboard.text = "RIGHT WINS";
        }
    }
}