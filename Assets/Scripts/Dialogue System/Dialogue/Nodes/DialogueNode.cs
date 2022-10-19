using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DialogueNode : ScriptableObject
{
    [SerializeField] NarrationLine m_DialogueLine;

    public NarrationLine NarrationLine => m_DialogueLine;
}