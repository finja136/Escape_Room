using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReactorCountdown : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    [SerializeField] private float startMinutes = 30f;

    [Header("Alarm Audio")]
    [SerializeField] private AudioSource alarmAudioSource;
    [SerializeField] private AudioClip alarmClip;

    private bool alarmPlayed = false;
    private float remainingTime;

    private bool GameOver;

    void Start()
    {
        remainingTime = startMinutes * 60f;
        UpdateDisplay();
    }

    void Update()
    {
        if (GameOver) return;

        remainingTime -= Time.deltaTime;

        if (remainingTime < 300)
        {
            PlayAlarmSound();
        }

        if (remainingTime <= 0)
        {
            remainingTime = 0;
            TriggerGameOver();
        }
        UpdateDisplay();
    }

    private void PlayAlarmSound()
    {
        if (!alarmAudioSource.isPlaying)
        {
            alarmAudioSource.clip = alarmClip;
            alarmAudioSource.loop = true;
            alarmAudioSource.Play();
        }
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
        GameResults.GameOver = true;
        GameResults.Won = false;
        GameResults.Score = 0;
        SceneManager.LoadScene("GameLobby");
    }
}