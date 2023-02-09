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
    GameObject innerDialogue;
    [SerializeField]
    GameObject mcDialogue;
    [SerializeField]
    GameObject targetDialogue;

    [SerializeField]
    MainUI mainUI;

    [SerializeField]
    GameManager gameManager;

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
    [SerializeField]
    GameObject friend;
    DialogueRunner dialogueRunner;
    InMemoryVariableStorage variableStorage;
    bool pause = false;
    bool dialogueUp = false;
    bool actionTaken = false;

    StoryElement chain = null;
    KeywordNode[] currentKeywords;
    bool textBoxMode = true;

    // Start is called before the first frame update
    void Awake()
    {
        dialogueRunner = GetComponent<DialogueRunner>();
        variableStorage = GameObject.FindObjectOfType<InMemoryVariableStorage>();
        if (variableStorage == null)
        {
            Debug.Log("I can't find the variable storage!");
        }
    }

    public void StartDialogue(string dialogue, int dialogueType, KeywordNode[] keywordSet = null, bool end = false)
    {
        if (!dialogueUp)
        {
            switch (dialogueType)
            {
                case 0:
                    outerDialogue.SetActive(true);
                    if (!textBoxMode)
                    {
                        dialogueRunner.SetDialogueViews(new DialogueViewBase[] { outerDialogue.GetComponent<LineView>() });
                        textBoxMode = true;
                    }
                    break;
                case 1:
                    innerDialogue.SetActive(true);
                    if (textBoxMode)
                    {
                        dialogueRunner.SetDialogueViews(new DialogueViewBase[] { mcDialogue.GetComponent<DuoView>(), targetDialogue.GetComponent<DuoView>() });
                        currentKeywords = keywordSet;
                        textBoxMode = false;
                        gameManager.GetAudioManager().Stop("Exploration");
                        gameManager.GetAudioManager().Play("Introduction", 0);
                    }
                    gameManager.SetIntro(true);
                    break;
                case 2:
                    innerDialogue.SetActive(true);
                    if (textBoxMode)
                    {
                        dialogueRunner.SetDialogueViews(new DialogueViewBase[] { mcDialogue.GetComponent<DuoView>(), targetDialogue.GetComponent<DuoView>() });
                        currentKeywords = keywordSet;
                        textBoxMode = false;
                        gameManager.GetAudioManager().Stop("Exploration");
                        gameManager.GetAudioManager().Play("Persuasion", 0);
                    }
                    gameManager.SetPersuade(true);
                    break;
                default:
                    break;
            }

            dialogueRunner.StartDialogue(dialogue);
            if (!CheckCutscene())
            {
                gameManager.LockMovement(true);
                gameManager.PauseTimer(true);
                gameManager.HideTimer(true);
                dialogueUp = true;
            }
        }
    }

    public void Interrupt()
    {
        if (dialogueUp)
        {
            dialogueRunner.Stop();
            EndDialogue();
        }
    }

    public void ConfirmKeywords()
    {
        actionTaken = true;
        mainUI.EnableAdvance();
        mainUI.ResetKeywords();
    }

    public bool CheckCutscene()
    {
        if (SceneManager.GetActiveScene().name == "Intro Cutscene" || SceneManager.GetActiveScene().name == "End Cutscene")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void FocusTarget(bool target)
    {
        if (target)
        {
            targetDialogue.GetComponent<DuoView>().SetCurrent(true);
            mcDialogue.GetComponent<DuoView>().SetCurrent(false);
        }
        else
        {
            targetDialogue.GetComponent<DuoView>().SetCurrent(false);
            mcDialogue.GetComponent<DuoView>().SetCurrent(true);
        }
    }

    public void SetChoice(string choice)
    {
        variableStorage = GameObject.FindObjectOfType<InMemoryVariableStorage>();
        variableStorage.SetValue("$choice", choice);
    }

    public void SetResponse(string response)
    {
        variableStorage = GameObject.FindObjectOfType<InMemoryVariableStorage>();
        variableStorage.SetValue("$response", response);
    }

    // Yarn commands below this point

    [YarnCommand("enddialogue")]
    public void EndDialogue()
    {
        if (dialogueUp && !CheckCutscene())
        {
            dialogueUp = false;
            outerDialogue.SetActive(false);
            innerDialogue.SetActive(false);
            gameManager.SetIntro(false);
            gameManager.SetPersuade(false);
            gameManager.LockMovement(false);
            gameManager.PauseTimer(false);
            gameManager.HideTimer(false);
            gameManager.Reset();
            gameManager.GetAudioManager().Stop("Introduction");
            gameManager.GetAudioManager().Stop("Persuasion");
            gameManager.GetAudioManager().Play("Exploration", 0);
        }
    }

    [YarnCommand("changescene")]
    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    [YarnCommand("callkeywords")]
    public void CallKeywords()
    {
        string type = gameManager.CheckPersuade() ? "Persuasion" : "Intro";
        mainUI.DisplayKeywords(currentKeywords[gameManager.GetTurn() - 1].Keywords, type);

        foreach (Combination combination in currentKeywords[gameManager.GetTurn() - 1].Combinations)
        {
            gameManager.AddCombination(combination);
        }
    }

    [YarnCommand("addclue")]
    public void AddClue(string name, string description, string character)
    {
        gameManager.AddClue(name, description, character);
    }

    [YarnCommand("rollsuccess")]
    public void RollSuccess()
    {
        variableStorage = GameObject.FindObjectOfType<InMemoryVariableStorage>();
        variableStorage.SetValue("$success", gameManager.RollSuccess());
    }

    [YarnCommand("endlevel")]
    public void EndLevel()
    {
        gameManager.AddLevel();
        gameManager.LockMovement(true);

    }

    [YarnCommand("randomisemood")]
    public void RandomiseMood()
    {
        gameManager.RandomiseMood();
        variableStorage = GameObject.FindObjectOfType<InMemoryVariableStorage>();
        variableStorage.SetValue("$mood", gameManager.GetMood());
    }

    [YarnCommand("togglefriend")]
    public void ToggleFriend()
    {
        friend.SetActive(!friend.activeSelf);
    }

    [YarnCommand("setmultiplier")]
    public void SetMultiplier(int multiplier)
    {
        gameManager.SetMultiplier(multiplier);
    }

    [YarnCommand("checkpersuasion")]
    public void CheckPersuasion()
    {
        variableStorage = GameObject.FindObjectOfType<InMemoryVariableStorage>();
        variableStorage.SetValue("$persuasion", gameManager.GetPersuasion());
    }
}