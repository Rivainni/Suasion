using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainUI : MonoBehaviour
{
    [SerializeField] GameObject answerPanel;
    [SerializeField] GameObject dialogueScreen;
    [SerializeField] GameObject keywordPanel;
    [SerializeField] GameObject targetBox;
    [SerializeField] GameObject mcBox;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] GameObject togglePrefab;
    [SerializeField] HealthBar persuasionBar;
    [SerializeField] HealthBar empathyBar;
    [SerializeField] GameManager gameManager;
    [SerializeField] TextMeshProUGUI notebookText;
    List<string> proscriptionList = new List<string>();

    GameObject kaboom;

    // Start is called before the first frame update
    void Start()
    {
        persuasionBar.SetHealth(30);
        empathyBar.SetHealth(30);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EndTurn()
    {
        gameManager.EndTurn();
        if (gameManager.CheckIntro())
        {
            empathyBar.SetHealth(gameManager.GetEmpathy());
            Destroy(kaboom);
        }
        else if (gameManager.CheckPersuade())
        {
            persuasionBar.SetHealth(gameManager.GetPersuasion());
            Destroy(kaboom);
        }
    }

    public void DisplayKeywords(KeywordSet keywordSet, string type)
    {

        // for blocking keywords in tutorial
        if (gameManager.CheckIntro() && gameManager.GetLevel() == 0)
        {
            if (gameManager.GetTurn() == 1)
            {
                proscriptionList.Add("game");
                proscriptionList.Add("weather");
                proscriptionList.Add("demanding");
                proscriptionList.Add("cheerful");
            }
            if (gameManager.GetTurn() == 2)
            {
                proscriptionList.Add("inside");
                proscriptionList.Add("inspect");
                proscriptionList.Add("demanding");
            }
        }
        else if (gameManager.CheckPersuade() && gameManager.GetLevel() == 0)
        {
            if (gameManager.GetTurn() == 1)
            {
                proscriptionList.Add("state");
                proscriptionList.Add("weather");
                proscriptionList.Add("assertive");
                proscriptionList.Add("cautious");
                proscriptionList.Add("exaggerate");
                proscriptionList.Add("downplay");
            }
        }

        int buttonIndex = 0;

        foreach (Button button in keywordPanel.GetComponentsInChildren<Button>())
        {
            button.interactable = true;
        }

        foreach (string keyword in keywordSet.Topic)
        {
            Button button = keywordPanel.transform.GetChild(buttonIndex).GetComponent<Button>();
            button.GetComponentInChildren<TextMeshProUGUI>().text = keyword;

            if (proscriptionList.Contains(keyword))
            {
                button.interactable = false;
            }
            else
            {
                button.onClick.AddListener(delegate { LockKeyword(button, keyword); });
            }
        }

        proscriptionList.Clear();
    }

    public void SwitchKeywords(string keyword, string type, bool isKeyword)
    {
        for (int i = 0; i < 3; i++)
        {
            Button current = keywordPanel.transform.GetChild(i).GetComponent<Button>();
        }
    }

    public void LockKeyword(Button button, string keyword)
    {
        Debug.Log("Keyword: " + keyword);
        if (!gameManager.ContainsKeyword(keyword))
        {
            gameManager.AddKeyword(keyword);
        }
        else
        {
            gameManager.RemoveKeyword(keyword);
        }
    }

    public void UpdateNotebook()
    {
        notebookText.text = "";
        foreach (Clue clue in gameManager.GetClues())
        {
            notebookText.text += clue.character;
            notebookText.text += "\n" + clue.name;
            notebookText.text += "\n" + clue.description;
        }
    }

    public void CycleNotebook()
    {

    }
}
