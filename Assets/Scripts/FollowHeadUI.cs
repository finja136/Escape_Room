using UnityEngine;

public class FollowHeadUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform head;

    [Header("Local Offset")]
    [SerializeField] private Vector3 currentOffset = new Vector3(0.2f, -0.15f, 0.6f); //relative Position zum Kopf

    [Header("Rotation")]
    [SerializeField] private float rotationThreshold = 60f; // Rotations deadzone in Grad
    [SerializeField] private float rotationSmooth = 8f; // Faktor, wie schnell Rotation aktualisiert wird. 

    private bool visible;
    private bool grabbed; // Während das Menü gegriffen wird, wird die Position nicht aktualisiert

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
        if (!visible || head == null || grabbed) //Wenn Menü nicht angezeigt wird, Kopf nicht gesetzt oder Menü gegriffen wird, dann keine Aktualisierung
            return;

        Quaternion currentHeadRotation = GetHeadYawRotation();

        float delta =
            Quaternion.Angle(referenceRotation, currentHeadRotation);

        if (delta >= rotationThreshold) // Wenn mehr Rotation-Deadzone überschritten wird, wird Referenzrotation aktualisiert und Zielrotation angepasst, damit Menü nicht springt
        {
            Quaternion deltaRotation =
                currentHeadRotation * Quaternion.Inverse(referenceRotation);

            targetRotation = deltaRotation * targetRotation;

            referenceRotation = currentHeadRotation;
        }

        Vector3 forward = referenceRotation * Vector3.forward;
        Vector3 right = referenceRotation * Vector3.right;

        transform.position = 
            head.position +
            right * currentOffset.x +
            Vector3.up * currentOffset.y +
            forward * currentOffset.z;
        
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSmooth);
    }


    private Quaternion GetHeadYawRotation() // Gibt Kopfrotation zurück, wobei nur Yaw-Rotation (also Rotation um die Y-Achse) berücksichtigt wird.
    {
        Vector3 forward = head.forward;
        forward.y = 0f; 

        if (forward.sqrMagnitude < 0.0001f) // Wenn Rotation null ist wird keine Rotation zurückgegeben, da sonst LookRotation einen Fehler wirft
            return Quaternion.identity;

        forward.Normalize();

        return Quaternion.LookRotation(forward, Vector3.up);
    }

    public void Show() //Wird über InputAction Reference aufgerufen, um Menü anzuzeigen. Setzt Referenzrotation und Zielrotation auf aktuelle Rotation, damit Menü nicht springt.
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

    public void BeginGrab() // Wird über XRGrabInteractable aufgerufen, wenn Menü gegriffen wird. Verhindert, dass Menü aktualisert wird
    {
        grabbed = true;
    }

    public void EndGrab() // Wird über XRGrabInteractable aufgerufen, wenn Menü losgelassen wird. Setzt neue Referenzrotation
    {
        grabbed = false;

        referenceRotation = GetHeadYawRotation();
        targetRotation = transform.rotation;

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