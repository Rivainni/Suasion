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
                GameObject button = Instantiate(togglePrefab, panel.transform.GetChild(0).GetChild(0).transform.position, Quaternion.identity, panel.transform.GetChild(0).GetChild(0).transform);
                Toggle actualButton = button.GetComponent<Toggle>();
                actualButton.GetComponentInChildren<TextMeshProUGUI>().text = keyword;
                actualButton.onValueChanged.AddListener(delegate { LockKeyword(button, keyword); });
            }

            foreach (string keyword in keywordSet.Tone)
            {
                GameObject button = Instantiate(togglePrefab, panel.transform.GetChild(0).GetChild(1).transform.position, Quaternion.identity, panel.transform.GetChild(0).GetChild(1).transform);
                Toggle actualButton = button.GetComponent<Toggle>();
                actualButton.GetComponentInChildren<TextMeshProUGUI>().text = keyword;
                actualButton.onValueChanged.AddListener(delegate { LockKeyword(button, keyword); });
            }

            foreach (string keyword in keywordSet.Honesty)
            {
                GameObject button = Instantiate(togglePrefab, panel.transform.GetChild(0).GetChild(2).transform.position, Quaternion.identity, panel.transform.GetChild(0).GetChild(2).transform);
                Toggle actualButton = button.GetComponent<Toggle>();
                actualButton.GetComponentInChildren<TextMeshProUGUI>().text = keyword;
                actualButton.onValueChanged.AddListener(delegate { LockKeyword(button, keyword); });
            }

            kaboom = panel;
        }
        else
        {
            GameObject panel = Instantiate(answerPanelI, new Vector3(Screen.width * 0.25f, Screen.height * 0.5f, 0), Quaternion.identity, dialogueI.transform);
            foreach (string keyword in keywordSet.Topic)
            {
                GameObject button = Instantiate(togglePrefab, panel.transform.GetChild(0).GetChild(0).transform.position, Quaternion.identity, panel.transform.GetChild(0).GetChild(0).transform);

                Toggle actualButton = button.GetComponent<Toggle>();
                actualButton.GetComponentInChildren<TextMeshProUGUI>().text = keyword;
                actualButton.onValueChanged.AddListener(delegate {LockKeyword(button, keyword);});
            }

            foreach (string keyword in keywordSet.Tone)
            {
                GameObject button = Instantiate(togglePrefab, panel.transform.GetChild(0).GetChild(1).transform.position, Quaternion.identity, panel.transform.GetChild(0).GetChild(1).transform);

                Toggle actualButton = button.GetComponent<Toggle>();
                actualButton.GetComponentInChildren<TextMeshProUGUI>().text = keyword;
                actualButton.onValueChanged.AddListener(delegate { LockKeyword(button, keyword); });
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

    public void ChangeColor(bool mc, bool persuade)
    {
        // change color of dialogueIBox to #DDB3B3
        if (mc)
        {
            if (persuade)
            {
                dialoguePBox.GetComponent<Image>().color = new Color32(221, 179, 179, 120);
            }
            else
            {
                dialogueIBox.GetComponent<Image>().color = new Color32(221, 179, 179, 120);
            }
        }
        else
        {
            // change color of DialogueIBox to #B3B7DD
            if (persuade)
            {
                dialoguePBox.GetComponent<Image>().color = new Color32(179, 183, 221, 120);
            }
            else
            {
                dialogueIBox.GetComponent<Image>().color = new Color32(179, 183, 221, 120);
            }
        }
        //B3B7DD
    }
}
