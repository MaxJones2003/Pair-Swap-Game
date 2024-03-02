using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int score = 0;

    private static readonly int[] Scores = new int[]
    {
        5, // small
        7, // big
        9, // abductor
        11 // dropper
    };


    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void IncreaseScore(EEnemyType type)
    {
        score += Scores[(int)type];
    }

    
}
