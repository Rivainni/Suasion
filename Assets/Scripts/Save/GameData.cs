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

    public List<Clue> clues;
    public List<Log> logs;
    public int[] finishedCharacters;
    public string mood;

    // Player
    public Vector2 playerPosition;

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
        playerPosition = new Vector2(0, 0);
    }
}
