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
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] HealthBar persuasionBar;
    [SerializeField] HealthBar empathyBar;
    [SerializeField] GameManager gameManager;

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
        if (type == "Persuasion")
        {
            GameObject panel = Instantiate(answerPanelP, new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0), Quaternion.identity, dialogueP.transform);
            foreach (string keyword in keywordSet.Topic)
            {
                GameObject button = Instantiate(buttonPrefab, panel.transform.GetChild(0).GetChild(0).transform.position, Quaternion.identity, panel.transform.GetChild(0).GetChild(0).transform);
                Button actualButton = button.GetComponent<Button>();
                actualButton.GetComponentInChildren<TextMeshProUGUI>().text = keyword;
                actualButton.onClick.AddListener(() => LockKeyword(button, keyword));
            }

            foreach (string keyword in keywordSet.Tone)
            {
                GameObject button = Instantiate(buttonPrefab, panel.transform.GetChild(0).GetChild(1).transform.position, Quaternion.identity, panel.transform.GetChild(0).GetChild(1).transform);
                Button actualButton = button.GetComponent<Button>();
                actualButton.GetComponentInChildren<TextMeshProUGUI>().text = keyword;
                actualButton.onClick.AddListener(() => LockKeyword(button, keyword));
            }

            foreach (string keyword in keywordSet.Honesty)
            {
                GameObject button = Instantiate(buttonPrefab, panel.transform.GetChild(0).GetChild(2).transform.position, Quaternion.identity, panel.transform.GetChild(0).GetChild(2).transform);
                Button actualButton = button.GetComponent<Button>();
                actualButton.GetComponentInChildren<TextMeshProUGUI>().text = keyword;
                actualButton.onClick.AddListener(() => LockKeyword(button, keyword));
            }

            kaboom = panel;
        }
        else
        {
            GameObject panel = Instantiate(answerPanelI, new Vector3(Screen.width * 0.25f, Screen.height * 0.5f, 0), Quaternion.identity, dialogueI.transform);
            foreach (string keyword in keywordSet.Topic)
            {
                GameObject button = Instantiate(buttonPrefab, panel.transform.GetChild(0).GetChild(0).transform.position, Quaternion.identity, panel.transform.GetChild(0).GetChild(0).transform);

                Button actualButton = button.GetComponent<Button>();
                actualButton.GetComponentInChildren<TextMeshProUGUI>().text = keyword;
                actualButton.onClick.AddListener(() => LockKeyword(button, keyword));
            }

            foreach (string keyword in keywordSet.Tone)
            {
                GameObject button = Instantiate(buttonPrefab, panel.transform.GetChild(0).GetChild(1).transform.position, Quaternion.identity, panel.transform.GetChild(0).GetChild(1).transform);

                Button actualButton = button.GetComponent<Button>();
                actualButton.GetComponentInChildren<TextMeshProUGUI>().text = keyword;
                actualButton.onClick.AddListener(() => LockKeyword(button, keyword));
            }

            kaboom = panel;
        }
    }

    public void LockKeyword(GameObject button, string keyword)
    {
        if (!button.GetComponent<Toggle>().isOn)
        {
            gameManager.AddKeyword(keyword);
        }
        else
        {
            gameManager.RemoveKeyword(keyword);
        }
    }
}
