using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickRadius : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<StoryElement>() != null && !other.gameObject.GetComponent<StoryElement>().GetClicked() && other.gameObject.name != "Door")
        {
            //change the other gameobject color to yellow
            other.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            other.gameObject.GetComponent<StoryElement>().SetInRange(true);
        }
        else if (other.gameObject.GetComponent<Door>() != null && other.gameObject.GetComponent<StoryElement>() != null && !other.gameObject.GetComponent<StoryElement>().GetClicked())
        {
            other.gameObject.GetComponent<StoryElement>().SetInRange(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<StoryElement>() != null && other.gameObject.name != "Door")
        {
            other.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            other.gameObject.GetComponent<StoryElement>().SetInRange(false);
        }
        else if (other.gameObject.GetComponent<Door>() != null && other.gameObject.GetComponent<StoryElement>() != null)
        {
            other.gameObject.GetComponent<StoryElement>().SetInRange(false);
        }
    }
}
