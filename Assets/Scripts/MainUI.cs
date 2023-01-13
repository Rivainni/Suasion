using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainUI : MonoBehaviour
{
    [SerializeField] GameObject answerPanelI;
    [SerializeField] GameObject answerPanelP;
    [SerializeField] GameObject dialogueScreen;
    [SerializeField] GameObject keywordPanel;
    [SerializeField] GameObject advanceInnerDialogueButton;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] GameObject togglePrefab;
    [SerializeField] GameObject scorePanel;
    [SerializeField] HealthBar persuasionBar;
    [SerializeField] HealthBar empathyBar;
    [SerializeField] GameManager gameManager;
    [SerializeField] TextMeshProUGUI notebookText;
    List<string> proscriptionList = new List<string>();

    GameObject kaboom;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Main Game")
        {
            persuasionBar.SetHealth(30);
            empathyBar.SetHealth(30);
            ResetKeywords();

            foreach (Button button in keywordPanel.GetComponentsInChildren<Button>())
            {
                button.interactable = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Main Menu");
        }
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
                proscriptionList.Add("state");
                proscriptionList.Add("weather");
                proscriptionList.Add("cautious");
                proscriptionList.Add("assertive");
                proscriptionList.Add("exaggerate");
                proscriptionList.Add("downplay");
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
            button.interactable = true;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate { SpawnKeywords(keywordSet, button.gameObject, type); });
        }
    }

    public void SpawnKeywords(KeywordSet keywordSet, GameObject originalButton, string type)
    {
        GameObject panel;

        if (type == "Intro")
        {
            panel = Instantiate(answerPanelI, new Vector2(originalButton.transform.position.x + (0.25f * Screen.width), originalButton.transform.position.y), Quaternion.identity, dialogueScreen.transform);
        }
        else
        {
            panel = Instantiate(answerPanelP, new Vector2(originalButton.transform.position.x + (0.35f * Screen.width), originalButton.transform.position.y), Quaternion.identity, dialogueScreen.transform);
        }

        foreach (Button button in keywordPanel.GetComponentsInChildren<Button>())
        {
            button.interactable = false;
        }

        ToggleGroup topic = panel.transform.GetChild(0).GetChild(0).gameObject.AddComponent<ToggleGroup>();
        topic.allowSwitchOff = true;
        foreach (string keyword in keywordSet.Topic)
        {
            GameObject button = Instantiate(togglePrefab, panel.transform.GetChild(0).GetChild(0).transform.position, Quaternion.identity, panel.transform.GetChild(0).GetChild(0).transform);
            Toggle actualButton = button.GetComponent<Toggle>();
            actualButton.group = topic;
            actualButton.GetComponentInChildren<TextMeshProUGUI>().text = keyword;
            if (proscriptionList.Contains(keyword))
            {
                actualButton.interactable = false;
            }
            else
            {
                actualButton.onValueChanged.AddListener(delegate { LockKeyword(panel, button, keyword); });
            }
        }

        ToggleGroup tone = panel.transform.GetChild(0).GetChild(1).gameObject.AddComponent<ToggleGroup>();
        tone.allowSwitchOff = true;
        foreach (string keyword in keywordSet.Tone)
        {
            GameObject button = Instantiate(togglePrefab, panel.transform.GetChild(0).GetChild(1).transform.position, Quaternion.identity, panel.transform.GetChild(0).GetChild(1).transform);
            Toggle actualButton = button.GetComponent<Toggle>();
            actualButton.group = tone;
            actualButton.GetComponentInChildren<TextMeshProUGUI>().text = keyword;
            if (proscriptionList.Contains(keyword))
            {
                actualButton.interactable = false;
            }
            else
            {
                actualButton.onValueChanged.AddListener(delegate { LockKeyword(panel, button, keyword); });
            }
        }

        ToggleGroup honesty = panel.transform.GetChild(0).GetChild(2).gameObject.AddComponent<ToggleGroup>();
        honesty.allowSwitchOff = true;
        foreach (string keyword in keywordSet.Honesty)
        {
            GameObject button = Instantiate(togglePrefab, panel.transform.GetChild(0).GetChild(2).transform.position, Quaternion.identity, panel.transform.GetChild(0).GetChild(2).transform);
            Toggle actualButton = button.GetComponent<Toggle>();
            actualButton.group = honesty;
            actualButton.GetComponentInChildren<TextMeshProUGUI>().text = keyword;
            if (proscriptionList.Contains(keyword))
            {
                actualButton.interactable = false;
            }
            else
            {
                actualButton.onValueChanged.AddListener(delegate { LockKeyword(panel, button, keyword); });
            }
        }

        // confirm button

        Button[] confirmPanel;
        if (type == "Intro")
        {
            confirmPanel = panel.transform.GetChild(0).GetChild(2).GetComponentsInChildren<Button>();
        }
        else
        {
            confirmPanel = panel.transform.GetChild(0).GetChild(3).GetComponentsInChildren<Button>();
        }
        foreach (Button confirmButton in confirmPanel)
        {
            confirmButton.onClick.AddListener(delegate { ConfirmKeywords(panel); });
        }

        kaboom = panel;
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
    }

    void ConfirmKeywords(GameObject panel)
    {
        foreach (Button button in keywordPanel.GetComponentsInChildren<Button>())
        {
            button.interactable = true;
        }
        gameManager.ConfirmKeywords();
        Destroy(panel);
    }

    public void ResetKeywords()
    {
        foreach (Button button in keywordPanel.GetComponentsInChildren<Button>())
        {
            button.interactable = false;
            button.onClick.RemoveAllListeners();
        }

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

        foreach (Log log in gameManager.GetLogs())
        {
            notebookText.text += log.ToString();
        }

        if (gameManager.GetClues().Count > 1)
        {
            notebookText.text += "Clues";
        }
        foreach (Clue clue in gameManager.GetClues())
        {
            notebookText.text += clue.character;
            notebookText.text += "\n" + clue.name;
            notebookText.text += "\n" + clue.description;
        }
    }

    public void NextPage()
    {
        if (notebookText.textInfo.pageCount > 1 && notebookText.pageToDisplay < notebookText.textInfo.pageCount)
        {
            notebookText.pageToDisplay += 1;
        }
    }

    public void PreviousPage()
    {
        if (notebookText.textInfo.pageCount > 1 && notebookText.pageToDisplay > 1)
        {
            notebookText.pageToDisplay -= 1;
        }
    }

    public void DisplayCurrentScore()
    {
        scorePanel.SetActive(true);
        scorePanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Persuaded: " + gameManager.GetSuccess();
        scorePanel.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Empathy: " + gameManager.GetEmpathy() + "%";
        scorePanel.transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = "Persuasion: " + gameManager.GetPersuasion() + "%";
        scorePanel.transform.GetChild(1).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Score: " + gameManager.GetScore();
    }
}
