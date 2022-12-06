using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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

    DialogueNode current;
    bool pause = false;
    int currentTextIndex = 0;
    bool dialogueUp = false;
    bool actionTaken = false;

    // Start is called before the first frame update
    void Start() { }

    public void StartDialogue(Dialogue dialogue, int dialogueType)
    {
        // open the dialogue box
        if (!dialogueUp)
        {
            switch (dialogueType)
            {
                case 0:
                    outerDialogue.SetActive(true);
                    currentTextIndex = 0;
                    break;
                case 1:
                    introDialogue.SetActive(true);
                    currentTextIndex = 1;
                    gameManager.SetIntro(true);
                    break;
                case 2:
                    persuasionDialogue.SetActive(true);
                    currentTextIndex = 1;
                    gameManager.SetPersuade(true);
                    break;
                default:
                    break;
            }

            if (dialogueType == 1 || dialogueType == 2)
            {
                if (gameManager.GetLevel() > 0)
                {
                    gameManager.RandomiseMood();
                }

                switch (gameManager.GetLevel())
                {
                    case 0:
                        gameManager.SetMultiplier(3);
                        break;
                    case 1:
                        gameManager.SetMultiplier(3);
                        break;
                }
                mainUI.ResetKeywords();
            }
            current = dialogue.firstNode; // store the dialogue from dialogue trigger
            PrintDialogue(); // Prints out the first line of dialogue
            gameManager.LockMovement(true);
            dialogueUp = true;
        }
    }

    public void AdvanceDialogue() // call when a player presses a button in Dialogue Trigger
    {
        if (!pause)
        {
            if (actionTaken && (gameManager.CheckIntro() || gameManager.CheckPersuade()))
            {
                actionTaken = false;
                mainUI.EndTurn();
                current = gameManager.GetNext();
                gameManager.ResetNext();
            }
            PrintDialogue();
        }
    }

    void PrintDialogue()
    {
        if (current is BasicDialogueNode)
        {
            BasicDialogueNode basicNode = current as BasicDialogueNode;
            nameText[currentTextIndex].text = current.NarrationLine.Speaker.CharacterName;

            // if it's the main character talking
            if (current.NarrationLine.Speaker.CharacterName == "AMSEL" || currentTextIndex == 0)
            {
                storyText[currentTextIndex].text = basicNode.NarrationLine.Text;
            }
            else if (currentTextIndex > 0)
            {
                storyText[currentTextIndex + 1].text = basicNode.NarrationLine.Text;
            }
            current = basicNode.NextNode;
        }
        else if (current is KeywordNode)
        {
            KeywordNode keywordNode = current as KeywordNode;

            nameText[currentTextIndex].text = current.NarrationLine.Speaker.CharacterName;
            // if it's the main character talking
            if (current.NarrationLine.Speaker.CharacterName == "AMSEL" || currentTextIndex == 0)
            {
                storyText[currentTextIndex].text = keywordNode.NarrationLine.Text;
            }
            else if (currentTextIndex > 0)
            {
                storyText[currentTextIndex + 1].text = keywordNode.NarrationLine.Text;
            }

            mainUI.DisplayKeywords(keywordNode.Keywords, keywordNode.Type);
            foreach (Combination combination in keywordNode.Combinations)
            {
                gameManager.AddCombination(combination);
            }
        }
        else if (current == null)
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        storyText[currentTextIndex].text = "";
        current = null;
        outerDialogue.SetActive(false);
        introDialogue.SetActive(false);
        persuasionDialogue.SetActive(false);
        gameManager.SetIntro(false);
        gameManager.SetPersuade(false);
        gameManager.Reset();
        gameManager.LockMovement(false);
        dialogueUp = false;

        if (SceneManager.GetActiveScene().name == "Intro Cutscene")
        {
            SceneManager.LoadScene("Main Game");
        }
    }

    public void ConfirmKeywords()
    {
        actionTaken = true;
        mainUI.EnableAdvance();
        mainUI.ResetKeywords();
    }

    void SetName(InputField input, GameObject toRemove)
    {
        // playerState.SetName(input.text);
        Destroy(toRemove);
        pause = false;
        PrintDialogue();
    }
}