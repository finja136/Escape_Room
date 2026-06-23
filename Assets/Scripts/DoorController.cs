using UnityEngine;

public class DoorController : MonoBehaviour
{
    private bool opened = false;

    public void OpenDoor()
    {
        if (opened)
            return;

        opened = true;

        transform.position += Vector3.up * 3f;
    }
}