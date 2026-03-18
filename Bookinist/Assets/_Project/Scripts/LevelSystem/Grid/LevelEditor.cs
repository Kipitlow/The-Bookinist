using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class LevelEditor : MonoBehaviour
{
    [Header("Layers (ordered front → back, matching your Page setup)")]
    [SerializeField] private List<LayerGrid> _layers = new();

    [Header("Prefab Palette")]
    [SerializeField] private List<GameObject> _palette = new();

    [Header("Editor State")]
    [SerializeField] private int _activeLayerIndex = 0;
    [SerializeField] private int _selectedPaletteIndex = 0;

    [Header("Ghost Preview")]
    [SerializeField] private float _ghostAlpha = 0.5f;

    // Nom du Sorting Layer dédié au ghost - doit exister dans Project Settings
    // et être placé tout en haut de la liste (= rendu en premier plan absolu)
    private const string GhostSortingLayer = "Editor_Ghost";

    // ── Runtime state ─────────────────────────────────────────
    private GameObject _ghost;       // preview object following the mouse
    private Vector2Int? _ghostCell;   // current snapped cell (null = outside grid)
    private Camera _cam;

    // ── Cached input actions ──────────────────────────────────
    private InputAction _placementAction;
    private InputAction _eraseAction;
    private InputAction _scrollAction;
    private InputAction _nextPrefabAction;
    private InputAction _prevPrefabAction;
    private InputAction _mousePositionAction;

    // ── Properties ────────────────────────────────────────────
    public LayerGrid ActiveLayer =>
        (_layers != null && _activeLayerIndex >= 0 && _activeLayerIndex < _layers.Count)
            ? _layers[_activeLayerIndex]
            : null;

    public GameObject SelectedPrefab =>
        (_palette != null && _selectedPaletteIndex >= 0 && _selectedPaletteIndex < _palette.Count)
            ? _palette[_selectedPaletteIndex]
            : null;

    public int LayerCount => _layers.Count;
    public int ActiveLayerIndex => _activeLayerIndex;
    public int PaletteCount => _palette.Count;
    public int SelectedPaletteIndex => _selectedPaletteIndex;
    public List<LayerGrid> Layers => _layers;

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
        EnableInputActions();
        SpawnGhost();
    }

    private void OnDisable()
    {
        DisableInputActions();
        DestroyGhost();
    }

    private void OnDestroy()
    {
        DisposeInputActions();
    }

    private void Update()
    {
        // Block all editor input while the mouse is over a UI element
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            SetGhostVisible(false);
            return;
        }

        UpdateGhostPosition();
        HandleLayerSwitch();
    }

    // ──────────────────────────────────────────────────────────
    //  Input Actions – setup
    // ──────────────────────────────────────────────────────────

    private void CreateInputActions()
    {
        // Left click → place
        _placementAction = new InputAction("Place", InputActionType.Button);
        _placementAction.AddBinding("<Mouse>/leftButton");

        // Right click → erase
        _eraseAction = new InputAction("Erase", InputActionType.Button);
        _eraseAction.AddBinding("<Mouse>/rightButton");

        // Scroll wheel → change active layer
        _scrollAction = new InputAction("Scroll", InputActionType.Value,
            expectedControlType: "Vector2");
        _scrollAction.AddBinding("<Mouse>/scroll");

        // E → next prefab in palette
        _nextPrefabAction = new InputAction("NextPrefab", InputActionType.Button);
        _nextPrefabAction.AddBinding("<Keyboard>/e");

        // Q → previous prefab in palette
        _prevPrefabAction = new InputAction("PrevPrefab", InputActionType.Button);
        _prevPrefabAction.AddBinding("<Keyboard>/q");

        // Mouse position (read every frame in UpdateGhostPosition)
        _mousePositionAction = new InputAction("MousePosition", InputActionType.Value,
            expectedControlType: "Vector2");
        _mousePositionAction.AddBinding("<Mouse>/position");

        // Number keys 1-9 for direct layer selection are handled in Update via Keyboard
    }

    private void EnableInputActions()
    {
        _placementAction.performed += OnPlace;
        _eraseAction.performed += OnErase;
        _nextPrefabAction.performed += OnNextPrefab;
        _prevPrefabAction.performed += OnPrevPrefab;

        _placementAction.Enable();
        _eraseAction.Enable();
        _scrollAction.Enable();
        _nextPrefabAction.Enable();
        _prevPrefabAction.Enable();
        _mousePositionAction.Enable();
    }

    private void DisableInputActions()
    {
        _placementAction.performed -= OnPlace;
        _eraseAction.performed -= OnErase;
        _nextPrefabAction.performed -= OnNextPrefab;
        _prevPrefabAction.performed -= OnPrevPrefab;

        _placementAction.Disable();
        _eraseAction.Disable();
        _scrollAction.Disable();
        _nextPrefabAction.Disable();
        _prevPrefabAction.Disable();
        _mousePositionAction.Disable();
    }

    private void DisposeInputActions()
    {
        _placementAction?.Dispose();
        _eraseAction?.Dispose();
        _scrollAction?.Dispose();
        _nextPrefabAction?.Dispose();
        _prevPrefabAction?.Dispose();
        _mousePositionAction?.Dispose();
    }

    // ──────────────────────────────────────────────────────────
    //  Input callbacks
    // ──────────────────────────────────────────────────────────

    private void OnPlace(InputAction.CallbackContext ctx)
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;
        if (_ghostCell == null || ActiveLayer == null || SelectedPrefab == null) return;

        ActiveLayer.PlaceObject(SelectedPrefab, _ghostCell.Value);
    }

    private void OnErase(InputAction.CallbackContext ctx)
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;
        if (_ghostCell == null || ActiveLayer == null) return;

        ActiveLayer.RemoveObject(_ghostCell.Value);
    }

    private void OnNextPrefab(InputAction.CallbackContext ctx)
        => SetSelectedPrefab((_selectedPaletteIndex + 1) % Mathf.Max(1, _palette.Count));

    private void OnPrevPrefab(InputAction.CallbackContext ctx)
        => SetSelectedPrefab((_selectedPaletteIndex - 1 + _palette.Count) % Mathf.Max(1, _palette.Count));

    // ──────────────────────────────────────────────────────────
    //  Per-frame input (scroll + number keys)
    // ──────────────────────────────────────────────────────────

    private void HandleLayerSwitch()
    {
        // Scroll wheel: y axis, positive = scroll up = move to closer (lower index) layer
        Vector2 scroll = _scrollAction.ReadValue<Vector2>();
        if (Mathf.Abs(scroll.y) > 0.01f)
        {
            int delta = scroll.y > 0 ? -1 : 1;
            SetActiveLayer(_activeLayerIndex + delta);
        }

        // Number keys 1-9 for direct layer selection
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        Key[] digitKeys =
        {
            Key.Digit1, Key.Digit2, Key.Digit3, Key.Digit4, Key.Digit5,
            Key.Digit6, Key.Digit7, Key.Digit8, Key.Digit9
        };

        for (int i = 0; i < Mathf.Min(_layers.Count, digitKeys.Length); i++)
        {
            if (keyboard[digitKeys[i]].wasPressedThisFrame)
            {
                SetActiveLayer(i);
                break;
            }
        }
    }

    // ──────────────────────────────────────────────────────────
    //  Ghost (preview) management
    // ──────────────────────────────────────────────────────────

    private void SpawnGhost()
    {
        DestroyGhost();
        if (SelectedPrefab == null) return;

        // Vérifie que le Sorting Layer dédié existe
        bool layerExists = false;
        foreach (var sl in SortingLayer.layers)
            if (sl.name == GhostSortingLayer) { layerExists = true; break; }
        if (!layerExists)
            Debug.LogError($"[LevelEditor] Le Sorting Layer " + GhostSortingLayer +
        " n'existe pas. " +
                           "Crée-le dans Edit → Project Settings → Tags and Layers " +
                           "et place-le tout en haut de la liste.", this);

        _ghost = Instantiate(SelectedPrefab);
        SetupGhostRenderers(_ghost);

        // Disable all colliders so the ghost doesn't interact with the scene
        foreach (var col in _ghost.GetComponentsInChildren<Collider>())
            col.enabled = false;
        foreach (var col in _ghost.GetComponentsInChildren<Collider2D>())
            col.enabled = false;
    }

    private void DestroyGhost()
    {
        if (_ghost != null)
        {
            Destroy(_ghost);
            _ghost = null;
        }
        _ghostCell = null;
    }

    private void UpdateGhostPosition()
    {
        if (_ghost == null || ActiveLayer == null)
        {
            SetGhostVisible(false);
            return;
        }

        Vector3? worldPos = RaycastOntoLayerPlane(ActiveLayer);
        if (worldPos == null)
        {
            SetGhostVisible(false);
            _ghostCell = null;
            return;
        }

        Vector3? snapped = ActiveLayer.SnapToGrid(worldPos.Value);
        _ghostCell = ActiveLayer.WorldToCell(worldPos.Value);

        if (snapped == null)
        {
            SetGhostVisible(false);
            _ghostCell = null;
            return;
        }

        _ghost.transform.position = snapped.Value;

        // Tint ghost red if cell is occupied, normal semi-transparent otherwise
        bool cellFree = _ghostCell.HasValue && ActiveLayer.IsCellFree(_ghostCell.Value);
        SetGhostColor(cellFree ? new Color(1f, 1f, 1f, _ghostAlpha)
                               : new Color(1f, 0.2f, 0.2f, _ghostAlpha));
        SetGhostVisible(true);
    }

    private void SetGhostVisible(bool visible)
    {
        if (_ghost != null) _ghost.SetActive(visible);
    }

    private void SetGhostColor(Color color)
    {
        foreach (var sr in _ghost.GetComponentsInChildren<SpriteRenderer>())
            sr.color = color;
        foreach (var mr in _ghost.GetComponentsInChildren<MeshRenderer>())
            foreach (var mat in mr.materials)
                if (mat.HasProperty("_Color")) mat.color = color;
    }

    private void SetupGhostRenderers(GameObject ghost)
    {
        foreach (var sr in ghost.GetComponentsInChildren<SpriteRenderer>())
        {
            // Transparence
            Color c = sr.color;
            c.a = _ghostAlpha;
            sr.color = c;

            // Toujours au premier plan
            sr.sortingLayerName = GhostSortingLayer;
            sr.sortingOrder = 0;
        }
    }

    // ──────────────────────────────────────────────────────────
    //  Raycasting onto a layer's XY plane
    // ──────────────────────────────────────────────────────────

    /// <summary>
    /// Intersects the mouse ray with the XY plane of the given layer.
    /// Uses the new Input System to read mouse position.
    /// Works regardless of camera angle (perspective or orthographic).
    /// </summary>
    private Vector3? RaycastOntoLayerPlane(LayerGrid layer)
    {
        Vector2 mouseScreenPos = _mousePositionAction.ReadValue<Vector2>();
        Ray ray = _cam.ScreenPointToRay(mouseScreenPos);

        // The layer's plane: normal = layer's forward (≈ world Z for a standard layer),
        // point  = layer's world position.
        Vector3 planeNormal = layer.transform.forward;
        Vector3 planePoint = layer.transform.position;

        float denom = Vector3.Dot(planeNormal, ray.direction);

        // Ray nearly parallel to the plane → no intersection
        if (Mathf.Abs(denom) < 1e-6f) return null;

        float t = Vector3.Dot(planePoint - ray.origin, planeNormal) / denom;
        if (t < 0f) return null; // plane is behind the camera

        return ray.origin + ray.direction * t;
    }

    // ──────────────────────────────────────────────────────────
    //  Public API (callable from UI buttons, etc.)
    // ──────────────────────────────────────────────────────────

    public void SetActiveLayer(int index)
    {
        _activeLayerIndex = Mathf.Clamp(index, 0, Mathf.Max(0, _layers.Count - 1));
        SpawnGhost(); // re-spawn ghost when layer changes (Z depth changes)
    }

    public void SetSelectedPrefab(int paletteIndex)
    {
        _selectedPaletteIndex = Mathf.Clamp(paletteIndex, 0, Mathf.Max(0, _palette.Count - 1));
        SpawnGhost();
    }

    public void SetSelectedPrefab(GameObject prefab)
    {
        int idx = _palette.IndexOf(prefab);
        if (idx >= 0) SetSelectedPrefab(idx);
    }

    /// <summary>Add a layer at runtime (e.g. when a new Page is created).</summary>
    public void RegisterLayer(LayerGrid layer)
    {
        if (!_layers.Contains(layer))
            _layers.Add(layer);
    }

    public void UnregisterLayer(LayerGrid layer)
    {
        _layers.Remove(layer);
        _activeLayerIndex = Mathf.Clamp(_activeLayerIndex, 0, Mathf.Max(0, _layers.Count - 1));
    }

    // ──────────────────────────────────────────────────────────
    //  Debug Gizmos
    // ──────────────────────────────────────────────────────────

    private void OnDrawGizmos()
    {
        if (ActiveLayer == null) return;

        // Highlight the active layer's plane border in cyan
        Gizmos.color = Color.cyan;
        Gizmos.matrix = ActiveLayer.transform.localToWorldMatrix;
        float w = ActiveLayer.Columns * ActiveLayer.CellWidth;
        float h = ActiveLayer.Rows * ActiveLayer.CellHeight;
        Gizmos.DrawWireCube(new Vector3(w * 0.5f, h * 0.5f, 0f), new Vector3(w, h, 0.02f));
    }
}