using UnityEngine;
using TMPro;

public class GameEndManager : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TMP_Text winPanelText;

    void Start()
    {
        if(GameResults.GameOver) // Greift auf Spielergebnisse zu, um Sore und Gewinnstatus anzuzeigen
        { 
            winPanel.SetActive(true);
            if (GameResults.Won)
            {
                winPanelText.text = "<Size=150%><b>Congratulations!</b></size> \n You survived! Now your stuck on an alien space ship" +
                    " alone in outer space. While you hope someone will rescue you, you can take some comfort in your\n" +
                    "<Size=120%><b>Gamescore: </b></Size>" + GameResults.Score + "\n If you want to try again for a better score, interact with the hologram.";
            }
            else
            {
                winPanelText.text = "<Size=150%><b>Game Over! </b></size>\n Sadly you couldnt make it. We are sorry for your loss. \n" +
                    "<size=120%><b>Gamescore: </b></size>" + GameResults.Score + "\n If you want to try again, interact with the hologram.";
            }
        }
        
    }
}
