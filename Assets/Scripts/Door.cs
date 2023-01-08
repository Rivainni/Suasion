using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Door : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject[] links;
    [SerializeField] GameObject[] inverseLinks; // stuff that should appear once the door disappears
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetLinks(bool active)
    {
        foreach (GameObject link in links)
        {
            link.SetActive(active);
        }
        foreach (GameObject inverseLink in inverseLinks)
        {
            inverseLink.SetActive(!active);
        }
        gameObject.SetActive(active);
    }
}
