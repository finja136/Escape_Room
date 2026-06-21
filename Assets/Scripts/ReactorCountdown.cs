using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReactorCountdown : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    [SerializeField] private float startMinutes = 30f;

    private float remainingTime;

    public bool GameOver = false;

    void Start()
    {
        remainingTime = startMinutes * 60f;
        UpdateDisplay();
    }

    void Update()
    {
        if (GameOver) return;

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0)
        {
            remainingTime = 0;
            TriggerGameOver();
        }
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);

        timerText.text = "REACTOR MELTDOWN IN\n" +
                 $"{minutes:00}:{seconds:00}";
    }
    void TriggerGameOver()

    {
        GameOver = true;

        SceneManager.LoadScene("GameLobby");
    }
}