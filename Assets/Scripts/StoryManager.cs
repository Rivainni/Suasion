using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Yarn.Unity;

[System.Serializable]
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
[System.Serializable]
public struct Log
{
    public List<string> keywords;
    public string character;
    public string state;
    public float result;
    public float maxResult;
    public string honesty;
    public int honestyMult;
    public string response;
    public string comment;

    public Log(List<string> keywords, string character, string state, float result, float maxResult, string honesty, int honestyMult, string response, string comment)
    {
        this.keywords = new List<string>();
        //this.keywords = keywords;
        foreach (string keyword in keywords)
        {
            this.keywords.Add(keyword);
        }
        this.state = state;
        this.character = character;
        this.result = result;
        this.maxResult = maxResult;
        this.honesty = honesty;
        this.honestyMult = honestyMult;
        this.response = response;
        this.comment = comment;
    }

    public override string ToString()
    {
        string curr = "During: ";
        curr += "<b>" + character + "'s</b>" + state + "\n";
        curr += "Chosen keywords: ";
        curr += keywords[0] + ", " + keywords[1] + ", ";
        if (state == "Introduction")
        {
            curr += "\n";
            curr += "Result: ";
            curr += result + " out of " + maxResult + "\n";
        }
        else
        {
            curr += keywords[2] + "\n";
            curr += "Result: ";
            curr += result + "out of " + maxResult + "\n";
            curr += "You matched the mood " + comment + " with " + honesty + " honesty, multiplying your result by " + honestyMult + "%.\n";
        }

        curr += "Response: ";
        curr += response + "\n";
        curr += "Comment: ";
        curr += comment + "\n";
        curr += "\n";
        Debug.Log(curr);
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
    GameObject cutsceneDialogue;

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
    int previousType = 0;

    [SerializeField]
    StoryElement[] cutscenes;
    [SerializeField]
    TMP_Text[] dialogueTexts;

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

    void Start()
    {
        // if scene is main game play ambient
        if (SceneManager.GetActiveScene().name == "Main Game")
        {
            PlayAmbient();
        }
        PlayMusic("Map");
    }

    public void StartDialogue(string dialogue, int dialogueType, KeywordNode[] keywordSet = null, bool end = false)
    {
        if (!dialogueUp)
        {
            switch (dialogueType)
            {
                case 0:
                    outerDialogue.SetActive(true);
                    if (previousType != dialogueType)
                    {
                        dialogueRunner.SetDialogueViews(new DialogueViewBase[] { outerDialogue.GetComponent<LineView>() });
                        previousType = dialogueType;
                    }
                    break;
                case 1:
                    innerDialogue.SetActive(true);
                    // mainUI.Fade();
                    if (previousType != dialogueType)
                    {
                        dialogueRunner.SetDialogueViews(new DialogueViewBase[] { mcDialogue.GetComponent<DuoView>(), targetDialogue.GetComponent<DuoView>() });
                        currentKeywords = keywordSet;
                        previousType = dialogueType;
                    }
                    PauseAmbient();
                    PauseMusic("Map");
                    PlayMusic("Introduction");
                    gameManager.SetIntro(true);
                    break;
                case 2:
                    innerDialogue.SetActive(true);
                    // mainUI.Fade();
                    if (previousType != dialogueType)
                    {
                        dialogueRunner.SetDialogueViews(new DialogueViewBase[] { mcDialogue.GetComponent<DuoView>(), targetDialogue.GetComponent<DuoView>() });
                        currentKeywords = keywordSet;
                        previousType = dialogueType;
                    }
                    PauseAmbient();
                    PauseMusic("Map");
                    PlayMusic("Persuasion");
                    gameManager.SetPersuade(true);
                    break;
                case 3:
                    cutsceneDialogue.SetActive(true);
                    if (previousType != dialogueType)
                    {
                        dialogueRunner.SetDialogueViews(new DialogueViewBase[] { cutsceneDialogue.GetComponent<LineView>() });
                        previousType = dialogueType;
                    }

                    PauseAmbient();
                    break;
                default:
                    break;
            }

            if (!CheckCutscene())
            {
                gameManager.LockMovement(true);
                gameManager.PauseTimer(true);
                gameManager.HideTimer(true);
                dialogueUp = true;
            }
            dialogueRunner.StartDialogue(dialogue);
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

    // Audio Commands
    void PlayMusic(string music)
    {
        AkSoundEngine.PostEvent("Enter_" + music, gameObject);
    }

    void PauseMusic(string music)
    {
        AkSoundEngine.ExecuteActionOnEvent("Enter_" + music, AkActionOnEventType.AkActionOnEventType_Pause, gameObject, 0);
    }

    void ResumeMusic(string music)
    {
        AkSoundEngine.ExecuteActionOnEvent("Enter_" + music, AkActionOnEventType.AkActionOnEventType_Resume, gameObject, 1);
    }

    void PlayAmbient()
    {
        AkSoundEngine.PostEvent("Play_Ambient", gameObject);
    }

    void PauseAmbient()
    {
        AkSoundEngine.ExecuteActionOnEvent("Play_Ambient", AkActionOnEventType.AkActionOnEventType_Pause, gameObject, 0);
    }

    void ResumeAmbient()
    {
        AkSoundEngine.ExecuteActionOnEvent("Play_Ambient", AkActionOnEventType.AkActionOnEventType_Resume, gameObject, 1);
    }

    public void TriggerCutscene()
    {
        if (gameManager.GetCharacter() == "Friend")
        {
            cutscenes[0].TriggerDialogue();
        }
        else if (gameManager.GetCharacter() == "Doctor")
        {
            cutscenes[1].TriggerDialogue();
        }
        else if (gameManager.GetCharacter() == "Vice Mayor")
        {
            if (gameManager.GetElectionStatus())
            {
                cutscenes[5].TriggerDialogue();
            }
            else
            {
                cutscenes[6].TriggerDialogue();
            }
        }
        else if (gameManager.CheckAllCharactersDone())
        {
            cutscenes[4].TriggerDialogue();
        }
        else if (gameManager.GetCharacter() == "Farmer")
        {
            cutscenes[2].TriggerDialogue();
        }
        else if (gameManager.GetCharacter() == "Baker")
        {
            cutscenes[3].TriggerDialogue();
        }
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
            cutsceneDialogue.SetActive(false);
            gameManager.SetIntro(false);
            gameManager.SetPersuade(false);
            gameManager.LockMovement(false);
            gameManager.PauseTimer(false);
            gameManager.HideTimer(false);
            gameManager.Reset();
            mainUI.HideItems();
            mainUI.ResetBars();
            mainUI.BlockClicks();
            ResumeMusic("Map");
            PauseMusic("Introduction");
            PauseMusic("Persuasion");
            ResumeAmbient();
        }
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

    [YarnCommand("callitems")]
    public void CallItems()
    {
        mainUI.DisplayItems();
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

    [YarnCommand("modmultiplier")]
    public void ModMultiplier()
    {
        gameManager.ModMultiplier();
    }

    [YarnCommand("checkpersuasion")]
    public void CheckPersuasion()
    {
        variableStorage = GameObject.FindObjectOfType<InMemoryVariableStorage>();
        variableStorage.SetValue("$persuasion", gameManager.GetPersuasion());
    }

    [YarnCommand("checkempathy")]
    public void CheckEmpathy()
    {
        variableStorage = GameObject.FindObjectOfType<InMemoryVariableStorage>();
        variableStorage.SetValue("$empathy", gameManager.GetEmpathy());
    }

    [YarnCommand("setcharacter")]
    public void SetCharacter(string character)
    {
        gameManager.SetCharacter(character);
    }

    [YarnCommand("unlockitems")]
    public void UnlockItems()
    {
        gameManager.GenerateItems();
        mainUI.DisplayItems(true);
    }

    [YarnCommand("checkitemunlock")]
    public void CheckItemUnlock()
    {
        variableStorage = GameObject.FindObjectOfType<InMemoryVariableStorage>();
        variableStorage.SetValue("$itemon", gameManager.CheckItemUnlock());
    }

    [YarnCommand("checkpersuaded")]
    public void CheckPersuaded(string character)
    {
        variableStorage = GameObject.FindObjectOfType<InMemoryVariableStorage>();
        variableStorage.SetValue("$req" + character, gameManager.CheckPersuaded(character));
    }

    [YarnCommand("transition")]
    public void Transition()
    {
        mainUI.Fade();
    }

    [YarnCommand("teleport")]
    public void Teleport(string location)
    {
        gameManager.Teleport(location);
    }

    [YarnCommand("quit")]
    public void Quit()
    {
        Application.Quit();
    }
}