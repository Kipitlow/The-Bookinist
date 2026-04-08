using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject contenant la definition d'un level/page (objets placés, métadonnées).
/// </summary>
[CreateAssetMenu(fileName = "LevelData_Page", menuName = "Level Editor/Level Data")]
public class LevelData : ScriptableObject
{
    [Serializable]
    public class PlacedEntry
    {
        public Vector2Int cell;
        public GameObject prefab;
        public int manualSortingOffset;
    }

    [Header("Métadonnées")]
    public string layerName;
    public int pageIndex;

    [Header("Objets placés")]
    public List<PlacedEntry> entries = new();
}