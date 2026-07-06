using UnityEngine;

public class FollowHeadUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform head;

    [Header("Offset")]
    [SerializeField] private Vector3 currentOffset = new Vector3(0.2f, -0.15f, 0.6f);

    [Header("Rotation Settings")]
    [SerializeField] private float rotationThreshold = 50f;
    [SerializeField] private float rotationSmooth = 6f;

    private bool visible;
    private bool grabbed;

    void LateUpdate()
    {
        if (!visible || head == null || grabbed)
            return;

        // ---------------------------
        // POSITION (dein funktionierender Teil)
        // ---------------------------

        Vector3 forward = head.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 right = head.right;
        right.y = 0f;
        right.Normalize();

        Vector3 targetPos =
            head.position +
            forward * currentOffset.z +
            right * currentOffset.x +
            Vector3.up * currentOffset.y;

        transform.position = targetPos;

        // ---------------------------
        // ROTATION (NEU: Deadzone System)
        // ---------------------------

        Vector3 toMenu = transform.position - head.position;
        toMenu.y = 0f;

        Vector3 headForward = head.forward;
        headForward.y = 0f;

        if (toMenu.sqrMagnitude > 0.001f)
        {
            float angle = Vector3.Angle(headForward, toMenu);

            if (angle > rotationThreshold)
            {
                Quaternion targetRot = Quaternion.LookRotation(toMenu);

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRot,
                    Time.deltaTime * rotationSmooth);
            }
        }
    }

    // ---------------------------
    // VISIBILITY
    // ---------------------------

    public void Show()
    {
        visible = true;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        visible = false;
        gameObject.SetActive(false);
    }

    // ---------------------------
    // GRAB SYSTEM
    // ---------------------------

    public void BeginGrab()
    {
        grabbed = true;
    }

    public void EndGrab()
    {
        grabbed = false;

        // Offset neu speichern
        Vector3 delta = transform.position - head.position;

        Vector3 forward = head.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 right = head.right;
        right.y = 0f;
        right.Normalize();

        currentOffset = new Vector3(
            Vector3.Dot(delta, right),
            delta.y,
            Vector3.Dot(delta, forward)
        );
    }
}