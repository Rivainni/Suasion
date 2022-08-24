using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoryManager : MonoBehaviour
{
    [SerializeField] GameObject outerDialogue;
    [SerializeField] GameObject introDialogue;
    [SerializeField] GameObject persuasionDialogue;
    [SerializeField] MainUI mainUI;
    [SerializeField] GameManager gameManager;
    [SerializeField] TextMeshProUGUI[] storyText;
    Queue<string> inputStream = new Queue<string>();
    bool pause = false;
    bool primarySpeaker = false;
    int currentTextIndex = 0;

    bool actionTaken = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartDialogue(Queue<string> dialogue, int dialogueType)
    {
        // open the dialogue box
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
        inputStream = dialogue; // store the dialogue from dialogue trigger
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
            }
            PrintDialogue();
        }
    }

    void PrintDialogue()
    {
        if (inputStream.Count == 0 || inputStream.Peek().Contains("EndQueue")) // special phrase to stop dialogue
        {
            inputStream.Dequeue(); // Clear Queue
            EndDialogue();
        }
        else if (inputStream.Peek().Contains("[NAME="))
        {
            string name = inputStream.Peek();
            name = inputStream.Dequeue().Substring(name.IndexOf('=') + 1, name.IndexOf(']') - (name.IndexOf('=') + 1));
            // characterName.text = name;
            primarySpeaker = true;
            PrintDialogue();
        }
        else if (inputStream.Peek().Contains("[ACTION="))
        {
            string action = inputStream.Peek();
            action = inputStream.Dequeue().Substring(action.IndexOf('=') + 1, action.IndexOf(']') - (action.IndexOf('=') + 1));
            actionTaken = true;

            if (action == "Choose")
            {
                if (gameManager.CheckIntro())
                {
                    mainUI.DisplayKeywords("Tutorial", gameManager.GetTurn(), false);
                }
                else if (gameManager.CheckPersuade())
                {

                    mainUI.DisplayKeywords("Tutorial", gameManager.GetTurn(), true);
                }
            }

            PrintDialogue();
        }
        else if (inputStream.Peek().Contains("{0}"))
        {
            string current = inputStream.Dequeue();

            int stop = 0;
            for (int i = 0; i < current.Length; i++)
            {
                if (current[i] == ':')
                {
                    stop = i;
                    break;
                }
            }
            string expression = current.Substring(0, stop);
            // storyText.text = string.Format(current.Substring(stop + 1), playerState.GetName());
        }
        else if (inputStream.Peek().Contains(":"))
        {
            string current = inputStream.Dequeue();

            int stop = 0;
            for (int i = 0; i < current.Length; i++)
            {
                if (current[i] == ':')
                {
                    stop = i;
                    break;
                }
            }
            string expression = current.Substring(0, stop);
            storyText[currentTextIndex].text = current.Substring(stop + 1);
        }
        else
        {
            storyText[currentTextIndex].text = inputStream.Dequeue();
        }
    }

    public void EndDialogue()
    {

        storyText[currentTextIndex].text = "";
        // characterName.text = "";
        inputStream.Clear();
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