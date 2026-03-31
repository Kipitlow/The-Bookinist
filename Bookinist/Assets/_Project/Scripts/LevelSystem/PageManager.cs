using System.Collections.Generic;
using UnityEngine;

public class PageManager : MonoBehaviour
{
    [SerializeField] List<GameObject> layerHolder;

    [SerializeField] string _sortingLayerPrefix = "Page_";

    public static int layerSpread = 20;
    public static int maxLayer;

    private void Awake()
    {
        maxLayer = layerHolder.Count;
    }

    private void Start()
    {
        for (int i = 0; i < layerHolder.Count; i++)
        {
            GameObject layer = layerHolder[i];

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
}