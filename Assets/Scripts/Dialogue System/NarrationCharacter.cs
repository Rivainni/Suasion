using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ScriptableObjects/Narration/Character")]
public class NarrationCharacter : ScriptableObject
{
    [SerializeField]
    private string m_CharacterName;
    [SerializeField]
    private Sprite m_CharacterImage;

    public string CharacterName => m_CharacterName;
    public Sprite CharacterImage => m_CharacterImage;
}