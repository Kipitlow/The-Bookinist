using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelEditorWindow : EditorWindow
{
    #region Variables

    [MenuItem("Tools/Level Editor/Save & Load")]
    public static void OpenWindow()
    {
        LevelEditorWindow window = GetWindow<LevelEditorWindow>("Level Editor");
        window.minSize = new Vector2(320, 280);
        window.Show();
    }

    private string _saveFolder = "Levels/Level_01";
    private LevelEditor _levelEditor;
    private Vector2 _scrollPos;

    #endregion

    #region GUI

    private void OnGUI()
    {
        EditorGUILayout.Space(8);
        EditorGUILayout.LabelField("Level Editor - Save & Load", EditorStyles.boldLabel);
        EditorGUILayout.Space(4);

        _levelEditor = (LevelEditor)EditorGUILayout.ObjectField(
            "Level Editor (optionnel)",
            _levelEditor,
            typeof(LevelEditor),
            allowSceneObjects: true);

        if (_levelEditor == null)
        {
            EditorGUILayout.HelpBox(
                "Sans LevelEditor : seul le chargement est disponible (scène Gameplay)." +
                "Avec LevelEditor : sauvegarde et chargement disponibles (scène Editor).",
                MessageType.Info);

            if (GUILayout.Button("Chercher automatiquement dans la scène"))
                _levelEditor = FindFirstObjectByType<LevelEditor>();
        }

        EditorGUILayout.Space(8);

        EditorGUILayout.LabelField("Dossier de sauvegarde", EditorStyles.boldLabel);
        _saveFolder = EditorGUILayout.TextField("Assets/", _saveFolder);

        EditorGUILayout.Space(8);

        EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);

        GUI.backgroundColor = new Color(0.4f, 0.9f, 0.4f);
        using (new EditorGUI.DisabledGroupScope(_levelEditor == null))
        {
            if (GUILayout.Button("💾  Sauvegarder le niveau", GUILayout.Height(36)))
                SaveAll();
        }

        GUI.backgroundColor = new Color(0.4f, 0.7f, 1f);
        if (GUILayout.Button("📂  Charger le niveau", GUILayout.Height(36)))
        {
            if (EditorUtility.DisplayDialog(
                "Charger le niveau",
                "Cette action va effacer tous les objets actuellement placés. Continuer ?",
                "Oui, charger",
                "Annuler"))
                LoadAll();
        }

        GUI.backgroundColor = new Color(1f, 0.4f, 0.4f);
        if (GUILayout.Button("🗑️  Vider toutes les grilles", GUILayout.Height(28)))
        {
            if (EditorUtility.DisplayDialog(
                "Vider les grilles",
                "Tous les objets placés seront supprimés. Continuer ?",
                "Oui, vider",
                "Annuler"))
                ClearAll();
        }

        GUI.backgroundColor = Color.white;

        EditorGUILayout.Space(8);

        DrawExistingAssets();
    }

    #endregion

    #region Methods

    private void DrawExistingAssets()
    {
        string fullFolder = $"Assets/{_saveFolder}";
        if (!AssetDatabase.IsValidFolder(fullFolder))
        {
            EditorGUILayout.HelpBox($"Dossier introuvable : {fullFolder}", MessageType.Warning);
            return;
        }

        string[] guids = AssetDatabase.FindAssets("t:LevelData", new[] { fullFolder });
        if (guids.Length == 0)
        {
            EditorGUILayout.HelpBox("Aucun asset LevelData dans ce dossier.", MessageType.Info);
            return;
        }

        EditorGUILayout.LabelField($"Assets sauvegardés ({guids.Length})", EditorStyles.boldLabel);
        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.MaxHeight(120));

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            LevelData data = AssetDatabase.LoadAssetAtPath<LevelData>(path);
            if (data == null) continue;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"  {data.layerName}  ({data.entries.Count} objets)");
            if (GUILayout.Button("Sélectionner", GUILayout.Width(90)))
                Selection.activeObject = data;
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
    }

    private void SaveAll()
    {
        if (_levelEditor == null) return;

        string fullFolder = $"Assets/{_saveFolder}";
        EnsureFolderExists(fullFolder);

        List<LayerGrid> layers = _levelEditor.Layers;

        for (int i = 0; i < layers.Count; i++)
        {
            LayerGrid grid = layers[i];
            if (grid == null) continue;

            Page page = grid.GetComponent<Page>();
            int pageIndex = page != null ? page.PageIndex : i;

            string assetPath = $"{fullFolder}/LevelData_Page{pageIndex}.asset";
            LevelData data = AssetDatabase.LoadAssetAtPath<LevelData>(assetPath);

            if (data == null)
            {
                data = CreateInstance<LevelData>();
                AssetDatabase.CreateAsset(data, assetPath);
            }

            data.layerName = grid.gameObject.name;
            data.pageIndex = pageIndex;
            data.entries.Clear();

            foreach (var kvp in grid.GetPlacedObjects())
            {
                if (kvp.Value == null) continue;

                if (!kvp.Value.TryGetComponent<PlacedObject>(out var po))
                {
                    Debug.LogWarning($"[LevelEditorWindow] Objet en {kvp.Key} sans composant PlacedObject - ignoré.", kvp.Value);
                    continue;
                }

                if (po.SourcePrefab == null)
                {
                    Debug.LogWarning($"[LevelEditorWindow] Objet en {kvp.Key} sans prefab source enregistré - ignoré.", kvp.Value);
                    continue;
                }

                data.entries.Add(new LevelData.PlacedEntry
                {
                    cell = kvp.Key,
                    prefab = po.SourcePrefab,
                    manualSortingOffset = po.ManualSortingOffset
                });
            }

            grid.SetLevelData(data);
            EditorUtility.SetDirty(data);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"[LevelEditorWindow] Sauvegarde terminée dans Assets/{_saveFolder}");
    }

    private void LoadAll()
    {
        string fullFolder = $"Assets/{_saveFolder}";

        List<LayerGrid> layers;
        if (_levelEditor != null)
        {
            layers = _levelEditor.Layers;
        }
        else
        {
            layers = new List<LayerGrid>(FindObjectsByType<LayerGrid>(FindObjectsSortMode.None));
            layers.Sort((a, b) =>
            {
                Page pa = a.GetComponent<Page>();
                Page pb = b.GetComponent<Page>();
                int ia = pa != null ? pa.PageIndex : 0;
                int ib = pb != null ? pb.PageIndex : 0;
                return ia.CompareTo(ib);
            });
        }

        if (layers.Count == 0)
        {
            Debug.LogWarning("[LevelEditorWindow] Aucun LayerGrid trouvé dans la scène.");
            return;
        }

        for (int i = 0; i < layers.Count; i++)
        {
            LayerGrid grid = layers[i];
            if (grid == null) continue;

            Page page = grid.GetComponent<Page>();
            int pageIndex = page != null ? page.PageIndex : i;

            string assetPath = $"{fullFolder}/LevelData_Page{pageIndex}.asset";
            LevelData data = AssetDatabase.LoadAssetAtPath<LevelData>(assetPath);

            if (data == null)
            {
                Debug.LogWarning($"[LevelEditorWindow] Aucun asset trouvé à {assetPath}.");
                continue;
            }

            grid.ClearAll();

            foreach (var entry in data.entries)
            {
                if (entry.prefab == null)
                {
                    Debug.LogWarning($"[LevelEditorWindow] Prefab manquant pour la cellule {entry.cell} sur {data.layerName}.");
                    continue;
                }

                GameObject placed = grid.PlaceObject(entry.prefab, entry.cell);
                if (placed == null) continue;

                if (placed.TryGetComponent<PlacedObject>(out var po)) po.ManualSortingOffset = entry.manualSortingOffset;
            }

            grid.SetLevelData(data);
            EditorUtility.SetDirty(grid);
        }

        UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
        Debug.Log($"[LevelEditorWindow] Chargement terminé depuis Assets/{_saveFolder}. Sauvegarde la scène (Ctrl+S) pour conserver les objets.");
    }

    private void ClearAll()
    {
        if (_levelEditor == null) return;
        foreach (LayerGrid grid in _levelEditor.Layers)
            grid?.ClearAll();
    }

    private static void EnsureFolderExists(string folderPath)
    {
        string[] parts = folderPath.Split('/');
        string current = parts[0];

        for (int i = 1; i < parts.Length; i++)
        {
            string next = current + "/" + parts[i];
            if (!AssetDatabase.IsValidFolder(next))
                AssetDatabase.CreateFolder(current, parts[i]);
            current = next;
        }
    }

    #endregion
}