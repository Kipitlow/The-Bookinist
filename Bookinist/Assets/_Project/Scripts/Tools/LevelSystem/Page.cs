using System.Collections.Generic;
using UnityEngine;

public class Page : MonoBehaviour
{
    [SerializeField] List<GameObject> pageObjects;
    [SerializeField] int pageIndex;
    private SnapPointManager _snapPointManager;

    public List<GameObject> PageObjects => pageObjects;
    public int PageIndex => pageIndex;

    // Nom du Sorting Layer assigné à cette page (défini par PageManager au Start)
    private string _sortingLayerName;
    public string SortingLayerName => _sortingLayerName;

    private void Start()
    {
        // Si PageManager n'a pas encore appelé SetSortingLayer (ordre d'exécution),
        // on tente de le déduire depuis pageIndex comme fallback
        if (string.IsNullOrEmpty(_sortingLayerName))
            _sortingLayerName = "Page_" + pageIndex;

        ApplySortingToPageObjects();
        _snapPointManager = GetComponentInChildren<SnapPointManager>();
        if (_snapPointManager != null ) _snapPointManager.SetUpSnapPoints();
    }

    /// <summary>
    /// Appelé par PageManager pour assigner le Sorting Layer de cette page.
    /// Doit être appelé avant Start si possible (depuis Awake de PageManager).
    /// </summary>
    public void SetSortingLayer(string sortingLayerName)
    {
        _sortingLayerName = sortingLayerName;
        ApplySortingToPageObjects();
    }

    /// <summary>
    /// Applique le Sorting Layer et un sortingOrder séquentiel (0, 1, 2...)
    /// à tous les objets pré-placés dans la liste pageObjects.
    /// Les objets placés dynamiquement par LevelEditor sont gérés par PlacedObject.
    /// </summary>
    private void ApplySortingToPageObjects()
    {
        if (string.IsNullOrEmpty(_sortingLayerName)) return;

        // Vérifie que le Sorting Layer existe bien dans le projet
        /*if (!SortingLayerExists(_sortingLayerName) && _sortingLayerName!=null)                   <--------------------------------------------------
        {
            Debug.LogError($"[Page] Le Sorting Layer \"{_sortingLayerName}\" n'existe pas. " +
                           $"Crée-le dans Edit → Project Settings → Tags and Layers.", this);
            return;
        }*/

        for (int i = 0; i < pageObjects.Count; i++)
        {
            if (pageObjects[i] == null) continue;

            SpriteRenderer sr = pageObjects[i].GetComponent<SpriteRenderer>();
            if (sr == null) continue;

            sr.sortingLayerName = _sortingLayerName;
            //print("Sorting object in : " + _sortingLayerName);
            sr.sortingOrder = i; // 0, 1, 2... sans limite liée aux autres pages
            pageObjects[i].layer = 3;
        }
    }

    /// <summary>
    /// Vérifie qu'un Sorting Layer avec ce nom existe dans les settings du projet.
    /// </summary>
    private static bool SortingLayerExists(string layerName)
    {
        foreach (var layer in SortingLayer.layers)
            if (layer.name == layerName) return true;
        return false;
    }
}