using System;
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
    public string inDialogue;

    public List<Clue> clues;
    public List<Log> logs;
    public string mood;

    public bool[] gameProgress;
    public List<bool> currentCharacterProgress;

    // Player
    public Vector2 playerPosition;

    // Time
    public string time;

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
        playerPosition = new Vector2(2.6f, -9.48f);
        inDialogue = "exploration";
        gameProgress = new bool[5];
        gameProgress[0] = true;
        currentCharacterProgress = new List<bool>();
        time = "09:00";
    }
}
