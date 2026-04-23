using System.Collections.Generic;
using UnityEngine;

public class PageManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _layerHolder;

    [SerializeField] private string _sortingLayerPrefix = "Page_";

    [SerializeField] private List<int> _layerSpread;
    public static int maxLayer;

    [SerializeField] private int _activePageIndex = 0;

    public List<int> LayerSpread => _layerSpread;
    public List<GameObject> LayerHolder => _layerHolder;

    private static PageManager _instance;

    public static PageManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<PageManager>();

                if (_instance == null)
                {
                    GameObject obj = new GameObject(nameof(PageManager));
                    _instance = obj.AddComponent<PageManager>();
                }
            }
            return _instance;
        }
    }

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
            layer.transform.position = new Vector3(0, layer.transform.localPosition.y, _layerSpread[i]);

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

    public GameObject GetPageFromInt(int layer)
    {
        return _layerHolder[layer];
    }

    public void SetActivePage(int index)
    {
        _activePageIndex = Mathf.Clamp(index, 0, _layerHolder.Count - 1);
    }
}