using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Narration/Dialogue/Dialogue")]
public class Dialogue : ScriptableObject
{
    [SerializeField] DialogueNode m_firstNode;
    public DialogueNode firstNode => m_firstNode;
}