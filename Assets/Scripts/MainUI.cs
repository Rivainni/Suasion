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

        foreach (Button button in keywordPanel.GetComponentsInChildren<Button>())
        {
            button.interactable = false;
        }
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
            button.onClick.AddListener(delegate { SpawnKeywords(keywordSet, button, buttonIndex); });
            buttonIndex++;
        }

        proscriptionList.Clear();
    }

    public void SpawnKeywords(KeywordSet keywordSet, Button originalButton, int category)
    {
        GameObject panel = Instantiate(answerPanel, new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0), Quaternion.identity, originalButton.transform);
        if (category == 0)
        {
            foreach (string keyword in keywordSet.Topic)
            {
                GameObject button = Instantiate(togglePrefab, panel.transform.GetChild(0).GetChild(0).transform.position, Quaternion.identity, panel.transform.GetChild(0).GetChild(0).transform);
                Button actualButton = button.GetComponent<Button>();
                button.GetComponentInChildren<TextMeshProUGUI>().text = keyword;

                if (proscriptionList.Contains(keyword))
                {
                    actualButton.interactable = false;
                }
                else
                {
                    actualButton.onClick.AddListener(delegate { LockKeyword(actualButton, keyword); });
                }
            }
        }
        else if (category == 1)
        {
            foreach (string keyword in keywordSet.Tone)
            {
                GameObject button = Instantiate(togglePrefab, panel.transform.GetChild(1).transform.position, Quaternion.identity, panel.transform.GetChild(0).GetChild(0).transform);
                Button actualButton = button.GetComponent<Button>();
                button.GetComponentInChildren<TextMeshProUGUI>().text = keyword;

                if (proscriptionList.Contains(keyword))
                {
                    actualButton.interactable = false;
                }
                else
                {
                    actualButton.onClick.AddListener(delegate { LockKeyword(actualButton, keyword); });
                }
            }
        }
        else if (category == 2)
        {
            foreach (string keyword in keywordSet.Honesty)
            {
                GameObject button = Instantiate(togglePrefab, panel.transform.GetChild(2).transform.position, Quaternion.identity, panel.transform.GetChild(0).GetChild(0).transform);
                Button actualButton = button.GetComponent<Button>();
                button.GetComponentInChildren<TextMeshProUGUI>().text = keyword;

                if (proscriptionList.Contains(keyword))
                {
                    actualButton.interactable = false;
                }
                else
                {
                    actualButton.onClick.AddListener(delegate { LockKeyword(actualButton, keyword); });
                }
            }
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
