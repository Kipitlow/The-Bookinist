using UnityEngine;

/// <summary>
/// Parallax simple : déplace l'objet en fonction du mouvement de la caméra.
/// </summary>
public class Parallax : MonoBehaviour
{
    #region Variables

    public Transform cameraTransform;
    [Range(0f, 1f)] public float parallaxFactor = 0.3f;

    [Header("Scaling")]
    public float scaleFactor = 0.05f;

    private Vector3 _initialPosition;
    private Vector3 _cameraStart;

    #endregion

    #region Unity Methods

    private void Start()
    {
        _initialPosition = transform.position;
        _cameraStart = cameraTransform.position;
    }

    private void LateUpdate()
    {
        Vector3 delta = cameraTransform.position - _cameraStart;
        transform.position = new Vector3(
            _initialPosition.x + delta.x * parallaxFactor,
            _initialPosition.y + delta.y * parallaxFactor,
            _initialPosition.z + delta.y * parallaxFactor
        );
    }

    #endregion
}
