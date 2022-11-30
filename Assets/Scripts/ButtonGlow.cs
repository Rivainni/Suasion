using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonGlow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    bool mouse_over = false;
    Button current;

    void Start()
    {
        current = gameObject.GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (current.interactable)
        {
            mouse_over = true;
            Color currentColor = GetComponent<Image>().color;
            currentColor.a = 255;
            GetComponent<Image>().color = currentColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (current.interactable)
        {
            mouse_over = false;
            Color currentColor = GetComponent<Image>().color;
            currentColor.a = 0;
            GetComponent<Image>().color = currentColor;
        }
    }
}
