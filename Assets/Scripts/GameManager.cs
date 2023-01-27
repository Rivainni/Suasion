using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] MainUI mainUI;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] TimeController timeController;
    [SerializeField] GameObject[] levelObjects;
    // Start is called before the first frame update

    public struct SaveFile
    {
        bool inPersuade;
        bool inIntro;
        float lastPValue;
        float lastMultiplier;
        int level;
    }

    float persuasion = 30;
    float empathy = 30;
    float honesty = 0;
    int turn = 1;
    int turnMultiplier = 0;
    string currentCharacter = "";
    string mood = "neutral";
    bool inPersuade = false;
    bool inIntro = false;
    int level = 0;
    int score;
    bool success = false;

    // for the log
    float currentValue = 0;
    string response = "";

    List<string> keywordList = new List<string>();
    List<Combination> combinationList = new List<Combination>();
    List<Log> logList = new List<Log>();
    List<Clue> clueList = new List<Clue>();

    SaveFile current;
    void Start()
    {
        current = new SaveFile();
        // TextRW.SetKeywords(keywordsFile);
        // foreach (TextRW.Keyword keyword in TextRW.GetKeywords())
        // {
        //     Debug.Log(keyword.Word);
        //     Debug.Log(keyword.Category);
        //     Debug.Log(keyword.Correct);
        // }
        logList.Clear();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EndTurn()
    {
        Calculate();
        if (inPersuade)
        {
            AddLog("Persuasion");
        }
        else if (inIntro)
        {
            AddLog("Introduction");
        }
        ClearKeywords();
        AddTurn();
    }

    public void Calculate()
    {
        int basePoints = 0;
        int hMult = 0;
        Debug.Log("You have " + keywordList.Count + " keywords");
        foreach (Combination combination in combinationList)
        {
            if (combination.CheckKeywords(keywordList, mood, CheckPersuade()) > 0)
            {
                basePoints += combination.CheckKeywords(keywordList, mood, CheckPersuade());
                hMult += combination.CheckHonesty();
                // next = combination.NextNode;
                // response = combination.NextNode.NarrationLine.Text;
                break;
            }
        }
        float calc = 0;
        score += basePoints;

        if (inIntro)
        {
            calc = basePoints * turnMultiplier;
            empathy = Mathf.Clamp(empathy + calc, 0, 100);
        }
        else if (inPersuade)
        {
            calc = basePoints * (1 + (empathy * 0.01f)) * turnMultiplier;
            if (mood == "neutral" && hMult == 0)
            {
                calc += 5;
            }
            else if (mood == "positive" && hMult == 1)
            {
                calc *= 15;
            }
            else if (mood == "negative")
            {
                calc *= 10;
            }

            if (hMult > 0)
            {
                honesty += 0.5f;
            }
            persuasion = Mathf.Clamp(persuasion + calc, 0, 100);
        }
        Debug.Log("Empathy: " + empathy);
        Debug.Log("Persuasion ni Bossing: " + persuasion);
    }

    public void AddKeyword(string keyword)
    {
        keywordList.Add(keyword);
    }

    public void RemoveKeyword(string keyword)
    {
        keywordList.Remove(keyword);
    }

    public void ClearKeywords()
    {
        keywordList.Clear();
    }

    public bool ContainsKeyword(string keyword)
    {
        return keywordList.Contains(keyword);
    }

    public void AddCombination(Combination combination)
    {
        combinationList.Add(combination);
    }

    public void ClearCombinations()
    {
        combinationList.Clear();
    }

    public void Reset()
    {
        ClearCombinations();
        ClearKeywords();
        turn = 1;
        currentCharacter = "";
    }

    public void ResetEnd()
    {
        persuasion = 30;
        empathy = 30;
    }

    // getter/setter methods below
    public int GetTurn()
    {
        return turn;
    }

    public void AddTurn()
    {
        turn++;
    }

    public void SetMultiplier(int multiplier)
    {
        turnMultiplier = multiplier;
    }

    public bool CheckIntro()
    {
        return inIntro;
    }

    public void SetIntro(bool set)
    {
        inIntro = set;
    }

    public bool CheckPersuade()
    {
        return inPersuade;
    }

    public void SetPersuade(bool set)
    {
        inPersuade = set;
    }

    public string GetCharacter()
    {
        return currentCharacter;
    }

    public void SetCharacter(string character)
    {
        currentCharacter = character;
    }

    public float GetPersuasion()
    {
        return persuasion;
    }

    public float GetEmpathy()
    {
        return empathy;
    }

    public void LockMovement(bool status)
    {
        if (SceneManager.GetActiveScene().name == "Main Game")
        {
            if (status)
            {
                playerMovement.LockMovement();
            }
            else
            {
                playerMovement.UnlockMovement();
            }
        }
    }

    public void RandomiseMood()
    {
        int rand = Random.Range(0, 3);
        if (rand == 0)
        {
            mood = "positive";
        }
        else if (rand == 1)
        {
            mood = "neutral";
        }
        else
        {
            mood = "negative";
        }
    }

    public int GetLevel()
    {
        return level;
    }

    public void AddLevel()
    {
        mainUI.DisplayCurrentScore();
        playerMovement.gameObject.transform.position = new Vector3(0, 0, 0);
        Reset();
        ResetEnd();
        // ClearClues();

        levelObjects[level].SetActive(false);
        level++;
        levelObjects[level].SetActive(true);

        foreach (GameObject renew in GameObject.FindGameObjectsWithTag("Respawn"))
        {
            renew.SetActive(true);
        }
    }

    public void AddClue(string name, string description, string character)
    {
        clueList.Add(new Clue(name, description, character));
    }

    public void ClearClues()
    {
        clueList.Clear();
    }

    public List<Clue> GetClues()
    {
        return clueList;
    }

    public void AddLog(string state)
    {
        logList.Add(new Log(keywordList, state, currentValue, response, mood));
    }

    public List<Log> GetLogs()
    {
        return logList;
    }

    public bool GetSuccess()
    {
        return success;
    }

    public void RollSuccess()
    {
        // roll a random number between 0 and 100, then check if the random number is less than or equal to persuasion
        int rand = Random.Range(0, 100);
        if (rand <= persuasion)
        {
            success = true;
        }
        else
        {
            success = false;
        }
    }

    public int GetScore()
    {
        return score;
    }
    public void PauseTimer(bool toggle)
    {
        timeController.SetPause(toggle);
    }

    public void HideTimer(bool toggle)
    {
        timeController.SetHide(toggle);
    }

    // I didn't want to reference the story manager directly from the UI, so I made this method to call the story manager's ConfirmKeywords() method
    public void ConfirmKeywords()
    {
        gameObject.GetComponent<StoryManager>().ConfirmKeywords();
    }
}
