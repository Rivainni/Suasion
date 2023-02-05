using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Roof : MonoBehaviour
{
    Collider2D collider;
    Tilemap tilemap;
    Color curr = new Color(255, 255, 255, 255);
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
        tilemap = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        tilemap.color = curr;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<ClickRadius>() != null)
        {
            curr = new Color(1.0f, 1.0f, 1.0f, 1.0f - Mathf.Clamp(Mathf.Abs(Physics2D.Distance(other, collider).distance) / 2.0f, 0.0f, 1.0f));
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<ClickRadius>() != null)
        {
            curr = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
    }
}
