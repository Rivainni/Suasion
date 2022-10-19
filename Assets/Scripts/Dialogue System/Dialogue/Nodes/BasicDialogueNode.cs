using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Narration/Dialogue/Node/Basic")]
public class BasicDialogueNode : DialogueNode
{
    [SerializeField]
    private DialogueNode m_NextNode;
    public DialogueNode NextNode => m_NextNode;
}