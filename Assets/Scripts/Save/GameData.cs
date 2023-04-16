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
    public bool itemUnlocked;
    public int level;
    public int totalScore;
    public string currentCharacter;
    public string inDialogue;

    public List<Clue> clues;
    public List<Log> logs;
    public List<PromoMaterial> items;
    public string mood;

    public int[] gameProgress;
    public int[] levelScores;
    public int[] empathyScores;
    public int[] persuasionScores;
    public string[] characterOrder;
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
        itemUnlocked = false;
        level = 0;
        totalScore = 0;
        currentCharacter = "Friend";
        mood = "neutral";
        playerPosition = new Vector2(2.6f, -9.48f);
        inDialogue = "exploration";
        gameProgress = new int[5];
        gameProgress[0] = 1;
        levelScores = new int[5];
        empathyScores = new int[5];
        persuasionScores = new int[5];
        characterOrder = new string[5];
        currentCharacterProgress = new List<int>();
        time = "09:00";
        clues = new List<Clue>();
        logs = new List<Log>();
        items = new List<PromoMaterial>();
    }
}