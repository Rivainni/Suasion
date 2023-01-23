using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Yarn.Unity;

public struct Clue
{
    public string name;
    public string description;
    public string character;

    public Clue(string name, string description, string character)
    {
        this.name = name;
        this.description = description;
        this.character = character;
    }
}

// chosen keywords, the amount of persuasion or empathy, their response, their mood
public struct Log
{
    public List<string> keywords;
    public string state;
    public float result;
    public string response;
    public string comment;

    public Log(List<string> keywords, string state, float result, string response, string comment)
    {
        this.keywords = new List<string>();
        //this.keywords = keywords;
        foreach (string keyword in keywords)
        {
            this.keywords.Add(keyword);
        }
        this.state = state;
        this.result = result;
        this.response = response;
        this.comment = comment;
    }

    public override string ToString()
    {
        string curr = "During: ";
        curr += state + "\n";
        curr += "Chosen keywords: ";
        curr += keywords[0] + ", " + keywords[1] + ", ";
        if (state == "Introduction")
        {
            curr += "\n";
        }
        else
        {
            curr += keywords[2] + "\n";
        }
        curr += "Result: ";
        curr += result + "\n";
        curr += "Response: ";
        curr += response + "\n";
        curr += "Comment: ";
        curr += comment + "\n";
        curr += "\n";
        return curr;
    }
}

public class StoryManager : MonoBehaviour
{
    [SerializeField]
    GameObject outerDialogue;

    [SerializeField]
    GameObject introDialogue;

    [SerializeField]
    GameObject persuasionDialogue;

    [SerializeField]
    MainUI mainUI;

    [SerializeField]
    GameManager gameManager;

    [SerializeField]
    TextMeshProUGUI[] storyText;
    [SerializeField]
    TextMeshProUGUI[] nameText;
    [SerializeField]
    TextMeshProUGUI notebookText;
    [SerializeField]
    Image targetImage;
    [SerializeField]
    Image mcImage;
    [SerializeField]
    Image targetGlow;
    [SerializeField]
    Image mcGlow;
    DialogueRunner dialogueRunner;
    bool pause = false;
    bool dialogueUp = false;
    bool actionTaken = false;

    StoryElement chain = null;
    bool end = false;

    // Start is called before the first frame update
    void Awake()
    {
        dialogueRunner = GetComponent<DialogueRunner>();
    }

    public void StartDialogue(string dialogue, int dialogueType)
    {
        if (!dialogueUp)
        {
            switch (dialogueType)
            {
                case 0:
                    outerDialogue.SetActive(true);
                    break;
                case 1:
                    introDialogue.SetActive(true);
                    gameManager.SetIntro(true);
                    break;
                case 2:
                    persuasionDialogue.SetActive(true);
                    gameManager.SetPersuade(true);
                    break;
                default:
                    break;
            }
            dialogueRunner.StartDialogue(dialogue);
            gameManager.LockMovement(true);
            gameManager.PauseTimer(true);
            gameManager.HideTimer(true);
        }
    }

    [YarnCommand("enddialogue")]
    public void EndDialogue()
    {
        if (dialogueUp)
        {
            dialogueUp = false;
            outerDialogue.SetActive(false);
            introDialogue.SetActive(false);
            persuasionDialogue.SetActive(false);
            gameManager.SetIntro(false);
            gameManager.SetPersuade(false);
            gameManager.LockMovement(false);
            gameManager.PauseTimer(false);
            gameManager.HideTimer(false);
        }
    }

    public void ConfirmKeywords()
    {
        actionTaken = true;
        mainUI.EnableAdvance();
        mainUI.ResetKeywords();
    }

    [YarnCommand("changescene")]
    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}