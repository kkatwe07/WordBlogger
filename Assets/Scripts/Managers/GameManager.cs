using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // Game Manager Instance
    public static GameManager Instance;

    // serialized fields
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private WordValidator _wordValidator;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private EndlessMode _endlessMode;
    [SerializeField] private LevelsMode _levelsMode;

    // public properties
    public GridManager Grid => _gridManager;
    public UIManager UiManager => _uiManager;
    public LevelsMode LevelsMode => _levelsMode;
    public WordValidator WordValidator => _wordValidator;

    // private variables
    private GameModeBase currentGameMode;


    private void Awake()
    {
        Instance = this;
        _wordValidator.LoadWords();
    }

    public void StartGame(GameModeBase mode)
    {
        currentGameMode = mode;
        currentGameMode.Init();
    }

    public void SubmitWord(string word, List<LetterTile> tiles)
    {
        currentGameMode.OnWordSubmitted(word, tiles);
    }
    
    public void ResetGame()
    {
        _gridManager.ClearGrid();
        currentGameMode = null;
    }
}