using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BinaryEnterButton : MonoBehaviour
{
    [SerializeField] private ReactorPuzzleManager manager;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable;

    private void Awake()
    {
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();
        interactable.selectEntered.AddListener(_ => manager.CommitValue());
    }
}