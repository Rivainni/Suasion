using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] MainUI mainUI;
    [SerializeField] TextAsset keywordsFile;
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
    bool inPersuade = false;
    bool inIntro = false;

    List<TextRW.Keyword> keywordList = new List<TextRW.Keyword>();

    SaveFile current;
    void Start()
    {
        current = new SaveFile();
        TextRW.SetKeywords(keywordsFile);
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
        int correct = 0;
        int incorrect = 0;
        foreach (TextRW.Keyword keyword in keywordList)
        {
            if (keyword.Correct)
            {
                correct++;
            }
            else
            {
                incorrect++;
            }
        }
        float calc = (correct - incorrect) * 5.0f;

        if (inIntro)
        {
            Debug.Log("bruh" + calc);
            empathy += calc;
        }
        else if (inPersuade)
        {
            Debug.Log("bruh2" + calc);
            persuasion += calc;
        }
    }

    public void AddKeyword(TextRW.Keyword keyword)
    {
        keywordList.Add(keyword);
    }

    public void ClearKeywords()
    {
        keywordList.Clear();
    }

    public void Reset()
    {
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
}
