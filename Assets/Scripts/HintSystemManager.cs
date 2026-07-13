using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HintSystemManager : MonoBehaviour // Kontrolliert die UI auf dem HintCanvas
{
    [SerializeField] private TMP_Text HintText;
    [SerializeField] private TMP_Text ButtonText;
    [SerializeField] private Button HintButton;

    [SerializeField] private string[] hints;

    private int currentHintIndex = 0;
    private int currentPuzzleIndex = 0;

    private int hintsUsed = 0;

    // Welcher Hint angezeigt werden muss, wird über currentHintIndex und currentPuzzleIndex bestimmt. currentPuzzleIndex wird von den PuzzleManagern erhöht, currentHintIndex vom Button.
    // Da es für jedes Rätsel 3 Hints gibt, wird currentPuzzleIndex um 3 erhöht, wenn ein Rätsel gelöst wurde, currentHintIndex wird auf 0 zurückgesetzt.
    void Start()
    {
        UpdateUI();
    }

    public void OnHintButtonClicked() // Wenn der Button geklickt wird, wird der aktuelle HintIndex erhöht und die UI aktualisiert.  
    {
        if (currentHintIndex < 3)
        {
            currentHintIndex++;
            hintsUsed++;
            UpdateUI();
        }
    }

    private void UpdateUI() {
        if (currentHintIndex == 0)
            HintText.text = "";
        else
            HintText.text = "Hint " + currentHintIndex + ": " + hints[currentHintIndex + currentPuzzleIndex - 1];

        if (currentHintIndex != 3)
        {
            ButtonText.text = "Hint " + (currentHintIndex +1);
            HintButton.interactable = true;
        }
        else
        {
            ButtonText.text = "No more hints";
            HintButton.interactable = false;
        }

    }

    public void UpdatePuzzleIndex()
    {
        currentPuzzleIndex+=3;
        currentHintIndex = 0;
        UpdateUI();
    }

    public int GetHintsUsed()
    {
        return hintsUsed;
    }


}
