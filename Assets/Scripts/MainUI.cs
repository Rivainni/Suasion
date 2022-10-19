using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainUI : MonoBehaviour
{
    [SerializeField] GameObject answerPanelI;
    [SerializeField] GameObject answerPanelP;
    [SerializeField] GameObject dialogueP;
    [SerializeField] GameObject dialogueI;
    [SerializeField] GameObject dialoguePBox;
    [SerializeField] GameObject dialogueIBox;
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

        if (type == "Persuasion")
        {
            GameObject panel = Instantiate(answerPanelP, new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0), Quaternion.identity, dialogueP.transform);
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
                    actualButton.onValueChanged.AddListener(delegate { LockKeyword(actualButton, keyword); });
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
                    actualButton.onValueChanged.AddListener(delegate { LockKeyword(actualButton, keyword); });
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
                    actualButton.onValueChanged.AddListener(delegate { LockKeyword(actualButton, keyword); });
                }
            }

            kaboom = panel;
        }
        else
        {
            GameObject panel = Instantiate(answerPanelI, new Vector3(Screen.width * 0.25f, Screen.height * 0.5f, 0), Quaternion.identity, dialogueI.transform);
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
                    actualButton.onValueChanged.AddListener(delegate { LockKeyword(actualButton, keyword); });
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
                    actualButton.onValueChanged.AddListener(delegate { LockKeyword(actualButton, keyword); });
                }
            }

            kaboom = panel;
        }

        proscriptionList.Clear();
    }

    public void LockKeyword(Toggle button, string keyword)
    {
        Debug.Log("Keyword: " + keyword);
        if (!gameManager.ContainsKeyword(keyword) && button.isOn)
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
