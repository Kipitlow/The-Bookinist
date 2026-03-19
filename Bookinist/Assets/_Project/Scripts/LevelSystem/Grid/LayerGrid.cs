using System.Collections.Generic;
using UnityEngine;

public class LayerGrid : MonoBehaviour
{
    [Header("Grid Dimensions")]
    [SerializeField] private int _columns = 10;
    [SerializeField] private int _rows = 6;

    [Header("Cell Size (world units)")]
    [SerializeField] private float _cellWidth = 1f;
    [SerializeField] private float _cellHeight = 1f;

    [Header("Grid Origin (bottom-left corner, local space)")]
    [SerializeField] private Vector2 _gridOrigin = Vector2.zero;

    [Header("Visuals")]
    [SerializeField] private bool _showGridGizmos = true;
    [SerializeField] private Color _gridColor = new Color(1f, 1f, 0f, 0.3f);
    [SerializeField] private Color _occupiedColor = new Color(1f, 0.3f, 0.3f, 0.5f);

    [Header("Sauvegarde")]
    [Tooltip("LevelData ScriptableObject correspondant à ce layer. " +
             "Assigné automatiquement à la sauvegarde, ou manuellement dans l'Inspector.")]
    [SerializeField] private LevelData _levelData;

    // Maps grid coordinate → placed GameObject
    private Dictionary<Vector2Int, GameObject> _placedObjects = new();

    // ──────────────────────────────────────────────────────────
    //  Unity lifecycle
    // ──────────────────────────────────────────────────────────

    private void Start()
    {
        if (_levelData != null)
        {
            Debug.Log($"[LayerGrid] Chargement automatique de {_levelData.name} " +
                      $"({_levelData.entries.Count} entrées).");
            LoadFromData(_levelData);
        }
        else
        {
            Debug.LogWarning($"[LayerGrid] Aucun LevelData assigné sur {gameObject.name} " +
                             "- rien à charger.", this);
        }
    }

    // ──────────────────────────────────────────────────────────
    //  Public API
    // ──────────────────────────────────────────────────────────

    public int Columns => _columns;
    public int Rows => _rows;
    public float CellWidth => _cellWidth;
    public float CellHeight => _cellHeight;

    /// <summary>
    /// Converts a world position to the nearest grid cell on this layer.
    /// Returns null if the position is outside the grid.
    /// </summary>
    public Vector2Int? WorldToCell(Vector3 worldPos)
    {
        // Convert to local space of this grid's transform
        Vector3 local = transform.InverseTransformPoint(worldPos);

        float fx = (local.x - _gridOrigin.x) / _cellWidth;
        float fy = (local.y - _gridOrigin.y) / _cellHeight;

        int col = Mathf.FloorToInt(fx);
        int row = Mathf.FloorToInt(fy);

        if (col < 0 || col >= _columns || row < 0 || row >= _rows)
            return null;

        return new Vector2Int(col, row);
    }

    /// <summary>
    /// Converts a grid cell to the world-space center of that cell (at this layer's Z).
    /// </summary>
    public Vector3 CellToWorld(Vector2Int cell)
    {
        float lx = _gridOrigin.x + (cell.x + 0.5f) * _cellWidth;
        float ly = _gridOrigin.y + (cell.y + 0.5f) * _cellHeight;

        // Z stays at 0 in local space → the transform's world Z is the layer's depth
        Vector3 localCenter = new Vector3(lx, ly, 0f);
        return transform.TransformPoint(localCenter);
    }

    /// <summary>
    /// Snaps a world position to the center of the nearest valid cell.
    /// Returns null if outside the grid.
    /// </summary>
    public Vector3? SnapToGrid(Vector3 worldPos)
    {
        Vector2Int? cell = WorldToCell(worldPos);
        if (cell == null) return null;
        return CellToWorld(cell.Value);
    }

    /// <summary>Returns true if the cell is within bounds and not already occupied.</summary>
    public bool IsCellFree(Vector2Int cell)
    {
        if (cell.x < 0 || cell.x >= _columns || cell.y < 0 || cell.y >= _rows)
            return false;
        return !_placedObjects.ContainsKey(cell);
    }

