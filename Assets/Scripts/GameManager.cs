using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] MainUI mainUI;
    [SerializeField] StoryManager storyManager;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] TimeController timeController;
    [SerializeField] GameObject[] levelObjects;
    // Start is called before the first frame update

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
    int totalScore;
    bool success = false;
    bool itemUnlocked = false;

    // for the log
    float currentValue = 0;
    string response = "";

    List<string> keywordList = new List<string>();
    List<Combination> combinationList = new List<Combination>();
    List<Log> logList = new List<Log>();
    List<Clue> clueList = new List<Clue>();
    List<PromoMaterial> itemList = new List<PromoMaterial>();
    int[] finishedCharacters = new int[5];

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Main Game")
        {
            levelObjects[level].SetActive(true);
        }
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
                currentValue = basePoints * hMult;
                response = combination.Response;
                storyManager.SetChoice(combination.Choice);
                storyManager.SetResponse(combination.Response);
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

    public int GetKeywordsCount()
    {
        return keywordList.Count;
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
        Random.seed = System.DateTime.Now.Millisecond;
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

    public string GetMood()
    {
        return mood;
    }

    public int GetLevel()
    {
        return level;
    }

    public void AddLevel()
    {
        mainUI.DisplayCurrentScore();
    }

    public void ContinueToNextLevel()
    {
        Reset();
        ResetEnd();
        finishedCharacters[level] = 1;
        if (level < 1)
        {
            levelObjects[level].SetActive(false);
            level++;
            levelObjects[level].SetActive(true);
        }
        else
        {
            level++;
        }


        if (level > 1)
        {
            playerMovement.gameObject.transform.position = new Vector3(30.25f, -9.48f, 0);
        }
        else
        {
            playerMovement.gameObject.transform.position = new Vector3(2.6f, -3.62f, 0);
        }

        foreach (GameObject renew in GameObject.FindGameObjectsWithTag("Respawn"))
        {
            renew.SetActive(true);
        }

        LockMovement(false);

        totalScore += score;
        score = 0;
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

    public bool CheckClues(string name)
    {
        foreach (Clue clue in clueList)
        {
            if (clue.name == name)
            {
                return true;
            }
        }
        return false;
    }

    public void AddLog(string state)
    {
        logList.Add(new Log(keywordList, state, currentValue, response, mood));
    }

    public List<Log> GetLogs()
    {
        return logList;
    }

    public void GenerateItems()
    {
        int rand = Random.Range(0, 4);

        itemUnlocked = true;

        if (rand == 1)
        {
            itemList.Add(new PromoMaterial("Pamphlet", 5, 3));
            itemList.Add(new PromoMaterial("Poster", 10, 0));
            itemList.Add(new PromoMaterial("Assorted", 15, 0));
        }
        else if (rand == 2)
        {
            itemList.Add(new PromoMaterial("Pamphlet", 5, 3));
            itemList.Add(new PromoMaterial("Poster", 10, 2));
            itemList.Add(new PromoMaterial("Assorted", 15, 0));
        }
        else if (rand == 3)
        {
            itemList.Add(new PromoMaterial("Pamphlet", 5, 3));
            itemList.Add(new PromoMaterial("Poster", 10, 2));
            itemList.Add(new PromoMaterial("Assorted", 15, 1));
        }
    }

    public void UseItem(float effect)
    {
        persuasion += persuasion * effect;
    }

    public List<PromoMaterial> GetItems()
    {
        return itemList;
    }

    public bool GetSuccess()
    {
        return success;
    }

    public bool RollSuccess()
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

        return success;
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
        storyManager.ConfirmKeywords();
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.persuasion = persuasion;
        gameData.empathy = empathy;
        gameData.level = level;
        gameData.totalScore = totalScore;
        gameData.currentCharacter = currentCharacter;
        if (inPersuade)
        {
            gameData.inDialogue = "persuasion";
        }
        else if (inIntro)
        {
            gameData.inDialogue = "intro";
        }
        else
        {
            gameData.inDialogue = "exploration";
        }

        gameData.clues = clueList;
        gameData.logs = logList;
        gameData.items = itemList;
        gameData.mood = mood;

        for (int i = 0; i < levelObjects.Length; i++)
        {
            gameData.gameProgress[i] = levelObjects[i].activeSelf;
        }
    }

    public void LoadData(GameData gameData)
    {
        persuasion = gameData.persuasion;
        empathy = gameData.empathy;
        level = gameData.level;
        totalScore = gameData.totalScore;
        currentCharacter = gameData.currentCharacter;
        if (gameData.inDialogue == "persuasion start")
        {
            inPersuade = true;
        }
        else if (gameData.inDialogue == "intro start")
        {
            inIntro = true;
        }
        else
        {
            inPersuade = false;
            inIntro = false;
        }

        clueList = gameData.clues;
        logList = gameData.logs;
        itemList = gameData.items;
        mood = gameData.mood;

        for (int i = 0; i < levelObjects.Length; i++)
        {
            levelObjects[i].SetActive(gameData.gameProgress[i]);
        }
    }
}