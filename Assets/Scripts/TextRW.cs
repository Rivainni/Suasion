using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class TextRW
{
    public static TextAsset keywordsFile;

    public struct Keyword
    {
        public string Word { get; set; }
        public string Category { get; set; }
        public int Turn { get; set; }
        public string Character { get; set; }
        public bool Correct { get; set; }
        public bool Persuasion { get; set; }
    }

    static List<Keyword> keywords = new List<Keyword>();

    public static List<Keyword> GetKeywords()
    {
        return keywords;
    }

    public static List<Keyword> GetKeywords(string character, int turn, bool persuasion)
    {
        List<Keyword> retrieved = new List<Keyword>();

        foreach (Keyword keyword in keywords)
        {
            if (keyword.Character == character && keyword.Turn == turn && keyword.Persuasion == persuasion)
            {
                retrieved.Add(keyword);
            }
        }
        return retrieved;
    }

    public static void SetKeywords(TextAsset text)
    {
        keywordsFile = text;
        string txt = keywordsFile.text;
        string[] lines = txt.Split(System.Environment.NewLine.ToCharArray());

        int turn = 0;
        string character = "";
        string category = "";
        bool persuasion = false;
        foreach (string line in lines)
        {
            if (!string.IsNullOrEmpty(line))
            {
                if (line.StartsWith("["))
                {
                    if (line[1] == 'P')
                    {
                        persuasion = true;
                    }
                    turn = Int32.Parse(line[2].ToString());
                    character = line.Substring(4, line.Length - 5);
                }
                else if (line.StartsWith("!"))
                {
                    category = line.Substring(1, line.Length - 2);
                }
                else if (line.StartsWith("END"))
                {
                    turn = 0;
                    character = "";
                    category = "";
                    persuasion = false;
                }
                else
                {
                    if (category != "Correct")
                    {
                        Keyword curr = new Keyword();
                        curr.Word = line;
                        curr.Category = category;
                        curr.Turn = turn;
                        curr.Character = character;
                        curr.Persuasion = persuasion;
                        curr.Correct = false;
                        keywords.Add(curr);
                    }
                    else
                    {
                        for (int i = 0; i < keywords.Count; i++)
                        {
                            if (keywords[i].Word == line && keywords[i].Turn == turn && keywords[i].Character == character && keywords[i].Persuasion == persuasion)
                            {
                                Keyword curr = keywords[i];
                                curr.Correct = true;
                                keywords[i] = curr;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}