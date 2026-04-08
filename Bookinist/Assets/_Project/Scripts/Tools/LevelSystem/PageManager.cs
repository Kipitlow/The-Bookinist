using System.Collections.Generic;
using UnityEngine;

public class PageManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _layerHolder;
    [SerializeField] private string _sortingLayerPrefix = "Page_";

    public static int layerSpread = 20;
    public static int maxLayer;

    private void Awake()
    {
        maxLayer = _layerHolder != null ? _layerHolder.Count : 0;
    }

    private void Start()
    {
        for (int i = 0; i < _layerHolder.Count; i++)
        {
            GameObject layer = _layerHolder[i];
            layer.transform.position = new Vector3(0, 0, i * layerSpread);

            string sortingLayerName = _sortingLayerPrefix + i;
            if (layer.TryGetComponent<Page>(out var page))
            {
                page.SetSortingLayer(sortingLayerName);
            }
            else
            {
                Debug.LogWarning($"[PageManager] Pas de composant Page sur {layer.name}.", layer);
            }
        }
    }
}