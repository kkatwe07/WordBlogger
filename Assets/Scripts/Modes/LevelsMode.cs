using System.Collections.Generic;
using UnityEngine;

public class LevelsMode : GameModeBase
{
    [SerializeField] private TextAsset levelJson;

    private int wordsFormed = 0;
    private int totalScore = 0;
    private int bonusCollected = 0;
    private int requiredWords = 3;
    private int requiredScore = 30;
    private float timeLeft = 60f;
    private bool hasTimer = false;
    private bool useScoreObjective = false;

    private int currentLevelIndex = 0;
    private Vector2Int gridSize;
    private LevelDataCollection levelCollection;
    private List<GridTileInfo> LevelGridData;

    private void Update()
    {
        if (hasTimer)
        {
            timeLeft -= Time.deltaTime;
            GameManager.Instance.UiManager.UpdateTimer(timeLeft);

            if (timeLeft <= 0)
            {
                hasTimer = false;
                Debug.Log("TIME UP!");
                GameManager.Instance.UiManager.ShowLoseScreen();
            }
        }
    }

    public override void Init()
    {
        LoadLevelFromJSON();
        GameManager.Instance.Grid.SetGridSize(gridSize);
        GameManager.Instance.Grid.LoadStaticGrid(LevelGridData);
        GameManager.Instance.UiManager.ShowLevelObjectives(requiredWords, requiredScore, timeLeft);
    }

    public override void OnWordSubmitted(string word, List<LetterTile> tilesUsed)
    {
        if (!GameManager.Instance.WordValidator.IsValidWord(word)) return;

        int wordScore = 0;

        foreach (var tile in tilesUsed)
        {
            if (tile.TileType == TileType.Blocked)
            {
                Debug.Log("Blocked tile used - invalid move");
                return;
            }

            if (tile.TileType == TileType.Bonus)
            {
                bonusCollected++;
                Debug.Log("Bonus letter used!");
            }

            wordScore += tile.GetScore();
        }

        UnblockAdjacentTiles(tilesUsed);
        totalScore += wordScore;
        wordsFormed++;

        GameManager.Instance.UiManager.UpdateLevelProgress(wordsFormed, totalScore, requiredWords, bonusCollected);

        CheckWinCondition();
        foreach (var tile in tilesUsed)
        {
            tile.ResetColor();
        }
    }

    private void UnblockAdjacentTiles(List<LetterTile> usedTiles)
    {
        Vector2Int[] directions = {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
        };

        foreach (var tile in usedTiles)
        {
            foreach (var dir in directions)
            {
                LetterTile neighbor = GameManager.Instance.Grid.GetTileAt(tile.X + dir.x, tile.Y + dir.y);
                if (neighbor != null && neighbor.TileType == TileType.Blocked)
                {
                    neighbor.SetToNormal();
                }
            }
        }
    }

    private void CheckWinCondition()
    {
        if (requiredWords > 0 && wordsFormed >= requiredWords)
        {
            GameManager.Instance.UiManager.ShowWinScreen(currentLevelIndex);
        }
        else if (useScoreObjective && totalScore >= requiredScore)
        {
            GameManager.Instance.UiManager.ShowWinScreen(currentLevelIndex);
        }
    }

    private void LoadLevelFromJSON()
    {
        string raw = levelJson.text;
        levelCollection = JsonUtility.FromJson<LevelDataCollection>(raw);

        if (levelCollection.data == null || levelCollection.data.Count == 0)
        {
            Debug.LogError("No level data found!");
            return;
        }

        LoadLevel(currentLevelIndex);
    }

    private void LoadLevel(int index)
    {
        if (index >= levelCollection.data.Count)
        {
            Debug.Log("All levels completed!");
            GameManager.Instance.UiManager.ShowFinalCompletionScreen();
            return;
        }

        LevelData levelData = levelCollection.data[index];

        requiredWords = levelData.wordCount;
        requiredScore = levelData.totalScore;
        timeLeft = levelData.timeSec;
        useScoreObjective = requiredScore > 0;
        hasTimer = timeLeft > 0;
        LevelGridData = levelData.gridData;
        gridSize = levelData.gridSize;

        wordsFormed = 0;
        totalScore = 0;

        GameManager.Instance.Grid.SetGridSize(gridSize);
        GameManager.Instance.Grid.LoadStaticGrid(LevelGridData);

        GameManager.Instance.UiManager.ShowLevelObjectives(requiredWords, requiredScore, hasTimer ? timeLeft : -1f);
    }

    public void LoadNextLevel()
    {
        currentLevelIndex++;
        LoadLevel(currentLevelIndex);
    }
}
