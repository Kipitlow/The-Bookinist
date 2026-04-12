using System.Collections.Generic;
using UnityEngine;

public class PageManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _layerHolder;

    [SerializeField] string _sortingLayerPrefix = "Page_";

    public static int layerSpread = 20;
    public static int maxLayer;

    [SerializeField] private int _activePageIndex = 0;

    private void Awake()
    {
        maxLayer = _layerHolder.Count;
    }

    private void Start()
    {
        for (int i = 0; i < _layerHolder.Count; i++)
        {
            GameObject layer = _layerHolder[i];

            // Position Z inchangée (gère le z-fighting en 3D)
            layer.transform.position = new Vector3(0, 0, i * layerSpread);

            // Assigne le Sorting Layer à tous les SpriteRenderer du layer
            string sortingLayerName = _sortingLayerPrefix + i;
            Page page = layer.GetComponent<Page>();
            if (page != null)
            {
                page.SetSortingLayer(sortingLayerName);
            }
                
                
            else
                Debug.LogWarning($"[PageManager] Pas de composant Page sur {layer.name}.", layer);
        }
    }

    public Page GetActivePage()
    {
        if (_activePageIndex < 0 || _activePageIndex >= _layerHolder.Count)
            return null;
        return _layerHolder[_activePageIndex].GetComponent<Page>();
    }

    public void SetActivePage(int index)
    {
        _activePageIndex = Mathf.Clamp(index, 0, _layerHolder.Count - 1);
    }
}