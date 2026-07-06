using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HintSystemManager : MonoBehaviour
{
    [SerializeField] private TMP_Text HintText;
    [SerializeField] private TMP_Text ButtonText;
    [SerializeField] private Button HintButton;

    [SerializeField] private string[] hints;

    private int currentHintIndex = 0;
    private int currentPuzzleIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateUI();
    }

    public void OnHintButtonClicked()
    {
        
        if (currentHintIndex < 3)
        {
            currentHintIndex++;
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


}
