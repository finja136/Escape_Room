using UnityEngine;
using UnityEngine.InputSystem;

public class HintMenuToggle : MonoBehaviour // Togglet das HintMenu, wenn auf linkem Controller die Menü-Taste gedrückt wird.
{
    [Header("References")]
    public FollowHeadUI hintMenu;

    [Header("Input")]
    public InputActionReference toggleAction;

    private bool isOpen;

    private void OnEnable() // Aktiviert Hint-System nur, wenn die Escape-Room szene geladen wird. Sonst wird es deaktiviert
    {
        toggleAction.action.performed += Toggle;
        toggleAction.action.Enable();
    }

    private void OnDisable()
    {
        toggleAction.action.performed -= Toggle;
        toggleAction.action.Disable();
    }

    private void Toggle(InputAction.CallbackContext ctx) //Togglet das HintMenu über die InputAction
    {
        isOpen = !isOpen;

        if (isOpen)
            hintMenu.Show();
        else
            hintMenu.Hide();
    }
}
