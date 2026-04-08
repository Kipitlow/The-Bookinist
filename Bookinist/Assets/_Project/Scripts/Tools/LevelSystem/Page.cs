using System.Collections.Generic;
using UnityEngine;

public class Page : MonoBehaviour
{
    #region Variables

    [SerializeField] private List<GameObject> _pageObjects;
    [SerializeField] private int _pageIndex;
    private SnapPointManager _snapPointManager;

    private string _sortingLayerName;
    public List<GameObject> PageObjects => _pageObjects;
    public int PageIndex => _pageIndex;
    public string SortingLayerName => _sortingLayerName;

    #endregion

    #region Unity Methods

    private void Start()
    {
        if (string.IsNullOrEmpty(_sortingLayerName))
            _sortingLayerName = "Page_" + _pageIndex;

        ApplySortingToPageObjects();
        _snapPointManager = GetComponentInChildren<SnapPointManager>();
        if (_snapPointManager != null) _snapPointManager.SetUpSnapPoints();
    }

    #endregion

    #region Methods

    /// <summary>
    /// Appelé par PageManager pour assigner le Sorting Layer de cette page.
    /// </summary>
    public void SetSortingLayer(string sortingLayerName)
    {
        _sortingLayerName = sortingLayerName;
        ApplySortingToPageObjects();
    }

    private void ApplySortingToPageObjects()
    {
        if (string.IsNullOrEmpty(_sortingLayerName)) return;

        for (int i = 0; i < _pageObjects.Count; i++)
        {
            if (_pageObjects[i] == null) continue;
            if (!_pageObjects[i].TryGetComponent<SpriteRenderer>(out var sr)) continue;
            sr.sortingLayerName = _sortingLayerName;
            sr.sortingOrder = i;
        }
    }

    private static bool SortingLayerExists(string layerName)
    {
        foreach (var layer in SortingLayer.layers)
            if (layer.name == layerName) return true;
        return false;
    }

    #endregion
}