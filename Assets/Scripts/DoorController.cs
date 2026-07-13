using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip doorOpenSound;

    private bool opened = false;

    public void OpenDoor()
    {   
        if (opened) return;

        opened = true;
        animator.SetTrigger("Open");
        audioSource.PlayOneShot(doorOpenSound);
    }
}