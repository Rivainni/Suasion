using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryElement : MonoBehaviour
{
    [SerializeField] Dialogue dialogue; // your imported text file for your NPC
    [SerializeField] string title;
    [SerializeField] string type;
    [SerializeField] GameManager gameManager; // need to access the player state to determine whether to start dialogue or nah.

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
        if (type == "Object" || type == "Start")
        {
            FindObjectOfType<StoryManager>().StartDialogue(dialogue, 0); // Accesses Dialogue Manager and Starts Dialogue
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
    }
    
    void OnMouseDown()
    {
        TriggerDialogue();
    }
}
