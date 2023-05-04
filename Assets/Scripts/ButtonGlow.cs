using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonGlow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image Rectangle;
    Selectable current;

    void Start()
    {
        if (gameObject.GetComponent<Button>())
        {
            current = gameObject.GetComponent<Button>();
        }
        else
        {
            current = gameObject.GetComponent<Toggle>();
        }

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
            currentColor.a = 1.0f;

            if (gameObject.name == "Confirm")
            {
                Color confirmColor = Rectangle.color;
                confirmColor.a = 1.0f;
                Rectangle.color = confirmColor;
            }

            GetComponent<Image>().color = currentColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (current.interactable)
        {
            Color currentColor = GetComponent<Image>().color;
            currentColor.a = 0;

            if (gameObject.name == "Confirm")
            {
                Color confirmColor = Rectangle.color;
                confirmColor.a = 0.5f;
                Rectangle.color = confirmColor;
            }

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