using UnityEngine;

public class BinaryEnterButton : MonoBehaviour
{
    [SerializeField] private ReactorPuzzleManager manager;

    [Header("Visual")]
    [SerializeField] private Transform buttonTop;
    [SerializeField] private float pressedY = 0.01f;
    [SerializeField] private float releasedY = 0.02f;
    [SerializeField] private float resetDelay = 0.1f;

    // Hier funktioniert die Animationi weniger kompliziert als bei den anderen Buttons, da wir verschiedene Sachen ausprobieren wollten
    public void Press()
    {
        Vector3 pos = buttonTop.localPosition;
        pos.y = pressedY;
        buttonTop.localPosition = pos;

        manager.CommitValue();

        Invoke(nameof(Release), resetDelay);
    }

    private void Release()
    {
        Vector3 pos = buttonTop.localPosition;
        pos.y = releasedY;
        buttonTop.localPosition = pos;
    }
}