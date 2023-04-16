using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Items : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject[] items;
    // Start is called before the first frame update

    void Start()
    {
        DisplayItems();
    }

    public void DisplayItems(bool playerLock = false)
    {
        for (int i = 0; i < gameManager.GetItems().Count; i++)
        {
            if (gameManager.GetItems()[i].quantity > 0 || playerLock)
            {
                items[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                items[i].GetComponent<Button>().interactable = false;
            }

            items[i].GetComponentInChildren<TextMeshProUGUI>().text = "x" + gameManager.GetItems()[i].quantity;

            items[i].GetComponent<Button>().onClick.AddListener(() => UseItem(i));
        }
    }

    public void UseItem(int index)
    {
        if (items[index].activeSelf)
        {
            items[index].GetComponent<Button>().interactable = false;
            gameManager.GetItems()[index].UseItem();
        }
    }
}