using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct PromoMaterial
{
    public string name;
    public float bonus;
    public int quantity;

    public PromoMaterial(string name, float bonus, int quantity)
    {
        this.name = name;
        this.bonus = bonus;
        this.quantity = quantity;
    }
}

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
            int index = i;
            items[i].GetComponent<Button>().onClick.AddListener(() => UseItem(index));
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
        Debug.Log("index " + index);
        Debug.Log(gameManager.GetItems().Count);
        if (items[index].GetComponent<Button>().interactable)
        {
            items[index].GetComponent<Button>().interactable = false;
            int quantity = gameManager.GetItems()[index].quantity;
            gameManager.GetItems()[index] = new PromoMaterial(gameManager.GetItems()[index].name, gameManager.GetItems()[index].bonus, quantity - 1);
            gameManager.UseItem(gameManager.GetItems()[index].bonus);
            items[index].GetComponentInChildren<TextMeshProUGUI>().text = "x" + gameManager.GetItems()[index].quantity;
        }
    }
}