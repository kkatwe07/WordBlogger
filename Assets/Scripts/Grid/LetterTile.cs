using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LetterTile : MonoBehaviour
{
    public TMP_Text letterText;
    public Image background;

    public int X { get; set; }
    public int Y { get; set; }
    public char Letter { get; private set; }
    public TileType TileType { get; private set; }

    public void Init(char letter, TileType type, int x, int y)
    {
        Letter = letter;
        TileType = type;
        X = x;
        Y = y;

        letterText.text = Letter.ToString();

        switch (TileType)
        {
            case TileType.Normal:
                background.color = Color.green;
                break;
            case TileType.Bonus:
                background.color = Color.cyan;
                break;
            case TileType.Blocked:
                background.color = Color.gray;
                break;
            default:
                TileType = TileType.Normal;
                background.color = Color.green;
                break;
        }
    }

    public void ResetColor()
    {
        switch (TileType)
        {
            case TileType.Normal:
                background.color = Color.green;
                break;
            case TileType.Bonus:
                background.color = Color.cyan;
                break;
            case TileType.Blocked:
                background.color = Color.gray;
                break;
            default:
                TileType = TileType.Normal;
                background.color = Color.green;
                break;            
        }
    }

    public int GetScore()
    {
        char lower = char.ToLower(Letter);
        return "aeiou".Contains(lower) ? 1 : 2;
    }

    public void SetToNormal()
    {
        TileType = TileType.Normal;
        background.color = Color.green;
    }
}