using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private bool opened = false;

 

    public void OpenDoor()
    {   
        if (opened) return;

        opened = true;
        animator.SetTrigger("Open");
    }
}