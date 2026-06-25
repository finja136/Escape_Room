using UnityEngine;
using UnityEngine.UI;

public class KeypadPuzzleManager : MonoBehaviour
{
    [SerializeField] private Image puzzleImage;
    [SerializeField] private Sprite[] puzzleSprites;

    private void Start()
    {
        ShowPuzzle(0);
    }

    public void ShowPuzzle(int index)
    {
        if (index < 0 || index >= puzzleSprites.Length)
            return;

        puzzleImage.sprite = puzzleSprites[index];
    }
}