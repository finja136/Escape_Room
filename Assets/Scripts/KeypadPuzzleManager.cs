using UnityEngine;
using UnityEngine.UI;

public class KeypadPuzzleManager : MonoBehaviour
{
    [SerializeField] private Image puzzleImage;
    [SerializeField] private Sprite[] puzzleSprites;

    private void Start()
    {
        ShowPuzzle(3);
    }

    public void ShowPuzzle(int index)
    {
        if (index < 0 || index >= puzzleSprites.Length)
            return;
        if (index != 3)
        {
            puzzleImage.color = new Color(1f,1f,1f);
            puzzleImage.sprite = puzzleSprites[index];
        }
        else
            puzzleImage.sprite = puzzleSprites[index];
    }
}