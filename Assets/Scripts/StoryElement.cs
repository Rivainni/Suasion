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
    [SerializeField] GameObject referenceDisable = null; // opposite of the above
    [SerializeField] KeywordNode[] keywords; // keywords assigned to this story element if applicable (turn 1 is index 0, etc)
    bool clicked = false; // has the player clicked on this element yet?
    bool inRange = false; // is the player in range of this element?
    void Start()
    {
        if (title == "IntroCutscene" || type == "Start")
        {
            TriggerDialogue();
            gameObject.SetActive(true);
        }
    }

    /* Called when you want to start dialogue */
    public void TriggerDialogue()
    {
        storyManager.Interrupt();
        if (type != "Intro" && type != "Persuade" && type != "Cutscene")
        {
            storyManager.StartDialogue(title, 0);
        }
        else if (type == "Intro")
        {
            storyManager.StartDialogue(title, 1, keywords); // Accesses Dialogue Manager and Starts Dialogue
        }
        else if (type == "Persuade")
        {
            storyManager.StartDialogue(title, 2, keywords); // Accesses Dialogue Manager and Starts Dialogue
        }
        else if (type == "Cutscene")
        {
            storyManager.StartDialogue(title, 3);
        }
        this.enabled = false;
    }

    void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && !clicked && inRange && type != "Area" && this.enabled)
        {
            if (reference != null)
            {
                reference.SetActive(true);
            }

            if (referenceDisable != null)
            {
                referenceDisable.SetActive(false);
            }

            if (gameObject.GetComponent<Door>() != null)
            {
                gameObject.GetComponent<Door>().SetLinks(false);
            }

            clicked = true;

            if (gameObject.GetComponent<SpriteRenderer>() != null)
            {
                if (gameObject.GetComponent<SpriteRenderer>().color == Color.yellow)
                {
                    gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                }
            }

            TriggerDialogue();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player" && type == "Area" && this.enabled)
        {
            if (reference)
            {
                if (reference.transform.name == "Door Filler")
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

    [YarnCommand("enabledialogue")]
    public void Enable(string title)
    {
        if (this.title == title)
        {
            this.enabled = true;
        }
    }

    [YarnCommand("jumpstart")]
    public void ManualStart(string title)
    {
        if (this.title == title)
        {
            TriggerDialogue();
        }
    }
}
