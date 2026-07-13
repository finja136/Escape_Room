using UnityEngine;
using NavKeypad;

public class EnergyPuzzleManager : MonoBehaviour
{
    [SerializeField] private EnergySlot[] slots;
    [SerializeField] private Keypad keypad;
    [SerializeField] private HintSystemManager hintSystem;

    [ContextMenu("Force Solve Puzzle")]
    public void ForceSolvePuzzle() // Hilfsmethode zum Debuggen
    {
        Debug.Log("Puzzle 1 Force Solve");
        keypad.UnlockKeypad();
        hintSystem.UpdatePuzzleIndex();

    }

    public void CheckPuzzle()
    {
        foreach (EnergySlot slot in slots) //checkt für jeden Batterieslot, ob die richtige Batterie eingesetzt wurde
        {                                  //dafür wird über den SocketInteractor das Objekt geholt, gepüft ob es eine EnergyCell ist und wenn ja ob die ID stimmt. Nur wenn für alle Slots die ID stimmt, gibt das foreach kein Return.
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
        hintSystem.UpdatePuzzleIndex();
    }
}