using UnityEngine;

public class PlacedObject : MonoBehaviour
{
    // ── Données de placement ──────────────────────────────────
    public Vector2Int Cell { get; private set; }
    public LayerGrid OwnerGrid { get; private set; }

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

    public void Init(Vector2Int cell, LayerGrid ownerGrid)
    {
        Cell = cell;
        OwnerGrid = ownerGrid;
        ApplySorting();
    }

    // ──────────────────────────────────────────────────────────
    //  Calcul et application
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