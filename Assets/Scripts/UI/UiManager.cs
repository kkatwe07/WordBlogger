using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject startScreenPanel;
    [SerializeField] private GameObject winPopup;
    [SerializeField] private GameObject losePopup;
    [SerializeField] private GameObject backButton;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text levelProgressText;
    [SerializeField] private TMP_Text timerText;

    private int totalScore = 0;
    private int wordsSubmitted = 0;

    void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        winPopup.SetActive(false);
        losePopup.SetActive(false);
        startScreenPanel.SetActive(true);
        backButton.SetActive(false);
        scoreText.gameObject.SetActive(false);
        levelProgressText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);

        GameManager.Instance.ResetGame();
    }

    public void UpdateScore(int wordScore)
    {
        totalScore += wordScore;
        wordsSubmitted++;
        float average = (float)totalScore / wordsSubmitted;
        scoreText.text = $"Score: {totalScore} \nAvg: {average:F1}";
    }

    public void ShowLevelObjectives(int targetWords, int targetScore, float time = 0)
    {
        levelProgressText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(targetScore > 0);

        if (time > 0)
        {
            timerText.gameObject.SetActive(true);
            UpdateTimer(time);
        }
        else
        {
            timerText.gameObject.SetActive(false);
        }

        if (targetWords > 0 && targetScore > 0 && time > 0)
            levelProgressText.text = $"Make {targetWords} words\nReach {targetScore} score\nIn {time:F0}s";
        else if (targetWords > 0 && time > 0)
            levelProgressText.text = $"Make {targetWords} words\nIn {time:F0}s";
        else if (targetScore > 0 && time > 0)
            levelProgressText.text = $"Reach {targetScore} score\nIn {time:F0}s";
        else if (targetWords > 0)
            levelProgressText.text = $"Make {targetWords} words";
        else if (targetScore > 0)
            levelProgressText.text = $"Reach {targetScore} score";
        else
            levelProgressText.text = $"No objectives set";;
    }

    public void UpdateLevelProgress(int wordsMade, int currentScore, int targetWords, int bonusCollected)
    {
        levelProgressText.text = $"\nWords: {wordsMade}/{targetWords}\nScore: {currentScore}\nBugs: {bonusCollected}";
    }

    public void ResetScoreUI()
    {
        totalScore = 0;
        wordsSubmitted = 0;
        scoreText.text = $"Score: 0 | Avg: 0";
        backButton.SetActive(true);
    }

    public void ShowWinScreen(int levelIndex)
    {
        winPopup.SetActive(true);
        winPopup.GetComponentInChildren<TMP_Text>().text = $"Level {levelIndex + 1} Complete!";
        winPopup.transform.localScale = Vector3.zero;
        LeanTween.scale(winPopup, Vector3.one, 0.6f).setEaseOutBack();
    }

    public void UpdateTimer(float timer)
    {
        int seconds = Mathf.CeilToInt(timer);
        timerText.text = $"Time: {seconds}s";
    }

    public void ShowLoseScreen()
    {
        losePopup.SetActive(true);
        losePopup.transform.localScale = Vector3.zero;
        LeanTween.scale(losePopup, Vector3.one, 0.6f).setEaseOutBack();
    }

    public void ShowFinalCompletionScreen()
    {
        winPopup.SetActive(true);
        winPopup.GetComponentInChildren<TMP_Text>().text = "All Levels Complete!";
        winPopup.transform.localScale = Vector3.zero;
        LeanTween.scale(winPopup, Vector3.one, 0.6f).setEaseOutElastic();
    }

    public void OnTapNextLevel()
    {
        winPopup.SetActive(false);
        GameManager.Instance.LevelsMode.LoadNextLevel();
    }

    public void OnTapEndlessMode()
    {
        ResetScoreUI();
        startScreenPanel.SetActive(false);
        var endless = FindFirstObjectByType<EndlessMode>();
        GameManager.Instance.StartGame(endless);
        scoreText.gameObject.SetActive(true);
    }

    public void OnTapLevelsMode()
    {
        ResetScoreUI();
        startScreenPanel.SetActive(false);
        var levelMode = FindFirstObjectByType<LevelsMode>();
        GameManager.Instance.StartGame(levelMode);
        scoreText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
    }

    public void OnTapQuitButton()
    { 
        Application.Quit();
    }
}