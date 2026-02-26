using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform cameraTransform;
    [Range(0f, 1f)]
    public float parallaxFactor = 0.3f;

    [Header("Scaling")]
    public float scaleFactor = 0.05f;

    private Vector3 initialPosition;
    private Vector3 cameraStart;

    void Start()
    {
        initialPosition = transform.position;
        cameraStart = cameraTransform.position;
    }

    void LateUpdate()
    {
        // Camera movement delta
        Vector3 delta = cameraTransform.position - cameraStart;

        // Apply parallax ONLY on X/Y, keep Z fixed
        transform.position = new Vector3(
            initialPosition.x + delta.x * parallaxFactor,
            initialPosition.y + delta.y * parallaxFactor,
            initialPosition.z + delta.y * parallaxFactor
        );
    }
}
