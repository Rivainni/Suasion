using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Misc/PCharacter")]
public class PCharacter : ScriptableObject
{
    [SerializeField]
    private string m_charName;
    [SerializeField]
    private HonestyEffects[] m_HonestyEffects;
    [SerializeField]
    private int m_MaxPersuasionScore;
    [SerializeField]
    private int m_MaxEmpathyScore;
    [SerializeField]
    private string[] m_BonusCharacters;


    public string charName => m_charName;
    public HonestyEffects[] honestyEffects => m_HonestyEffects;
    public int maxPersuasionScore => m_MaxPersuasionScore;
    public int maxEmpathyScore => m_MaxEmpathyScore;
    public string[] bonusCharacters => m_BonusCharacters;

    public float GetMultiplier(string keyword, string mood)
    {
        foreach (HonestyEffects he in honestyEffects)
        {
            if (he.Mood == mood)
            {
                foreach (Honesty h in he.HWords)
                {
                    if (h.Keyword == keyword)
                    {
                        return h.Multiplier;
                    }
                }
            }
        }
        return 1;
    }
}


[Serializable]
public class HonestyEffects
{
    [SerializeField]
    private string m_Mood;
    [SerializeField]
    private Honesty[] m_HWords;

    public string Mood => m_Mood;
    public Honesty[] HWords => m_HWords;
}

[Serializable]
public class Honesty
{
    [SerializeField]
    private string m_Keyword;
    [SerializeField]
    private float m_Multiplier;

    public string Keyword => m_Keyword;
    public float Multiplier => m_Multiplier;
}