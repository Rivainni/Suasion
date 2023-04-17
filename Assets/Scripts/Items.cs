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
        GenerateItems();
    }

    public void GenerateItems()
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i].GetComponent<Button>().onClick.AddListener(() => UseItem(i));
        }
    }

    public void DisplayItems(bool playerLock = false)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (gameManager.GetItems()[i].quantity > 0 && !playerLock)
            {
                items[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                items[i].GetComponent<Button>().interactable = false;
            }

            items[i].GetComponentInChildren<TextMeshProUGUI>().text = "x" + gameManager.GetItems()[i].quantity;
        }
    }

    public void UseItem(int index)
    {
        Debug.Log("index");
        if (items[index].GetComponent<Button>().interactable)
        {
            items[index].GetComponent<Button>().interactable = false;
            gameManager.GetItems()[index].UseItem();
            gameManager.UseItem(gameManager.GetItems()[index].bonus);
            items[index].GetComponentInChildren<TextMeshProUGUI>().text = "x" + gameManager.GetItems()[index].quantity;
        }
    }
}