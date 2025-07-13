using System;

public enum TileType
{
    Normal = 0,
    Blocked = 1,
    Bonus = 2
}

[Serializable]
public class LetterTileData
{
    public char Letter;
 
    public TileType TileType;

    public int GetScore()
    {
        string vowels = "aeiou";
        return vowels.Contains(char.ToLower(Letter)) ? 1 : 2;
    }
}