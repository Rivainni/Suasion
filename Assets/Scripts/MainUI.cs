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
    [SerializeField] GameObject confirmButton;
    [SerializeField] GameObject advanceInnerDialogueButton;
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
        ResetKeywords();

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

        // make sure the player can't advance until they click confirm
        advanceInnerDialogueButton.GetComponent<Button>().interactable = false;

        // for blocking keywords in tutorial
        if (type == "Persuasion" && gameManager.GetLevel() == 0)
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
        else if (type == "Intro" && gameManager.GetLevel() == 0)
        {
            Debug.Log("IT is now turn " + gameManager.GetTurn());
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

        foreach (Button button in keywordPanel.GetComponentsInChildren<Button>())
        {
            if (button.gameObject.GetComponentInChildren<TextMeshProUGUI>().text == "" && gameManager.CheckIntro())
            {
                break;
            }
            button.interactable = true;
            string category = button.gameObject.GetComponentInChildren<TextMeshProUGUI>().text;
            button.onClick.AddListener(delegate { SpawnKeywords(keywordSet, button.gameObject, category); });
        }
        confirmButton.GetComponent<Button>().interactable = true;
    }

    public void SpawnKeywords(KeywordSet keywordSet, GameObject originalButton, string category)
    {
        GameObject panel = Instantiate(answerPanel, new Vector2(originalButton.transform.position.x + 0.15f * Screen.width, originalButton.transform.position.y), Quaternion.identity, dialogueScreen.transform);
        Debug.Log(category);
        foreach (Button button in keywordPanel.GetComponentsInChildren<Button>())
        {
            button.interactable = false;
        }

        if (category == "Topic")
        {
            foreach (string keyword in keywordSet.Topic)
            {
                if (proscriptionList.Contains(keyword))
                {
                    continue;
                }

                GameObject button = Instantiate(buttonPrefab, panel.transform.position, Quaternion.identity, panel.transform);
                Button actualButton = button.GetComponent<Button>();
                button.GetComponentInChildren<TextMeshProUGUI>().text = keyword;
                actualButton.onClick.AddListener(delegate { LockKeyword(panel, originalButton, keyword); });
            }
        }
        else if (category == "Tone")
        {
            foreach (string keyword in keywordSet.Tone)
            {
                if (proscriptionList.Contains(keyword))
                {
                    continue;
                }

                GameObject button = Instantiate(buttonPrefab, panel.transform.position, Quaternion.identity, panel.transform);
                Button actualButton = button.GetComponent<Button>();
                button.GetComponentInChildren<TextMeshProUGUI>().text = keyword;
                actualButton.onClick.AddListener(delegate { LockKeyword(panel, originalButton, keyword); });
            }
        }
        else if (category == "Honesty")
        {
            foreach (string keyword in keywordSet.Honesty)
            {
                if (proscriptionList.Contains(keyword))
                {
                    continue;
                }

                GameObject button = Instantiate(buttonPrefab, panel.transform.position, Quaternion.identity, panel.transform);
                Button actualButton = button.GetComponent<Button>();
                button.GetComponentInChildren<TextMeshProUGUI>().text = keyword;
                actualButton.onClick.AddListener(delegate { LockKeyword(panel, originalButton, keyword); });
            }
        }
    }

    public void LockKeyword(GameObject panel, GameObject source, string keyword)
    {
        if (!gameManager.ContainsKeyword(keyword))
        {
            gameManager.AddKeyword(keyword);
        }
        else
        {
            gameManager.RemoveKeyword(keyword);
        }

        source.GetComponentInChildren<TextMeshProUGUI>().text = keyword;
        EnableCategories();

        Destroy(panel);
    }

    public void ResetKeywords()
    {
        keywordPanel.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = "Topic";
        keywordPanel.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "Tone";
        keywordPanel.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = "";

        if (gameManager.CheckPersuade())
        {
            keywordPanel.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = "Honesty";
        }
        foreach (Button button in keywordPanel.GetComponentsInChildren<Button>())
        {
            button.interactable = false;
            button.onClick.RemoveAllListeners();
        }

        confirmButton.GetComponent<Button>().interactable = false;

        proscriptionList.Clear();
    }

    void EnableCategories()
    {
        foreach (Button button in keywordPanel.GetComponentsInChildren<Button>())
        {
            if (gameManager.CheckIntro() && button.GetComponentInChildren<TextMeshProUGUI>().text == "")
            {
                break;
            }
            button.interactable = true;
        }
    }

    public void EnableAdvance()
    {
        advanceInnerDialogueButton.GetComponent<Button>().interactable = true;
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
