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
    public List<PromoMaterial> items;
    public string mood;

    public int[] gameProgress;
    public List<int> currentCharacterProgress;

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
        gameProgress = new int[5];
        gameProgress[0] = 1;
        currentCharacterProgress = new List<int>();
        time = "09:00";
        clues = new List<Clue>();
        logs = new List<Log>();
        items = new List<PromoMaterial>();
    }
}
