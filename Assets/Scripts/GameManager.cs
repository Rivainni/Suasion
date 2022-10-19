using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] MainUI mainUI;
    [SerializeField] PlayerMovement playerMovement;
    // Start is called before the first frame update

    public struct SaveFile
    {
        bool inPersuade;
        bool inIntro;
        float lastPValue;
        float lastMultiplier;
        int level;
    }

    float currentCorrect;
    float currentIncorrect;
    float persuasion = 30;
    float empathy = 30;
    float honesty = 0;
    int turn = 1;
    string currentCharacter = "";
    string mood = "neutral";
    bool inPersuade = false;
    bool inIntro = false;
    int level = 0;
    DialogueNode next = null;

    List<string> keywordList = new List<string>();
    List<Combination> combinationList = new List<Combination>();
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
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EndTurn()
    {
        Calculate();
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
                next = combination.NextNode;
                break;
            }
        }
        float calc = 0;

        if (inIntro)
        {
            calc = basePoints * (1 - (empathy * 0.01f));
            empathy += calc;
        }
        else if (inPersuade)
        {
            calc = basePoints * (1 + (empathy * 0.01f));
            if (mood == "neutral" && hMult == 0)
            {
                calc += 0.01f * 5;
            }
            else if (mood == "positive" && hMult == 1)
            {
                calc *= 0.01f * 15;
            }
            else if (mood == "negative")
            {
                calc *= 0.01f * 10;
            }
            persuasion += calc;
        }
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
        persuasion = 30;
        empathy = 30;
        turn = 1;
        currentCharacter = "";
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

    public DialogueNode GetNext()
    {
        return next;
    }

    public void ResetNext()
    {
        next = null;
    }

    public void LockMovement(bool status)
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
        Reset();
        ClearClues();
        level++;
        playerMovement.gameObject.transform.position = new Vector3(0, 0, 0);

        if (level == 0)
        {
            GameObject.Find("Tutorial").SetActive(false);
        }
        else
        {
            GameObject.Find("Level " + level).SetActive(true);
        }
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

    public bool Success()
    {
        // roll a random number between 0 and 100, then check if the random number is less than or equal to persuasion
        int rand = Random.Range(0, 100);
        if (rand <= persuasion)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
