using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReactorCountdown : MonoBehaviour
{
    [SerializeField] private TMP_Text[] timerText;

    [SerializeField] private float startMinutes = 30f;

    [Header("Alarm Audio")]
    [SerializeField] private AudioSource alarmAudioSource;
    [SerializeField] private AudioClip alarmClip;

    [Header("Reactor Light Pulse")]
    [SerializeField] private ReactorLightPulse reactorLightPulse;

    private bool PulseActive;
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

        if (remainingTime < 300) // Ab 5 Minuten wird alarm abgespielt und die Emergency Lights im Reactor raum pulsieren
        {
            PlayAlarmSound();
            StartLightPulse();
        }

        if (remainingTime <= 0)
        {
            remainingTime = 0;
            TriggerGameOver(); // Wenn Zeit abläuft wird in GameLobby teleortiert und GameOver gesetzt
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

    private void StartLightPulse()
    {
        if(!PulseActive)
        {
            reactorLightPulse.StartEmergencyMode();
            PulseActive = true;
        }
    }

    private void UpdateDisplay()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);

        for (int i = 0; i < timerText.Length; i++)
        {
            timerText[i].text = "REACTOR MELTDOWN IN\n" +
                                $"{minutes:00}:{seconds:00}";
        }
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