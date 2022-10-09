using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class DialogueChoice
{
    [SerializeField]
    private string m_ChoicePreview;
    [SerializeField]
    private DialogueNode m_ChoiceNode;

    public string ChoicePreview => m_ChoicePreview;
    public DialogueNode ChoiceNode => m_ChoiceNode;
}


[CreateAssetMenu(menuName = "ScriptableObjects/Narration/Dialogue/Node/Choice")]
public class KeywordNode : DialogueNode
{
    [SerializeField]
    private DialogueChoice[] m_Choices;
    public DialogueChoice[] Choices => m_Choices;
}