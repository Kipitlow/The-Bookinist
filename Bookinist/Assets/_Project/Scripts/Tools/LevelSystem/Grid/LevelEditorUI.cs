using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UI helper pour le LevelEditor : boutons de layers & palette.
/// </summary>
public class LevelEditorUI : MonoBehaviour
{
    #region Variables

    [Header("References")]
    [SerializeField] private LevelEditor _editor;
    [SerializeField] private Transform _layerBar;
    [SerializeField] private Transform _paletteBar;

    [Header("Button Prefab (must have a Button + TMP_Text child)")]
    [SerializeField] private GameObject _buttonPrefab;

    [Header("Layer Button Labels (optional, leave empty for auto-numbering)")]
    [SerializeField] private List<string> _layerLabels = new();

    [Header("Palette Button Sprites (one per prefab, optional)")]
    [SerializeField] private List<Sprite> _paletteIcons = new();

    private static readonly Color ActiveColor = new(0.2f, 0.8f, 1f);
    private static readonly Color InactiveColor = Color.white;

    private readonly List<Button> _layerButtons = new();
    private readonly List<Button> _paletteButtons = new();

    #endregion

    #region Unity Methods

    private void Start()
    {
        BuildLayerButtons();
        BuildPaletteButtons();
        RefreshHighlight();
    }

    #endregion

    #region Methods

    private void BuildLayerButtons()
    {
        if (_layerBar == null || _editor == null) return;
        for (int i = 0; i < _editor.LayerCount; i++)
        {
            int captured = i;
            GameObject btn = Instantiate(_buttonPrefab, _layerBar);
            string label = (i < _layerLabels.Count && !string.IsNullOrEmpty(_layerLabels[i])) ? _layerLabels[i] : $"Layer {i + 1}";
            SetButtonLabel(btn, label);
            if (!btn.TryGetComponent<Button>(out var b)) { Debug.LogError("[LevelEditorUI] Le _buttonPrefab n'a pas de composant Button à sa racine !", btn); continue; }
            b.onClick.AddListener(() => { _editor.SetActiveLayer(captured); RefreshHighlight(); });
            _layerButtons.Add(b);
        }
    }

    private void BuildPaletteButtons()
    {
        if (_paletteBar == null || _editor == null) return;
        for (int i = 0; i < _editor.PaletteCount; i++)
        {
            int captured = i;
            GameObject btn = Instantiate(_buttonPrefab, _paletteBar);

            if (i < _paletteIcons.Count && _paletteIcons[i] != null)
            {
                Image[] imgs = btn.GetComponentsInChildren<Image>();
                Image icon = imgs.Length > 1 ? imgs[imgs.Length - 1] : (imgs.Length > 0 ? imgs[0] : null);
                if (icon != null) icon.sprite = _paletteIcons[i];
            }
            else
            {
                SetButtonLabel(btn, $"Item {i + 1}");
            }

            if (!btn.TryGetComponent<Button>(out var b)) { Debug.LogError("[LevelEditorUI] Le _buttonPrefab n'a pas de composant Button à sa racine !", btn); continue; }
            b.onClick.AddListener(() => { _editor.SetSelectedPrefab(captured); RefreshHighlight(); });
            _paletteButtons.Add(b);
        }
    }

    private void RefreshHighlight()
    {
        if (_editor == null) return;
        for (int i = 0; i < _layerButtons.Count; i++) SetButtonHighlight(_layerButtons[i], i == _editor.ActiveLayerIndex);
        for (int i = 0; i < _paletteButtons.Count; i++) SetButtonHighlight(_paletteButtons[i], i == _editor.SelectedPaletteIndex);
    }

    private static void SetButtonHighlight(Button btn, bool active)
    {
        ColorBlock cb = btn.colors;
        cb.normalColor = active ? ActiveColor : InactiveColor;
        btn.colors = cb;
    }

    private static void SetButtonLabel(GameObject btn, string text)
    {
        TMP_Text tmp = btn.GetComponentInChildren<TMP_Text>();
        if (tmp != null) { tmp.text = text; return; }
        Text legacy = btn.GetComponentInChildren<Text>();
        if (legacy != null) legacy.text = text;
    }

    #endregion
}