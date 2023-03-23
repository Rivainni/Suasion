using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    // Game Manager
    public float persuasion;
    public float empathy;
    public int level;
    public int totalScore;
    public string currentCharacter;
    public string progress;

    // Serialize clues and logs
    public string mood;

    // 

    public GameData()
    {
        ResetAll();
    }

    public void ResetAll()
    {
        persuasion = 30;
        empathy = 30;
        level = 0;
        totalScore = 0;
        currentCharacter = "";
        mood = "neutral";
    }
}
