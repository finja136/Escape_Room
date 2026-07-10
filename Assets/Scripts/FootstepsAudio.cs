using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private AudioSource audioSource;

    [Header("Footstep Sounds")]
    [SerializeField] private AudioClip[] footstepClips;

    [Header("Settings")]
    [SerializeField] private float stepDistance = 1.8f;
    [SerializeField] private float minimumMovement = 0.05f;

    private Vector3 lastPosition;
    private float distanceSinceLastStep;


    private void Start()
    {
        lastPosition = player.position;
    }


    private void Update()
    {
        Vector3 movement = player.position - lastPosition;

        // Kopfbewegungen ignorieren
        movement.y = 0;

        float distance = movement.magnitude;

        if (distance > minimumMovement)
        {
            distanceSinceLastStep += distance;
        }


        if (distanceSinceLastStep >= stepDistance)
        {
            PlayFootstep();
            distanceSinceLastStep = 0;
        }


        lastPosition = player.position;
    }


    private void PlayFootstep()
    {
        if (footstepClips.Length == 0)
            return;

        AudioClip clip =
            footstepClips[Random.Range(0, footstepClips.Length)];

        audioSource.PlayOneShot(clip);
    }
}