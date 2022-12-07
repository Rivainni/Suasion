using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class StoryElement : MonoBehaviour
{
    [SerializeField] Dialogue dialogue; // your imported text file for your NPC
    [SerializeField] string title;
    [SerializeField] string type;
    [SerializeField] GameManager gameManager; // need to access the player state to determine whether to start dialogue or nah.
    [SerializeField] GameObject reference = null; // the object that the player clicks on to start the dialogue
    [SerializeField] StoryElement chain = null; //do we trigger another dialogue after this one?
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
        if (type == "Object" || type == "Start" || type == "Clue")
        {
            if (chain != null)
            {
                FindObjectOfType<StoryManager>().StartDialogue(dialogue, 0, chain);
            }
            else
            {
                FindObjectOfType<StoryManager>().StartDialogue(dialogue, 0);
            }
            if (type == "Clue")
            {
                gameManager.AddClue(title, ((BasicDialogueNode)dialogue.firstNode).NextNode.NarrationLine.Text, dialogue.firstNode.NarrationLine.Speaker.CharacterName);
            }
        }
        else if (type == "Intro")
        {
            FindObjectOfType<StoryManager>().StartDialogue(dialogue, 1); // Accesses Dialogue Manager and Starts Dialogue
            gameObject.SetActive(false);
        }
        else if (type == "Persuade")
        {
            FindObjectOfType<StoryManager>().StartDialogue(dialogue, 2); // Accesses Dialogue Manager and Starts Dialogue
        }
        this.enabled = false;
    }

    void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (reference != null)
            {
                reference.SetActive(true);
            }
            TriggerDialogue();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            if (reference)
            {
                if (reference.transform.parent.name == "Tasks")
                {
                    if (gameManager.Success())
                    {
                        TriggerDialogue();
                    }
                    else
                    {
                        Application.Quit();
                    }
                }
            }
            else
            {
                TriggerDialogue();
            }
            gameObject.SetActive(false);
        }
    }
}
