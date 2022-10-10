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
    }

    float currentCorrect;
    float currentIncorrect;
    float persuasion = 30;
    float empathy = 30;
    int turn = 1;
    string currentCharacter = "";
    string mood;
    bool inPersuade = false;
    bool inIntro = false;
    DialogueNode next = null;

    List<string> keywordList = new List<string>();
    List<Combination> combinationList = new List<Combination>();

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

    public void StartIntro()
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
        foreach (Combination combination in combinationList)
        {
            if (combination.CheckKeywords(keywordList, mood) > 0)
            {
                basePoints += combination.CheckKeywords(keywordList, mood);
                next = combination.NextNode;
                break;
            }
        }
        float calc = 0;

        if (inIntro)
        {
            calc = basePoints * (1 - (empathy * 0.01f));
            Debug.Log("bruh" + calc);
            empathy += calc;
        }
        else if (inPersuade)
        {
            calc = basePoints * (1 - (persuasion * 0.01f));
            Debug.Log("bruh2" + calc);
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
}
