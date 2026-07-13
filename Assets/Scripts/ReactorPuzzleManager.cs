using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class ReactorPuzzleManager : MonoBehaviour // In diesem Rätsel wird Logik und UI zusammen verwaltet
{
    [Header("Puzzle Progress")]
    [SerializeField] private int playerState = 0; // 0–2 (3 Unterrätsel)

    [Header("Puzzle Display")]
    [SerializeField] private Sprite[] puzzleSprites;
    [SerializeField] private UnityEngine.UI.Image puzzleDisplay;

    [Header("Solutions [3 puzzles][4 numbers]")]   // Da die Lösungen Zahlen bis 15 enthalten, kann das Ergebnis nicht einfach als String dargestellt werden, sondern muss mit 3 Arrays übergeben werden
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
    private int currentValue = 0; // 0-15 (4 bits, die Pro Zahl eingegeben werden müssen)
    private int currentDigit = 0; // 0-3 (4 Zahlen, die Pro Rätsel eingegeben werden müssen)
    private int[] enteredValues = new int[4]; // Speichert eingegebene Values

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
    
    private float startTime; // Da das Script am Ende auch den GameScore berechnet, muss es die Zeit wissen, die seit Szenenstart vergangen ist. Da der ReactorCountdown aber runterzählt und wir das Zeitlimit nicht endgültig festlegen wollen, ist es einfacher das hier zu tracken.

    private void Start()
    {
        startTime = Time.time;
        LoadPuzzle();
        ClearDisplay(); // Offmaterial auf alle Zellen
    }

    private void LoadPuzzle() // Zeigt aktuelles Puzzle an und setzt Variablen zurück. Außerdem wird RowIndicator in die nächste Row gesetzt
    {
        if (playerState < puzzleSprites.Length)
            puzzleDisplay.sprite = puzzleSprites[playerState];

        currentValue = 0;
        currentDigit = 0;
        enteredValues = new int[4];

        UpdateRowIndicators();
    }


    public void ToggleBit(int bit) // Wird von Buttons aufgerufen, togglet deren BitValue und Updated die Anzeige
    {
        audioSource.PlayOneShot(buttonClickClip);
        currentValue ^= bit; // togglet bit

        UpdateDisplayLive();
    }

    private void UpdateDisplayLive()
    {
        for (int bit = 0; bit < 4; bit++)
        {
            int mask = 1 << bit;
            bool active = (currentValue & mask) != 0; // Prüft, ob mask in currentValue gesetzt ist

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

    public void CommitValue() // Wird von EnterButton aufgerufen, speichert currentValue in enterdeValues und geht zur nächsten Zahl (RowIndicators). Wenn alle 4 Zahlen eingegeben wurden, wird CheckPuzzle aufgerufen
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

    private void CheckPuzzle() // Vergleicht eingegebene Werte mit Lösung und startet HandleResult Coroutine
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

    private IEnumerator HandleResult(bool correct) // Zeigt Feedback an, spielt Sound ab und geht entweder zum nächsten Rätsel oder setzt das aktuelle zurück
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

    private IEnumerator ShowFeedback(bool correct) // Zeigt FeedbackPanel für feedbackTime an (color hängt von correct ab)
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

    private void AdvancePuzzle() // Erhöht playerState, lädt das nächste Puzzle oder beendet das Spiel, wenn alle Rätsel gelöst wurden
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

    private void FinishGame() // Setzt GameResults (berechnet Score) und lädt GameLobby
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
            rowIndicators[i].material = (i == currentDigit) // Nur der Rowindicator der aktuellen Row wird aktiv
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