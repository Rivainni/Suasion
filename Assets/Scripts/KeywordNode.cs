using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Combination
{
    [SerializeField]
    private string m_Set;
    [SerializeField]
    private string m_Mood;
    [SerializeField]
    private int m_Points;
    [SerializeField]
    private string m_Choice;
    [SerializeField]
    private string m_Response;

    public string Set => m_Set;
    public string Mood => m_Mood;
    public int Points => m_Points;
    public string Choice => m_Choice;
    public string Response => m_Response;

    public int CheckKeywords(List<string> keywords, string currMood, bool persuasion)
    {
        if (persuasion && Set.Contains(keywords[0]) && Set.Contains(keywords[1]) && Set.Contains(keywords[2]) && Mood == currMood)
        {
            return Points;
        }
        else if (!persuasion && Set.Contains(keywords[0]) && Set.Contains(keywords[1]) && Mood == currMood)
        {
            return Points;
        }
        else
        {
            return 0;
        }
    }

    public int CheckHonesty()
    {
        if (Set.Contains("straight"))
        {
            return 0;
        }
        else if (Set.Contains("exaggerate"))
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }
}

[Serializable]
public class KeywordSet
{
    [SerializeField]
    private string[] m_Topic;
    [SerializeField]
    private string[] m_Tone;
    [SerializeField]
    private string[] m_Honesty;
    [SerializeField]
    private KeywordRestrictions[] m_Restrictions;

    public string[] Topic => m_Topic;
    public string[] Tone => m_Tone;
    public string[] Honesty => m_Honesty;
    public KeywordRestrictions[] Restrictions => m_Restrictions;
}

[Serializable]
public class KeywordRestrictions
{
    [SerializeField]
    private string m_Keyword;
    [SerializeField]
    private string m_Clue;

    public string Keyword => m_Keyword;
    public string Clue => m_Clue;
}

[CreateAssetMenu(menuName = "ScriptableObjects/Narration/Dialogue/Node/Choice")]
public class KeywordNode : ScriptableObject
{
    [SerializeField]
    private KeywordSet m_Keywords;
    public KeywordSet Keywords => m_Keywords;
    [SerializeField]
    private Combination[] m_Combinations;
    public Combination[] Combinations => m_Combinations;

    [SerializeField]
    private string m_Type;
    public string Type => m_Type;
}