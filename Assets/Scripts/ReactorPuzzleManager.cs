using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ReactorPuzzleManager : MonoBehaviour
{
    [Header("Puzzle Display (Sprites)")]
    [SerializeField] private Image puzzleDisplay;
    [SerializeField] private Sprite[] puzzleSprites; // 3

    [Header("Binary Grid (16 cells)")]
    [SerializeField] private Renderer[] cells;

    [Header("Materials")]
    [SerializeField] private Material offMaterial;
    [SerializeField] private Material activeMaterial;

    [Header("Solutions (3 puzzles × 4 numbers)")]
    [SerializeField]
    private int[][] solutionCodes =
    {
        new int[4],
        new int[4],
        new int[4]
    };

    [SerializeField] private int playerState = 0;

    private int currentDigit = 0;
    private int currentValue = 0;

    private int[] enteredValues = new int[4];

    private void Awake()
    {
        LoadPuzzle();
        ClearGrid();
    }

    // =========================
    // LOAD PUZZLE STEP
    // =========================

    private void LoadPuzzle()
    {
        if (playerState < puzzleSprites.Length)
            puzzleDisplay.sprite = puzzleSprites[playerState];

        currentDigit = 0;
        currentValue = 0;
        enteredValues = new int[4];
    }

    // =========================
    // BIT TOGGLE (4 Buttons)
    // =========================

    public void ToggleBit(int bit)
    {
        currentValue ^= bit;
        UpdateCurrentDisplay();
    }

    // =========================
    // LIVE DISPLAY UPDATE
    // =========================

    private void UpdateCurrentDisplay()
    {
        for (int bit = 0; bit < 4; bit++)
        {
            bool active = (currentValue & (1 << bit)) != 0;

            int index = currentDigit * 4 + bit;

            cells[index].material = active ? activeMaterial : offMaterial;
        }
    }

    // =========================
    // ENTER VALUE
    // =========================

    public void CommitValue()
    {
        enteredValues[currentDigit] = currentValue;

        currentDigit++;
        currentValue = 0;

        if (currentDigit >= 4)
        {
            CheckPuzzle();
        }
        else
        {
            UpdateCurrentDisplay();
        }
    }

    // =========================
    // CHECK
    // =========================

    private void CheckPuzzle()
    {
        bool correct = true;

        for (int i = 0; i < 4; i++)
        {
            if (enteredValues[i] != solutionCodes[playerState][i])
            {
                correct = false;
                break;
            }
        }

        if (correct)
        {
            AdvanceState();
        }
        else
        {
            ResetCurrentPuzzle();
        }
    }

    // =========================
    // PROGRESSION
    // =========================

    private void AdvanceState()
    {
        playerState++;

        if (playerState >= puzzleSprites.Length)
        {
            PuzzleComplete();
            return;
        }

        LoadPuzzle();
        ClearGrid();
    }

    private void PuzzleComplete()
    {
        Debug.Log("Reactor Puzzle Completed");
        // Tür, Event etc.
    }

    // =========================
    // RESET CURRENT PUZZLE
    // =========================

    private void ResetCurrentPuzzle()
    {
        currentDigit = 0;
        currentValue = 0;
        enteredValues = new int[4];

        ClearGrid();
    }

    private void ClearGrid()
    {
        for (int i = 0; i < cells.Length; i++)
            cells[i].material = offMaterial;
    }

    // =========================
    // PUBLIC ACCESS (for future hints)
    // =========================

    public int GetPlayerState()
    {
        return playerState;
    }
}