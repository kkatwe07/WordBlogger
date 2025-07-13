using System.Collections.Generic;
using UnityEngine;

public class EndlessMode : GameModeBase
{
    [SerializeField] private Vector2Int gridSize;

    public override void Init()
    {
        GameManager.Instance.Grid.SetGridSize(gridSize);
        GameManager.Instance.Grid.InitGrid();
    }

    public override void OnWordSubmitted(string word, List<LetterTile> tilesUsed)
    {
        if (!GameManager.Instance.WordValidator.IsValidWord(word)) return;

        int wordScore = 0;
        foreach (var tile in tilesUsed)
        {
            wordScore += tile.GetScore();

            GameManager.Instance.Grid.ClearTileAt(tile.X, tile.Y);
            Destroy(tile.gameObject);
        }

        GameManager.Instance.UiManager.UpdateScore(wordScore);
        GameManager.Instance.Grid.FillEmptyTiles();
    }
}