    /// <summary>
    /// Place a prefab instance on the grid at the given cell.
    /// Returns the instantiated object, or null if the cell is occupied / out of bounds.
    /// </summary>
    public GameObject PlaceObject(GameObject prefab, Vector2Int cell)
    {
        if (!IsCellFree(cell)) return null;

        Vector3 pos = CellToWorld(cell);
        GameObject obj = Instantiate(prefab, pos, Quaternion.identity, transform);

        // Attache et initialise PlacedObject pour gérer le sortingOrder automatiquement.
        // On passe le prefab source ici pendant qu'on y a encore accès directement.
        PlacedObject po = obj.GetComponent<PlacedObject>();
        if (po == null) po = obj.AddComponent<PlacedObject>();
        po.Init(cell, this, prefab);

        _placedObjects[cell] = obj;
        return obj;
    }

    /// <summary>Remove and destroy the object at the given cell.</summary>
    public bool RemoveObject(Vector2Int cell)
    {
        if (!_placedObjects.TryGetValue(cell, out GameObject obj)) return false;
        _placedObjects.Remove(cell);
        if (obj != null) Destroy(obj);
        return true;
    }

    /// <summary>Returns the object occupying a cell, or null.</summary>
    public GameObject GetObjectAt(Vector2Int cell)
    {
        _placedObjects.TryGetValue(cell, out GameObject obj);
        return obj;
    }

    /// <summary>
    /// Charge un LevelData dans cette grille - efface l'état actuel et recrée les objets.
    /// Appelé automatiquement au Start si un LevelData est assigné.
    /// </summary>
    public void LoadFromData(LevelData data)
    {
        if (data == null) return;

        ClearAll();
        _levelData = data;

        foreach (var entry in data.entries)
        {
            if (entry.prefab == null)
            {
                Debug.LogWarning($"[LayerGrid] Prefab manquant pour la cellule {entry.cell} " +
                                 $"dans {data.name}.");
                continue;
            }

            GameObject placed = PlaceObject(entry.prefab, entry.cell);
            if (placed == null) continue;

            PlacedObject po = placed.GetComponent<PlacedObject>();
            if (po != null) po.ManualSortingOffset = entry.manualSortingOffset;
        }

        Debug.Log($"[LayerGrid] {data.entries.Count} objet(s) chargé(s) depuis {data.name}.");
    }

    /// <summary>
    /// Assigne le LevelData de ce layer (appelé par LevelEditorWindow après sauvegarde).
    /// </summary>
    public void SetLevelData(LevelData data) => _levelData = data;

    public LevelData GetLevelData() => _levelData;

    /// <summary>
    /// Retourne une copie en lecture seule du dictionnaire des objets placés.
    /// Utilisé par LevelEditorWindow pour sérialiser l'état de la grille.
    /// </summary>
    public IReadOnlyDictionary<Vector2Int, GameObject> GetPlacedObjects()
        => _placedObjects;

    /// <summary>Remove all placed objects on this layer.</summary>
    public void ClearAll()
    {
        foreach (var kvp in _placedObjects)
            if (kvp.Value != null) Destroy(kvp.Value);
        _placedObjects.Clear();
    }

    // ──────────────────────────────────────────────────────────
    //  Editor Gizmos
    // ──────────────────────────────────────────────────────────

    private void OnDrawGizmos()
    {
        if (!_showGridGizmos) return;

        Gizmos.matrix = transform.localToWorldMatrix;

        // Draw occupied cells first
        Gizmos.color = _occupiedColor;
        foreach (var kvp in _placedObjects)
        {
            float lx = _gridOrigin.x + kvp.Key.x * _cellWidth;
            float ly = _gridOrigin.y + kvp.Key.y * _cellHeight;
            Gizmos.DrawCube(
                new Vector3(lx + _cellWidth * 0.5f, ly + _cellHeight * 0.5f, 0f),
                new Vector3(_cellWidth * 0.9f, _cellHeight * 0.9f, 0.05f));
        }

        // Draw grid lines
        Gizmos.color = _gridColor;
        float totalW = _columns * _cellWidth;
        float totalH = _rows * _cellHeight;

        for (int col = 0; col <= _columns; col++)
        {
            float x = _gridOrigin.x + col * _cellWidth;
            Gizmos.DrawLine(
                new Vector3(x, _gridOrigin.y, 0f),
                new Vector3(x, _gridOrigin.y + totalH, 0f));
        }
        for (int row = 0; row <= _rows; row++)
        {
            float y = _gridOrigin.y + row * _cellHeight;
            Gizmos.DrawLine(
                new Vector3(_gridOrigin.x, y, 0f),
                new Vector3(_gridOrigin.x + totalW, y, 0f));
        }
    }
}