using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ScriptableObjects/Narration/Character")]
public class NarrationCharacter : ScriptableObject
{
    [SerializeField]
    private string m_CharacterName;
    [SerializeField]
    private Sprite m_CharacterIcon;
    [SerializeField]
    private Sprite m_CharacterCG;

    public string CharacterName => m_CharacterName;
    public Sprite CharacterIcon => m_CharacterIcon;
    public Sprite CharacterCG => m_CharacterCG;
}