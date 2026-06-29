using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("UI Elemente")]
    public TextMeshProUGUI scoreText;

    [Header("Spiel-Status")]
    public int totalPuzzles = 5;
    private int solvedPuzzles = 0;

    void Awake()
    {
        if (instance == null) { instance = this; }
    }

    void Start()
    {
        UpdateScoreUI();
    }

    public void RegisterSolvedPuzzle()
    {
        solvedPuzzles++;
        UpdateScoreUI();
        
        if (solvedPuzzles >= totalPuzzles)
        {
            scoreText.text += "\n<color=green>Glückwunsch! Du bist entkommen!</color>";
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = solvedPuzzles + " / " + totalPuzzles;
        }
    }
}