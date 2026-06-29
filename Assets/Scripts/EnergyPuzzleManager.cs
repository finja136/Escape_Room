using UnityEngine;
using NavKeypad;

public class EnergyPuzzleManager : MonoBehaviour
{
    [SerializeField] private EnergySlot[] slots;
    [SerializeField] private DoorController door;
    [SerializeField] private Keypad keypad;

    [ContextMenu("Force Solve Puzzle")]
    public void ForceSolvePuzzle()
    {
        Debug.Log("PUZZLE FORCED COMPLETE");
        keypad.UnlockKeypad();
    }

    public void CheckPuzzle()
    {
        foreach (EnergySlot slot in slots)
        {
            var socket = slot.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();

            if (socket.interactablesSelected.Count == 0)
                return;

            GameObject insertedObject =
                socket.interactablesSelected[0].transform.gameObject;

            EnergyCell cell =
                insertedObject.GetComponent<EnergyCell>();

            if (cell == null)
                return;

            if (cell.cellID != slot.requiredCellID)
                return;
        }

        keypad.UnlockKeypad();
    }
}