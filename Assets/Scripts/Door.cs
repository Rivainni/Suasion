using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Door : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject[] links;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (gameObject.activeSelf)
            {
                foreach (GameObject link in links)
                {
                    link.SetActive(false);
                }
                gameObject.SetActive(false);
            }
            else
            {
                foreach (GameObject link in links)
                {
                    link.SetActive(true);
                }
                gameObject.SetActive(true);
            }
        }
    }
}
