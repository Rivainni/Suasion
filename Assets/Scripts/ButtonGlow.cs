using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonGlow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    bool mouse_over = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
        Color currentColor = GetComponent<Image>().color;
        currentColor.a = 255;
        GetComponent<Image>().color = currentColor;
        Debug.Log("change");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
        Color currentColor = GetComponent<Image>().color;
        currentColor.a = 0;
        GetComponent<Image>().color = currentColor;
        Debug.Log("change");
    }
}
