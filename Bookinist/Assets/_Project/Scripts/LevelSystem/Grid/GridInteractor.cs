using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GridInteractor : MonoBehaviour
{
    [Header("Layer cible")]
    [SerializeField] private LayerGrid _targetLayer;

    [Header("Feedback visuel (test)")]
    [SerializeField] private Color _flashColor = new Color(1f, 0.8f, 0f);
    [SerializeField] private float _flashDuration = 0.2f;

    // Événement auquel d'autres scripts peuvent s'abonner
    public event System.Action<PlacedObject, Vector2Int> OnObjectTouched;

    private Camera _cam;
    private InputAction _touchAction;
    private InputAction _touchPositionAction;

    // ──────────────────────────────────────────────────────────
    //  Unity lifecycle
    // ──────────────────────────────────────────────────────────

    private void Awake()
    {
        _cam = Camera.main;
        CreateInputActions();
    }

    private void OnEnable()
    {
        _touchAction.performed += OnTouch;
        _touchAction.Enable();
        _touchPositionAction.Enable();
    }

    private void OnDisable()
    {
        _touchAction.performed -= OnTouch;
        _touchAction.Disable();
        _touchPositionAction.Disable();
    }

    private void OnDestroy()
    {
        _touchAction?.Dispose();
        _touchPositionAction?.Dispose();
    }

    // ──────────────────────────────────────────────────────────
    //  Input
    // ──────────────────────────────────────────────────────────

    private void CreateInputActions()
    {
        // Fonctionne aussi bien avec un vrai touch qu'avec le simulateur
        // de touch d'Unity (et le clic souris en Editor)
        _touchAction = new InputAction("Touch", InputActionType.Button);
        _touchAction.AddBinding("<Touchscreen>/primaryTouch/press");
        _touchAction.AddBinding("<Mouse>/leftButton"); // fallback Editor

        _touchPositionAction = new InputAction("TouchPosition",
            InputActionType.Value, expectedControlType: "Vector2");
        _touchPositionAction.AddBinding("<Touchscreen>/primaryTouch/position");
        _touchPositionAction.AddBinding("<Mouse>/position"); // fallback Editor
    }

    private void OnTouch(InputAction.CallbackContext ctx)
    {
        Debug.Log("[GridInteractor] Touch détecté !");

        // Ignore si le touch est sur un élément UI
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("[GridInteractor] Bloqué par un élément UI.");
            return;
        }

        if (_targetLayer == null)
        {
            Debug.LogWarning("[GridInteractor] Aucun LayerGrid assigné.", this);
            return;
        }

        Vector2 screenPos = _touchPositionAction.ReadValue<Vector2>();
        Debug.Log($"[GridInteractor] Position écran : {screenPos}");

        Vector3? worldPos = RaycastOntoLayerPlane(_targetLayer, screenPos);
        if (worldPos == null)
        {
            Debug.Log("[GridInteractor] Pas d'intersection avec le plan du layer.");
            return;
        }
        Debug.Log($"[GridInteractor] Point monde : {worldPos.Value}");

        Vector2Int? cell = _targetLayer.WorldToCell(worldPos.Value);
        if (cell == null)
        {
            Debug.Log("[GridInteractor] Point hors de la grille.");
            return;
        }
        Debug.Log($"[GridInteractor] Cellule : {cell.Value}");

        GameObject obj = _targetLayer.GetObjectAt(cell.Value);
        if (obj == null)
        {
            Debug.Log($"[GridInteractor] Cellule {cell.Value} vide.");
            return;
        }
        Debug.Log($"[GridInteractor] Objet trouvé : {obj.name}");

        PlacedObject placed = obj.GetComponent<PlacedObject>();
        if (placed == null)
        {
            Debug.Log("[GridInteractor] L'objet n'a pas de composant PlacedObject.");
            return;
        }

        // Feedback visuel
        // Debugging et test rapide — à désactiver
        StartCoroutine(FlashObject(obj));

        // Notifie tous les abonnés
        OnObjectTouched?.Invoke(placed, cell.Value);

        Debug.Log($"[GridInteractor] Objet touché : {obj.name} en cellule {cell.Value}");
    }

    // ──────────────────────────────────────────────────────────
    //  Intersection rayon / plan
    // ──────────────────────────────────────────────────────────

    /// <summary>
    /// Intersecte un rayon depuis screenPos avec le plan XY du layer donné.
    /// Retourne le point monde correspondant, ou null si pas d'intersection.
    /// </summary>
    private Vector3? RaycastOntoLayerPlane(LayerGrid layer, Vector2 screenPos)
    {
        Ray ray = _cam.ScreenPointToRay(screenPos);

        Vector3 planeNormal = layer.transform.forward;
        Vector3 planePoint = layer.transform.position;

        float denom = Vector3.Dot(planeNormal, ray.direction);
        if (Mathf.Abs(denom) < 1e-6f) return null; // rayon parallèle au plan

        float t = Vector3.Dot(planePoint - ray.origin, planeNormal) / denom;
        if (t < 0f) return null; // plan derrière la caméra

        return ray.origin + ray.direction * t;
    }

    // ──────────────────────────────────────────────────────────
    //  Feedback visuel
    // ──────────────────────────────────────────────────────────

    private IEnumerator FlashObject(GameObject obj)
    {
        SpriteRenderer[] renderers = obj.GetComponentsInChildren<SpriteRenderer>();

        // Sauvegarde les couleurs d'origine
        Color[] originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
            originalColors[i] = renderers[i].color;

        // Flash
        foreach (var sr in renderers)
            sr.color = _flashColor;

        yield return new WaitForSeconds(_flashDuration);

        // Restaure
        for (int i = 0; i < renderers.Length; i++)
            if (renderers[i] != null)
                renderers[i].color = originalColors[i];
    }

    // ──────────────────────────────────────────────────────────
    //  API publique
    // ──────────────────────────────────────────────────────────

    /// <summary>Change le layer cible à la volée (utile si la caméra snappe sur un autre layer).</summary>
    public void SetTargetLayer(LayerGrid layer) => _targetLayer = layer;
}