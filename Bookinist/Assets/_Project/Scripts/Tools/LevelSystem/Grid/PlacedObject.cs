using UnityEngine;

public class PlacedObject : MonoBehaviour
{
    // ── Données de placement ──────────────────────────────────
    public Vector2Int Cell { get; private set; }
    public LayerGrid OwnerGrid { get; private set; }

    /// <summary>
    /// Référence au prefab source stockée au moment du placement.
    /// Nécessaire pour la sauvegarde car PrefabUtility ne fonctionne pas en Play Mode.
    /// </summary>
    public GameObject SourcePrefab { get; private set; }

    // ── Contrôle manuel ───────────────────────────────────────
    [Header("Sorting Override")]
    [Tooltip("Décale le sortingOrder calculé automatiquement. " +
             "Positif = devant, Négatif = derrière.")]
    [SerializeField] private int _manualSortingOffset = 0;

    public int ManualSortingOffset
    {
        get => _manualSortingOffset;
        set { _manualSortingOffset = value; ApplySorting(); }
    }

    // ──────────────────────────────────────────────────────────
    //  Init
    // ──────────────────────────────────────────────────────────

    public void Init(Vector2Int cell, LayerGrid ownerGrid, GameObject sourcePrefab)
    {
        Cell = cell;
        OwnerGrid = ownerGrid;
        SourcePrefab = sourcePrefab;
        ApplySorting();
    }

    // ──────────────────────────────────────────────────────────
    //  Sorting
    // ──────────────────────────────────────────────────────────

    public void ApplySorting()
    {
        if (OwnerGrid == null) return;

        Page page = OwnerGrid.GetComponent<Page>();
        if (page == null)
        {
            Debug.LogWarning("[PlacedObject] Pas de composant Page sur le LayerGrid parent.", this);
            return;
        }

        string sortingLayer = page.SortingLayerName;
        int order = (OwnerGrid.Rows - Cell.y) + _manualSortingOffset;

        foreach (var sr in GetComponentsInChildren<SpriteRenderer>(includeInactive: true))
        {
            sr.sortingLayerName = sortingLayer;
            sr.sortingOrder = order;
        }
    }

    private void OnValidate()
    {
        if (Application.isPlaying) ApplySorting();
    }
}