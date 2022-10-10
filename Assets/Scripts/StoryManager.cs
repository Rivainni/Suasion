using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    DialogueNode current;
    bool pause = false;
    int currentTextIndex = 0;

    bool actionTaken = false;

    // Start is called before the first frame update
    void Start() { }

    public void StartDialogue(Dialogue dialogue, int dialogueType)
    {
        // open the dialogue box, this cah
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
                currentTextIndex = 2;
                gameManager.SetPersuade(true);
                break;
            default:
                break;
        }
        current = dialogue.firstNode; // store the dialogue from dialogue trigger
        PrintDialogue(); // Prints out the first line of dialogue
        gameManager.LockMovement(true);
    }

    public void AdvanceDialogue() // call when a player presses a button in Dialogue Trigger
    {
        if (!pause)
        {
            if (actionTaken && (gameManager.CheckIntro() || gameManager.CheckPersuade()))
            {
                mainUI.EndTurn();
                actionTaken = false;
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
            storyText[currentTextIndex].text = basicNode.NarrationLine.Text;
            current = basicNode.NextNode;
        }
        else if (current is KeywordNode)
        {
            KeywordNode keywordNode = current as KeywordNode;
            storyText[currentTextIndex].text = keywordNode.NarrationLine.Text;
            mainUI.DisplayKeywords(keywordNode.Keywords, keywordNode.Type);
            foreach (Combination combination in keywordNode.Combinations)
            {
                gameManager.AddCombination(combination);
            }
            actionTaken = true;
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
        gameManager.SetIntro(false);
        gameManager.Reset();
        gameManager.LockMovement(false);
    }

    void SetName(InputField input, GameObject toRemove)
    {
        // playerState.SetName(input.text);
        Destroy(toRemove);
        pause = false;
        PrintDialogue();
    }
}