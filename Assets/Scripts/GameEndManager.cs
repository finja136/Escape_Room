using UnityEngine;
using TMPro;

public class GameEndManager : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TMP_Text winPanelText;

    void Start()
    {
        if(GameResults.GameOver)
        { 
            winPanel.SetActive(true);
            if (GameResults.Won)
            {
                winPanelText.text = "Congratulations! \n You survived! Now your stuck on an alien space ship" +
                    " alone in outer space... While you wait for someone to rescue you, you can take a look at your \n" +
                    "Gamescore: " + GameResults.Score;
            }
            else
            {
                winPanelText.text = "Game Over! \n Sadly you couldnt make it. We are sorry for your loss. \n" +
                    "Gamescore: " + GameResults.Score;
            }
        }
        
    }
}
