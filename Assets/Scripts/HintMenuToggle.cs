using UnityEngine;
using UnityEngine.InputSystem;

public class HintMenuToggle : MonoBehaviour
{
    [Header("References")]
    public FollowHeadUI hintMenu;

    [Header("Input")]
    public InputActionReference toggleAction;

    private bool isOpen;

    private void OnEnable()
    {
        toggleAction.action.performed += Toggle;
        toggleAction.action.Enable();
    }

    private void OnDisable()
    {
        toggleAction.action.performed -= Toggle;
        toggleAction.action.Disable();
    }

    private void Toggle(InputAction.CallbackContext ctx)
    {
        isOpen = !isOpen;

        if (isOpen)
            hintMenu.Show();
        else
            hintMenu.Hide();
    }
}
