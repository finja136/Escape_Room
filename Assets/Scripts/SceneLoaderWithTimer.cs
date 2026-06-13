using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderWithTimer : MonoBehaviour
{
    public void StartGame()
    {
        GameTimer.Instance.StartTimer();
        SceneManager.LoadScene("EscapeRoom1");
    }
}