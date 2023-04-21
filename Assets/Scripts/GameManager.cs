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
    [SerializeField] PCharacter[] characterProperties;
    [SerializeField] GameObject waypoints;
    [SerializeField] GameObject bossGate;
    [SerializeField] GameObject endingDialogue;

    Dictionary<string, int> levelObjectMapping = new Dictionary<string, int>();
    // Start is called before the first frame update

    float persuasion = 30;
    float empathy = 30;
    int turn = 1;
    float turnMultiplier = 0;
    float honestyMultiplier = 0.15f;
    string currentCharacter = "Friend";
    string mood = "neutral";
    bool inPersuade = false;
    bool inIntro = false;
    int level = 0;
    int score;
    int totalScore;
    int finalPlayerVotes = 0;
    int finalTotalVotes = 0;
    bool success = false;
    bool itemUnlocked = false;

    // for the log
    float currentValue = 0;
    string response = "";
    string currentHonesty;

    List<string> keywordList = new List<string>();
    List<Combination> combinationList = new List<Combination>();
    List<Log> logList = new List<Log>();
    List<Clue> clueList = new List<Clue>();
    List<PromoMaterial> itemList = new List<PromoMaterial>();

    // 0 = not yet attempted
    // 1 = attempting
    // 2 = failed
    // 3 = succeeded
    int[] finishedCharacters = new int[5];
    int[] levelScores = new int[5];
    int[] empathyScores = new int[5];
    int[] persuasionScores = new int[5];
    string[] characterOrder = new string[5];

    void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Main Game")
        {
            for (int i = 0; i < levelObjects.Length; i++)
            {
                levelObjectMapping.Add(levelObjects[i].name, i);
            }
            levelObjects[GetCharacterIndex(currentCharacter)].SetActive(true);
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
        string honesty = "";
        float hMult = 0;
        Debug.Log("You have " + keywordList.Count + " keywords");
        foreach (Combination combination in combinationList)
        {
            if (combination.CheckKeywords(keywordList, mood, CheckPersuade()) > 0)
            {
                basePoints += combination.CheckKeywords(keywordList, mood, CheckPersuade());
                honesty += combination.CheckHonesty();
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
            currentValue = basePoints;
            empathy = Mathf.Clamp(empathy + calc, 0, 100);
            empathyScores[GetCharacterIndex(currentCharacter)] += basePoints;
        }
        else if (inPersuade)
        {
            calc = basePoints * (1 + (empathy * 0.01f)) * turnMultiplier;
            currentValue = basePoints;
            currentHonesty = honesty;

            hMult = characterProperties[GetCharacterIndex(currentCharacter)].GetMultiplier(honesty, mood) * honestyMultiplier;

            calc += calc * hMult;
            persuasion = Mathf.Clamp(persuasion + calc, 0, 100);
            persuasionScores[GetCharacterIndex(currentCharacter)] += basePoints;
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

    public void SetMultiplier(float multiplier)
    {
        turnMultiplier = multiplier;
    }

    public void ModMultiplier()
    {
        float mult = 0;
        if (currentCharacter == "Baker")
        {
            if (CheckPersuaded("Farmer"))
            {
                mult += 0.5f;
            }
        }
        if (currentCharacter == "Vice Mayor")
        {
            if (CheckPersuaded("Baker"))
            {
                mult += 0.5f;
            }
            if (CheckPersuaded("Farmer"))
            {
                mult += 0.15f;
            }
            if (CheckPersuaded("Doctor"))
            {
                mult += 0.25f;
            }
        }

        SetMultiplier(turnMultiplier + (turnMultiplier * mult));
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
        characterOrder[GetCharacterIndex(currentCharacter)] = currentCharacter;
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

    public void EndGame()
    {
        mainUI.DisplayFinalScore();
    }

    public void ContinueToNextLevel()
    {
        Reset();
        ResetEnd();

        if (currentCharacter == "Friend")
        {
            levelObjects[level].SetActive(false);
            level++;
            levelObjects[level].SetActive(true);
        }
        else
        {
            level++;
        }


        if (CheckAllCharactersDone())
        {
            levelObjects[4].SetActive(true);
            bossGate.GetComponent<StoryElement>().enabled = true;
            Teleport("Boss");
        }
        else if (level > 1)
        {
            Teleport("Spawn");
        }
        else
        {
            Teleport("Level 1");
        }

        if (currentCharacter == "Vice Mayor")
        {
            RollVotes();
            EndGame();
        }

        foreach (GameObject renew in GameObject.FindGameObjectsWithTag("Respawn"))
        {
            renew.SetActive(true);
        }

        LockMovement(false);
        PauseTimer(false);
        timeController.ResetTime();
        totalScore += score;
        levelScores[GetCharacterIndex(GetCharacter())] = score;
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
        string comment = "They were in a " + mood + " mood.";
        if (state == "Persuasion")
        {
            logList.Add(new Log(keywordList, currentCharacter, state, currentValue, characterProperties[GetCharacterIndex(currentCharacter)].maxPersuasionScore, currentHonesty, (int)characterProperties[GetCharacterIndex(currentCharacter)].GetMultiplier(currentHonesty, mood), response, comment));
        }
        else if (state == "Introduction")
        {
            logList.Add(new Log(keywordList, currentCharacter, state, currentValue, characterProperties[GetCharacterIndex(currentCharacter)].maxEmpathyScore, "", 0, response, comment));
        }

    }

    public List<Log> GetLogs()
    {
        return logList;
    }

    public void GenerateItems()
    {
        int rand = Random.Range(1, 4);

        itemUnlocked = true;

        if (rand == 1)
        {
            itemList.Add(new PromoMaterial("Pamphlet", 0.05f, 3));
            itemList.Add(new PromoMaterial("Poster", 0.10f, 0));
            itemList.Add(new PromoMaterial("Assorted", 0.15f, 0));
            Debug.Log("Type 1");
        }
        else if (rand == 2)
        {
            itemList.Add(new PromoMaterial("Pamphlet", 0.05f, 3));
            itemList.Add(new PromoMaterial("Poster", 0.10f, 2));
            itemList.Add(new PromoMaterial("Assorted", 0.15f, 0));
            Debug.Log("Type 2");
        }
        else if (rand == 3)
        {
            itemList.Add(new PromoMaterial("Pamphlet", 0.05f, 3));
            itemList.Add(new PromoMaterial("Poster", 0.10f, 2));
            itemList.Add(new PromoMaterial("Assorted", 0.15f, 1));
            Debug.Log("Type 3");
        }
    }

    public void UseItem(float effect)
    {
        persuasion += persuasion * effect;
        mainUI.UpdateBarOnly();
    }

    public List<PromoMaterial> GetItems()
    {
        return itemList;
    }

    public bool CheckItemUnlock()
    {
        return itemUnlocked;
    }

    public bool GetSuccess()
    {
        return success;
    }

    public bool RollSuccess()
    {
        Random.seed = System.DateTime.Now.Millisecond;
        // roll a random number between 0 and 100, then check if the random number is less than or equal to persuasion
        int rand = Random.Range(0, 100);
        if (rand <= persuasion || currentCharacter == "Doctor")
        {
            success = true;
            finishedCharacters[GetCharacterIndex(currentCharacter)] = 3;
        }
        else
        {
            success = false;
            finishedCharacters[GetCharacterIndex(currentCharacter)] = 2;
        }

        return success;
    }

    public int GetScore()
    {
        return score;
    }

    public int GetTotalScore()
    {
        return totalScore;
    }

    public void PauseTimer(bool toggle)
    {
        timeController.SetPause(toggle);
    }

    public void HideTimer(bool toggle)
    {
        timeController.SetHide(toggle);
    }

    public void DisableCharacters()
    {

    }

    public void EnableCharacters()
    {

    }

    public bool CheckAllCharactersDone()
    {
        bool allDone = true;

        for (int i = 0; i < finishedCharacters.Length; i++)
        {
            if (finishedCharacters[i] < 2 && i != 4)
            {
                allDone = false;
                break;
            }
        }

        return allDone;
    }

    public int GetCharacterIndex(string character)
    {
        return levelObjectMapping[character];
    }

    public bool CheckPersuaded(string character)
    {
        if (finishedCharacters[GetCharacterIndex(character)] == 3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Teleport(string position)
    {
        GameObject wp = waypoints.transform.Find(position).gameObject;

        mainUI.Fade();

        if (wp != null)
        {
            playerMovement.gameObject.transform.position = wp.transform.position;
        }
    }

    // I didn't want to reference the story manager directly from the UI, so I made this method to call the story manager's ConfirmKeywords() method
    public void ConfirmKeywords()
    {
        storyManager.ConfirmKeywords();
    }

    public void RollVotes()
    {
        Random.seed = System.DateTime.Now.Millisecond;

        int playerVotes = 0;
        int totalVotes = 0;
        int currentVotes = 0;

        foreach (int status in finishedCharacters)
        {
            currentVotes = Random.Range(4, 6);
            totalVotes += currentVotes;

            if (status == 3)
            {
                playerVotes += currentVotes;
            }
            else if (status == 2)
            {
                playerVotes += Random.Range(0, 2);
            }
        }

        // Religion votes will depend on how many of the other groups you persuaded

        if (levelObjectMapping["Farmer"] == 3)
        {
            playerVotes += 5;
            totalVotes += 5;
        }
        if (levelObjectMapping["Vice Mayor"] == 3)
        {
            playerVotes += 5;
            totalVotes += 5;
        }

        finalPlayerVotes = playerVotes;
        finalTotalVotes = totalVotes;
    }

    public bool GetElectionStatus()
    {
        if (finalPlayerVotes > finalTotalVotes / 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetPlayerVotes()
    {
        return finalPlayerVotes;
    }

    public int GetTotalVotes()
    {
        return finalTotalVotes;
    }

    public void Leave()
    {
        storyManager.TriggerCutscene();
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.persuasion = persuasion;
        gameData.empathy = empathy;
        gameData.itemUnlocked = itemUnlocked;
        gameData.level = level;
        gameData.totalScore = totalScore;
        levelScores = gameData.levelScores;
        empathyScores = gameData.empathyScores;
        persuasionScores = gameData.persuasionScores;
        gameData.currentCharacter = currentCharacter;
        gameData.characterOrder = characterOrder;
        if (inPersuade)
        {
            gameData.inDialogue = "persuasion start";
        }
        else if (inIntro)
        {
            gameData.inDialogue = "intro start";
        }
        else
        {
            gameData.inDialogue = "exploration";
        }

        gameData.clues = clueList;
        gameData.logs = logList;
        gameData.items = itemList;
        gameData.mood = mood;

        for (int i = 0; i < finishedCharacters.Length; i++)
        {
            gameData.gameProgress[i] = finishedCharacters[i];
        }
    }

    public void LoadData(GameData gameData)
    {
        persuasion = gameData.persuasion;
        empathy = gameData.empathy;
        itemUnlocked = gameData.itemUnlocked;
        level = gameData.level;
        totalScore = gameData.totalScore;
        levelScores = gameData.levelScores;
        empathyScores = gameData.empathyScores;
        persuasionScores = gameData.persuasionScores;
        currentCharacter = gameData.currentCharacter;
        characterOrder = gameData.characterOrder;
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
            finishedCharacters[i] = gameData.gameProgress[i];

            // if friend character
            // else if vice mayor
            // else

            if (currentCharacter == "Friend")
            {
                if (i == GetCharacterIndex("Doctor") || i == GetCharacterIndex("Vice Mayor"))
                {
                    levelObjects[i].SetActive(false);
                }
                else if (gameData.gameProgress[i] == 0 || gameData.gameProgress[i] == 1)
                {
                    levelObjects[i].SetActive(true);
                }
            }
            else if (currentCharacter == "Vice Mayor")
            {
                if (gameData.gameProgress[i] == 0 || gameData.gameProgress[i] == 1)
                {
                    levelObjects[i].SetActive(true);
                }
                else
                {
                    levelObjects[i].SetActive(false);
                }
            }
            else
            {
                if ((gameData.gameProgress[i] == 0 || gameData.gameProgress[i] == 1) && i != GetCharacterIndex("Vice Mayor"))
                {
                    levelObjects[i].SetActive(true);
                }
                else
                {
                    levelObjects[i].SetActive(false);
                }
            }
        }
    }
}