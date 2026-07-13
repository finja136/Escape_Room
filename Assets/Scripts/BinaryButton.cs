using System.Collections;
using UnityEngine;

public class BinaryButton : MonoBehaviour
{
    [Header("Bit Value (1,2,4,8)")]
    [SerializeField] private int bitValue;

    [Header("References")]
    [SerializeField] private ReactorPuzzleManager manager;

    [Header("Button Animation Settings")]
    [SerializeField] private float moveSpeed = 0.1f;
    [SerializeField] private float moveDist = 0.0025f;
    [SerializeField] private float pressedTime = 0.1f;

    private bool moving; // Flag um zu überprüfen, ob die Animation bereits läuft

    public void PressButton() // Methode wird beim Button-Press aufgerufen, um den Bitwert zu toggeln und die Animation zu starten
    {
        if (!moving)
        {
            manager.ToggleBit(bitValue);
            StartCoroutine(MoveSmooth());
        }
    }

    private IEnumerator MoveSmooth() // Methode wurde fast unverändert aus KeypadButton übernommen
    {
        moving = true;

        Vector3 startPos = transform.localPosition;
        Vector3 endPos = startPos + new Vector3(0, 0, moveDist);
        float elapsedTime = 0f;

        while (elapsedTime < moveSpeed)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / moveSpeed);

            transform.localPosition = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        transform.localPosition = endPos;

        yield return new WaitForSeconds(pressedTime);

        startPos = transform.localPosition;
        endPos = startPos - new Vector3(0, 0, moveDist);
        elapsedTime = 0f;

        while (elapsedTime < moveSpeed)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / moveSpeed);
            transform.localPosition = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
        transform.localPosition = endPos;
        moving = false;
    }
}