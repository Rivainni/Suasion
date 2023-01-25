using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Yarn.Unity;

public class StoryElement : MonoBehaviour
{
    [SerializeField] string title;
    [SerializeField] string type;
    [SerializeField] GameManager gameManager; // need to access the player state to determine whether to start dialogue or nah.
    [SerializeField] StoryManager storyManager;
    [SerializeField] GameObject reference = null; // the object that the player clicks on to start the dialogue
    [SerializeField] bool end = false; // does this element trigger the start of the next level?
    bool clicked = false; // has the player clicked on this element yet?
    bool inRange = false; // is the player in range of this element?
    void Start()
    {
        if (type == "Start")
        {
            TriggerDialogue();
            gameObject.SetActive(true);
        }
    }

    /* Called when you want to start dialogue */
    [YarnCommand("jumpstart")]
    public void TriggerDialogue()
    {
        storyManager.Interrupt();
        if (type == "Object" || type == "Start" || type == "Clue" || type == "Area")
        {
            storyManager.StartDialogue(title, 0);
        }
        else if (type == "Intro")
        {
            storyManager.StartDialogue(title, 1); // Accesses Dialogue Manager and Starts Dialogue
        }
        else if (type == "Persuade")
        {
            gameObject.SetActive(false);
            storyManager.StartDialogue(title, 2); // Accesses Dialogue Manager and Starts Dialogue
        }
        this.enabled = false;
    }

    void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && !clicked && inRange && type != "Area")
        {
            if (reference != null)
            {
                reference.SetActive(true);
            }

            if (gameObject.GetComponent<Door>() != null)
            {
                gameObject.GetComponent<Door>().SetLinks(false);
            }

            clicked = true;
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
                    if (gameManager.GetSuccess())
                    {
                        TriggerDialogue();
                    }
                    else
                    {
                        reference.GetComponent<StoryElement>().TriggerDialogue();
                    }
                }
                else if (reference.transform.name == "Door Filler")
                {
                    reference.SetActive(true);
                    TriggerDialogue();
                }
            }
            else
            {
                TriggerDialogue();
            }
            gameObject.SetActive(false);
        }
    }

    public bool GetClicked()
    {
        return clicked;
    }

    public void SetInRange(bool toggle)
    {
        inRange = toggle;
    }
}
