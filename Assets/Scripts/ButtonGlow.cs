using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonGlow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image Rectangle;
    Button current;

    void Start()
    {
        current = gameObject.GetComponent<Button>();
        SetInteractable();
    }

    void Update()
    {
        SetInteractable();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (current.interactable)
        {
            Color currentColor = GetComponent<Image>().color;
            currentColor.a = 255;
            GetComponent<Image>().color = currentColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (current.interactable)
        {
            Color currentColor = GetComponent<Image>().color;
            currentColor.a = 0;
            GetComponent<Image>().color = currentColor;
        }
    }

    void SetInteractable()
    {
        if (current.interactable)
        {
            Rectangle.enabled = true;
        }
        else
        {
            Rectangle.enabled = false;
        }
    }
}