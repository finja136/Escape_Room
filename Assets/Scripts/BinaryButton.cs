using UnityEngine;


public class BinaryButton : MonoBehaviour
{
    [SerializeField] private int bitValue;
    [SerializeField] private ReactorPuzzleManager manager;

    [SerializeField] private Transform buttonTop;
    [SerializeField] private float pressedY = 0.01f;
    [SerializeField] private float normalY = 0.02f;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable;

    private void Awake()
    {
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();
        interactable.selectEntered.AddListener(_ => OnPressed());
    }

    private void OnPressed()
    {
        manager.ToggleBit(bitValue);
        Animate();
    }

    private void Animate()
    {
        Vector3 pos = buttonTop.localPosition;
        pos.y = pressedY;
        buttonTop.localPosition = pos;

        Invoke(nameof(Reset), 0.1f);
    }

    private void Reset()
    {
        Vector3 pos = buttonTop.localPosition;
        pos.y = normalY;
        buttonTop.localPosition = pos;
    }
}