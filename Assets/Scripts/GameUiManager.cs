using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUiManager : MonoBehaviour
{ 
    public static GameUiManager Instance { get; private set; }
    private void Awake()
    {
        // singleton pattern
        if (Instance != null)
        {
            Debug.Log("More than one player!!!!!!!!");
        }

        Instance = this;
    }
    
    //Take in Score TMPro Object
    [SerializeField] private TextMeshProUGUI playerScore;
    //Take in Pause TMPro Object

    // public static void AddPlayerScore(int numIngredient)
    // {
    //     int ingredientValue = 10;
    //     int addScore = numIngredient * ingredientValue;
    //     playerScore.text = "POINTS: " + addScore;
    // }
}
