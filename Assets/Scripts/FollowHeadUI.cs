using UnityEngine;

public class FollowHeadUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform head;

    [Header("Local Offset")]
    [SerializeField] private Vector3 currentOffset = new Vector3(0.2f, -0.15f, 0.6f);

    [Header("Rotation")]
    [SerializeField] private float rotationThreshold = 60f;
    [SerializeField] private float rotationSmooth = 8f;

    private bool visible;
    private bool grabbed;

    // Referenzrotation, auf die sich der Offset bezieht
    private Quaternion referenceRotation;

    // Zielrotation des Menüs
    private Quaternion targetRotation;

    private void Start()
    {
        if (head == null)
            return;

        referenceRotation = GetHeadYawRotation();
        targetRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        if (!visible || head == null || grabbed)
            return;

        //----------------------------------
        // Prüfen, ob Deadzone überschritten
        //----------------------------------

        Quaternion currentHeadRotation = GetHeadYawRotation();

        float delta =
            Quaternion.Angle(referenceRotation, currentHeadRotation);

        if (delta >= rotationThreshold)
        {
            // Wie weit hat sich der Spieler wirklich gedreht?
            Quaternion deltaRotation =
                currentHeadRotation * Quaternion.Inverse(referenceRotation);

            // Menürotation mitdrehen
            targetRotation = deltaRotation * targetRotation;

            // Neue Referenz
            referenceRotation = currentHeadRotation;
        }

        //----------------------------------
        // Position
        //----------------------------------

        Vector3 forward = referenceRotation * Vector3.forward;
        Vector3 right = referenceRotation * Vector3.right;

        transform.position =
            head.position +
            right * currentOffset.x +
            Vector3.up * currentOffset.y +
            forward * currentOffset.z;

        //----------------------------------
        // Rotation
        //----------------------------------

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSmooth);
    }

    //------------------------------------------------

    private Quaternion GetHeadYawRotation()
    {
        Vector3 forward = head.forward;
        forward.y = 0f;

        if (forward.sqrMagnitude < 0.0001f)
            return Quaternion.identity;

        forward.Normalize();

        return Quaternion.LookRotation(forward, Vector3.up);
    }

    //------------------------------------------------
    // Visibility
    //------------------------------------------------

    public void Show()
    {
        visible = true;
        gameObject.SetActive(true);

        referenceRotation = GetHeadYawRotation();
        targetRotation = transform.rotation;
    }

    public void Hide()
    {
        visible = false;
        gameObject.SetActive(false);
    }

    //------------------------------------------------
    // Grab
    //------------------------------------------------

    public void BeginGrab()
    {
        grabbed = true;
    }

    public void EndGrab()
    {
        grabbed = false;

        // Neue Referenzrotation setzen
        referenceRotation = GetHeadYawRotation();
        targetRotation = transform.rotation;

        // Offset relativ zur Referenzrotation berechnen
        Vector3 delta = transform.position - head.position;

        Vector3 forward = referenceRotation * Vector3.forward;
        Vector3 right = referenceRotation * Vector3.right;

        currentOffset = new Vector3(
            Vector3.Dot(delta, right),
            delta.y,
            Vector3.Dot(delta, forward)
        );
    }
}