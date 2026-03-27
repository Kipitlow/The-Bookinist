using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(LevelEditor))]
public class LevelEditorSaver : MonoBehaviour
{
    [Header("Dossier de sauvegarde (relatif à Assets/)")]
    [SerializeField] private string _saveFolder = "Levels/Level_01";

    [Header("Layers à sauvegarder (doit correspondre à LevelEditor)")]
    [SerializeField] private List<LayerGrid> _layers = new();

    // ──────────────────────────────────────────────────────────
    //  Sauvegarde
    // ──────────────────────────────────────────────────────────

    /// <summary>
    /// Sauvegarde chaque layer dans un ScriptableObject séparé.
    /// Crée le dossier et les assets s'ils n'existent pas encore.
    /// Met à jour les assets existants si on re-sauvegarde.
    /// </summary>
    public void SaveAll()
    {
        string fullFolder = $"Assets/{_saveFolder}";
        EnsureFolderExists(fullFolder);

        for (int i = 0; i < _layers.Count; i++)
        {
            LayerGrid grid = _layers[i];
            if (grid == null) continue;

            Page page = grid.GetComponent<Page>();
            int pageIndex = page != null ? page.PageIndex : i;

            string assetPath = $"{fullFolder}/LevelData_Page{pageIndex}.asset";
            LevelData data = AssetDatabase.LoadAssetAtPath<LevelData>(assetPath);

            // Crée l'asset s'il n'existe pas encore
            if (data == null)
            {
                data = ScriptableObject.CreateInstance<LevelData>();
                AssetDatabase.CreateAsset(data, assetPath);
            }

            // Remplit les données
            data.layerName = grid.gameObject.name;
            data.pageIndex = pageIndex;
            data.entries.Clear();

            foreach (var kvp in grid.GetPlacedObjects())
            {
                PlacedObject po = kvp.Value.GetComponent<PlacedObject>();
                if (po == null) continue;

                // On remonte au prefab source via PrefabUtility
                GameObject prefabSource = PrefabUtility.GetCorrespondingObjectFromOriginalSource(kvp.Value);
                if (prefabSource == null)
                {
                    Debug.LogWarning($"[LevelEditorSaver] L'objet en {kvp.Key} n'est pas une instance " +
                                     $"de prefab - il ne sera pas sauvegardé.", kvp.Value);
                    continue;
                }

                data.entries.Add(new LevelData.PlacedEntry
                {
                    cell = kvp.Key,
                    prefab = prefabSource,
                    manualSortingOffset = po.ManualSortingOffset
                });
            }

            EditorUtility.SetDirty(data);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"[LevelEditorSaver] Sauvegarde terminée dans Assets/{_saveFolder}");
    }

    // ──────────────────────────────────────────────────────────
    //  Chargement
    // ──────────────────────────────────────────────────────────

    /// <summary>
    /// Charge chaque layer depuis son ScriptableObject.
    /// Efface d'abord les objets déjà présents sur la grille.
    /// </summary>
    public void LoadAll()
    {
        string fullFolder = $"Assets/{_saveFolder}";

        for (int i = 0; i < _layers.Count; i++)
        {
            LayerGrid grid = _layers[i];
            if (grid == null) continue;

            Page page = grid.GetComponent<Page>();
            int pageIndex = page != null ? page.PageIndex : i;

            string assetPath = $"{fullFolder}/LevelData_Page{pageIndex}.asset";
            LevelData data = AssetDatabase.LoadAssetAtPath<LevelData>(assetPath);

            if (data == null)
            {
                Debug.LogWarning($"[LevelEditorSaver] Aucun asset trouvé à {assetPath} - layer ignoré.");
                continue;
            }

            // Efface la grille actuelle
            grid.ClearAll();

            // Replace les objets
            foreach (var entry in data.entries)
            {
                if (entry.prefab == null)
                {
                    Debug.LogWarning($"[LevelEditorSaver] Prefab manquant pour la cellule {entry.cell} " +
                                     $"sur le layer {data.layerName}.");
                    continue;
                }

                GameObject placed = grid.PlaceObject(entry.prefab, entry.cell);
                if (placed == null) continue;

                PlacedObject po = placed.GetComponent<PlacedObject>();
                if (po != null) po.ManualSortingOffset = entry.manualSortingOffset;
            }
        }

        Debug.Log($"[LevelEditorSaver] Chargement terminé depuis Assets/{_saveFolder}");
    }

    // ──────────────────────────────────────────────────────────
    //  Utilitaires
    // ──────────────────────────────────────────────────────────

    private static void EnsureFolderExists(string folderPath)
    {
        // Crée récursivement les dossiers manquants
        string[] parts = folderPath.Split('/');
        string current = parts[0]; // "Assets"

        for (int i = 1; i < parts.Length; i++)
        {
            string next = current + "/" + parts[i];
            if (!AssetDatabase.IsValidFolder(next))
                AssetDatabase.CreateFolder(current, parts[i]);
            current = next;
        }
    }
}