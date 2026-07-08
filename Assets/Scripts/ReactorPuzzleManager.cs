using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class ReactorPuzzleManager : MonoBehaviour
{
    [Header("Puzzle Progress")]
    [SerializeField] private int playerState = 0; // 0–2 (3 Unterrätsel)

    [Header("Puzzle Display")]
    [SerializeField] private Sprite[] puzzleSprites;
    [SerializeField] private UnityEngine.UI.Image puzzleDisplay;

    [Header("Solutions [3 puzzles][4 numbers]")]
    [SerializeField] private int[] solutionPuzzle1 = new int[4];
    [SerializeField] private int[] solutionPuzzle2 = new int[4];
    [SerializeField] private int[] solutionPuzzle3 = new int[4];

    [Header("Display Cells (16 total = 4x4)")]
    [SerializeField] private Renderer[] cells;

    [Header("Materials (Bit Colors)")]
    [SerializeField] private Material offMaterial;
    [SerializeField] private Material bit1Material; // 1
    [SerializeField] private Material bit2Material; // 2
    [SerializeField] private Material bit4Material; // 4
    [SerializeField] private Material bit8Material; // 8

    [Header("Bit State")]
    private int currentValue = 0;
    private int currentDigit = 0;
    private int[] enteredValues = new int[4];

    [Header("Feedback")]
    [SerializeField] private Image feedbackPanel;
    [SerializeField] private Color correctColor = Color.green;
    [SerializeField] private Color wrongColor = Color.red;
    [SerializeField] private float feedbackTime = 0.5f;
    [SerializeField] private Renderer[] rowIndicators;
    [SerializeField] private Material inactiveMat;
    [SerializeField] private Material activeMat;

    [Header("Scene Transition")]
    [SerializeField] private string nextSceneName;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip correctClip;
    [SerializeField] private AudioClip wrongClip;
    [SerializeField] private AudioClip buttonClickClip;


    [Header("Hint System")]
    [SerializeField] private HintSystemManager hintSystemManager;
    // =========================
    // INIT
    // =========================

    private float startTime;

    private void Start()
    {
        startTime = Time.time;
        LoadPuzzle();
        ClearDisplay();
    }

    private int[] GetSolution(int index)
    {
        switch (index)
        {
            case 0: return solutionPuzzle1;
            case 1: return solutionPuzzle2;
            case 2: return solutionPuzzle3;
            default: return solutionPuzzle1;
        }
    }

    private void LoadPuzzle()
    {
        if (playerState < puzzleSprites.Length)
            puzzleDisplay.sprite = puzzleSprites[playerState];

        currentValue = 0;
        currentDigit = 0;
        enteredValues = new int[4];

        UpdateRowIndicators();
    }

    // =========================
    // BIT INPUT (called by buttons)
    // =========================

    public void ToggleBit(int bit)
    {
        audioSource.PlayOneShot(buttonClickClip);
        currentValue ^= bit; // toggle

        UpdateDisplayLive();
    }

    // =========================
    // LIVE DISPLAY UPDATE
    // =========================

    private void UpdateDisplayLive()
    {
        for (int bit = 0; bit < 4; bit++)
        {
            int mask = 1 << bit;
            bool active = (currentValue & mask) != 0;

            int index = currentDigit * 4 + bit;

            if (active)
            {
                cells[index].material = GetMaterialForBit(mask);
            }
            else
            {
                cells[index].material = offMaterial;
            }
        }
    }

    private IEnumerator ShowFeedback(bool correct)
    {
        if (feedbackPanel != null)
        {
            feedbackPanel.color = correct ? correctColor : wrongColor;
            feedbackPanel.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(feedbackTime);

        if (feedbackPanel != null)
            feedbackPanel.gameObject.SetActive(false);
    }

    private Material GetMaterialForBit(int bit)
    {
        switch (bit)
        {
            case 1: return bit1Material;
            case 2: return bit2Material;
            case 4: return bit4Material;
            case 8: return bit8Material;
            default: return offMaterial;
        }
    }

    // =========================
    // ENTER VALUE
    // =========================

    public void CommitValue()
    {   
        audioSource.PlayOneShot(buttonClickClip);
        enteredValues[currentDigit] = currentValue;

        currentDigit++;
        currentValue = 0;

        UpdateRowIndicators();

        if (currentDigit >= 4)
        {
            CheckPuzzle();
        }
        else
        {
            UpdateDisplayLive();
        }
    }

    // =========================
    // CHECK PUZZLE
    // =========================

    private void CheckPuzzle()
    {
        bool correct = true;

        for (int i = 0; i < 4; i++)
        {
            if (enteredValues[i] != GetSolution(playerState)[i])
            {
                correct = false;
                break;
            }
        }

        StartCoroutine(HandleResult(correct));
    }

    private IEnumerator HandleResult(bool correct)
    {
        StartCoroutine(ShowFeedback(correct));

        yield return new WaitForSeconds(feedbackTime);

        if (correct)
        {
            audioSource.PlayOneShot(correctClip);
            AdvancePuzzle();
            hintSystemManager.UpdatePuzzleIndex();
        }
        else
        {
            audioSource.PlayOneShot(wrongClip);
            ResetCurrentPuzzle();
        }
    }

    // =========================
    // PROGRESSION
    // =========================

    private void AdvancePuzzle()
    {
        playerState++;

        if (playerState >= puzzleSprites.Length)
        {
            FinishGame();
            return;
        }

        LoadPuzzle();
        ClearDisplay();
    }

    // =========================
    // FINISH → SCENE CHANGE
    // =========================

    private void FinishGame()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            GameResults.GameOver = true;
            GameResults.Won = true;
            GameResults.Score = ComputeScore();
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("No scene assigned!");
        }
    }

    // =========================
    // RESET
    // =========================

    private void ResetCurrentPuzzle()
    {
        currentDigit = 0;
        currentValue = 0;
        enteredValues = new int[4];

        UpdateRowIndicators();
        ClearDisplay();
    }

    private void ClearDisplay()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].material = offMaterial;
        }
    }

    private void UpdateRowIndicators()
    {
        for (int i = 0; i < rowIndicators.Length; i++)
        {
            rowIndicators[i].material = (i == currentDigit)
                ? activeMat
                : inactiveMat;
        }
    }

    private int ComputeScore()
    {
        float elapsedTime = Time.time - startTime;
        int baseScore = 20000;
        int hintsUsed = hintSystemManager.GetHintsUsed();
        int scorePenalty = hintsUsed * 500 + Mathf.RoundToInt(elapsedTime*3.7f);
        return Mathf.Max(baseScore - scorePenalty, 0);
    }
}