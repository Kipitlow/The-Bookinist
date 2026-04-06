using UnityEngine;

/// <summary>
/// Outil de debug : trace un rayon depuis l'écran et affiche hits / sphere handle en Editor.
/// </summary>
public class RayCastDebugger : MonoBehaviour
{
    #region Variables

    [Header("Ray Settings")]
    public Camera targetCamera;
    public float rayLength = 100f;
    public LayerMask hitMask = ~0;

    [Header("Debug")]
    public Color rayColor = Color.green;
    public Color hitColor = Color.red;
    public float hitSphereRadius = 0.15f;
    public bool logHits = true;

    #endregion

    #region Methods

    public void DebugRayFromScreen(Vector2 screenPosition)
    {
        if (!targetCamera)
            targetCamera = Camera.main;

        Ray ray = targetCamera.ScreenPointToRay(screenPosition);

        Debug.DrawRay(ray.origin, ray.direction * rayLength, rayColor, 10f);

        if (Physics.Raycast(ray, out RaycastHit hit, rayLength, hitMask))
        {
            Debug.DrawLine(ray.origin, hit.point, hitColor, 1f);
            DrawHitSphere(hit.point);

            if (logHits)
                Debug.Log($"Hit: {hit.collider.name}");
        }
    }

    void DrawHitSphere(Vector3 position)
    {
#if UNITY_EDITOR
        UnityEditor.Handles.color = hitColor;
        UnityEditor.Handles.SphereHandleCap(
            0,
            position,
            Quaternion.identity,
            hitSphereRadius,
            EventType.Repaint
        );
#endif
    }

    #endregion
}
