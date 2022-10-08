using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Narration/Line")]
public class NarrationLine : ScriptableObject
{
    [SerializeField] string m_characterName;
    
    public string characterName => m_characterName;

}