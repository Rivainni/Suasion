using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryElement : MonoBehaviour
{
    [SerializeField] TextAsset TextFileAsset; // your imported text file for your NPC
    [SerializeField] string title;
    [SerializeField] GameManager gameManager; // need to access the player state to determine whether to start dialogue or nah.
    Queue<string> dialogue = new Queue<string>(); // stores the dialogue (Great Performance!)

    void Start()
    {
        if (title == "Start")
        {
            TriggerDialogue();
            gameObject.SetActive(true);
        }
    }

    /* Called when you want to start dialogue */
    public void TriggerDialogue()
    {
        ReadTextFile(); // loads in the text file
        if (title == "Table" || title == "Bed" || title == "Start")
        {
            FindObjectOfType<StoryManager>().StartDialogue(dialogue, 0); // Accesses Dialogue Manager and Starts Dialogue
        }
        else if (title == "Intro")
        {
            FindObjectOfType<StoryManager>().StartDialogue(dialogue, 1); // Accesses Dialogue Manager and Starts Dialogue
        }
        else if (title == "Persuade")
        {
            FindObjectOfType<StoryManager>().StartDialogue(dialogue, 2); // Accesses Dialogue Manager and Starts Dialogue
        }
        gameObject.SetActive(false);
    }

    /* loads in your text file */
    void ReadTextFile()
    {
        string txt = TextFileAsset.text;

        string[] lines = txt.Split(System.Environment.NewLine.ToCharArray()); // Split dialogue lines by newline

        foreach (string line in lines) // for every line of dialogue
        {
            if (!string.IsNullOrEmpty(line))// ignore empty lines of dialogue
            {
                if (line.StartsWith("[")) // e.g [NAME=Michael] Hello, my name is Michael
                {
                    string special = line.Substring(0, line.IndexOf(']') + 1); // special = [NAME=Michael]
                    string curr = line.Substring(line.IndexOf(']') + 1); // curr = Hello, ...
                    dialogue.Enqueue(special); // adds to the dialogue to be printed
                    dialogue.Enqueue(curr);
                }
                else
                {
                    dialogue.Enqueue(line); // adds to the dialogue to be printed
                }
            }
        }
        dialogue.Enqueue("EndQueue");
    }

    void OnMouseDown()
    {
        TriggerDialogue();
    }
}
