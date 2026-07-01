using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderWithTimer : MonoBehaviour
{
    public void StartGame()
    {
        if (GameTimer.Instance != null)
        {
            GameTimer.Instance.StartTimer();
        }
        SceneManager.LoadScene("EscapeRoom1");
    }


}