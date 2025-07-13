using System.Collections.Generic;
using UnityEngine;

public class WordValidator : MonoBehaviour
{
    private HashSet<string> validWords = new HashSet<string>();

    public void LoadWords()
    {
        TextAsset wordList = Resources.Load<TextAsset>("wordList");
        string[] words = wordList.text.Split('\n');

        foreach (string word in words)
        {
            string trimmed = word.Trim().ToLower();
            if (!validWords.Contains(trimmed))
                validWords.Add(trimmed);
        }
    }

    public bool IsValidWord(string word)
    {
        return validWords.Contains(word.ToLower());
    }
}