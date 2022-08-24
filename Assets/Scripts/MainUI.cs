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

    public void DisplayKeywords(string character, int turn, bool persuasion)
    {
        if (persuasion)
        {
            GameObject panel = Instantiate(answerPanelP, new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0), Quaternion.identity, dialogueP.transform);
            foreach (TextRW.Keyword keyword in TextRW.GetKeywords(character, turn, persuasion))
            {
                int categoryIndex = 0;
                if (keyword.Category == "Background")
                {
                    categoryIndex = 0;
                }
                else if (keyword.Category == "Tone")
                {
                    categoryIndex = 1;
                }
                else if (keyword.Category == "Strategy")
                {
                    categoryIndex = 2;
                }
                else if (keyword.Category == "Honesty")
                {
                    categoryIndex = 3;
                }
                GameObject button = Instantiate(buttonPrefab, panel.transform.GetChild(0).GetChild(categoryIndex).transform.position, Quaternion.identity, panel.transform.GetChild(0).GetChild(categoryIndex).transform);
                Button actualButton = button.GetComponent<Button>();
                actualButton.GetComponentInChildren<TextMeshProUGUI>().text = keyword.Word;
                actualButton.onClick.AddListener(() => LockKeyword(button, keyword));
            }

            kaboom = panel;
        }
        else
        {
            GameObject panel = Instantiate(answerPanelI, new Vector3(Screen.width * 0.25f, Screen.height * 0.5f, 0), Quaternion.identity, dialogueI.transform);
            foreach (TextRW.Keyword keyword in TextRW.GetKeywords(character, turn, persuasion))
            {
                int categoryIndex = 0;
                if (keyword.Category == "Background")
                {
                    categoryIndex = 0;
                }
                else if (keyword.Category == "Tone")
                {
                    categoryIndex = 1;
                }
                GameObject button = Instantiate(buttonPrefab, panel.transform.GetChild(0).GetChild(categoryIndex).transform.position, Quaternion.identity, panel.transform.GetChild(0).GetChild(categoryIndex).transform);

                Button actualButton = button.GetComponent<Button>();
                actualButton.GetComponentInChildren<TextMeshProUGUI>().text = keyword.Word;
                actualButton.onClick.AddListener(() => LockKeyword(button, keyword));
            }

            kaboom = panel;
        }
    }

    public void LockKeyword(GameObject toRemove, TextRW.Keyword keyword)
    {
        gameManager.AddKeyword(keyword);
        Destroy(toRemove);
    }
}
