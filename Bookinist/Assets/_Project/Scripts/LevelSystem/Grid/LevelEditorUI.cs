using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Optional UI component for the Level Editor.
/// Generates layer-select buttons and palette buttons at runtime.
/// Attach to a Canvas GameObject that contains:
///   - A child Transform assigned to LayerBar   (e.g. with HorizontalLayoutGroup)
///   - A child Transform assigned to PaletteBar (e.g. with HorizontalLayoutGroup)
/// </summary>
public class LevelEditorUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LevelEditor _editor;
    [SerializeField] private Transform _layerBar;   // parent for layer toggle buttons
    [SerializeField] private Transform _paletteBar; // parent for prefab buttons

    [Header("Button Prefab")]
    [SerializeField] private GameObject _buttonPrefab;

    [Header("Layer Button Labels")]
    [SerializeField] private List<string> _layerLabels = new();

    [Header("Palette Button Sprites")]
    [SerializeField] private List<Sprite> _paletteIcons = new();

    private static readonly Color ActiveColor = new(0.2f, 0.8f, 1f);
    private static readonly Color InactiveColor = Color.white;

    private readonly List<Button> _layerButtons = new();
    private readonly List<Button> _paletteButtons = new();

    // 
    //  Unity lifecycle
    // 

    private void Start()
    {
        BuildLayerButtons();
        BuildPaletteButtons();
        RefreshHighlight();
    }

    // 
    //  Build buttons
    // 

    private void BuildLayerButtons()
    {
        if (_layerBar == null || _buttonPrefab == null || _editor == null) return;

        for (int i = 0; i < _editor.LayerCount; i++)
        {
            int captured = i;
            GameObject btn = Instantiate(_buttonPrefab, _layerBar);

            string label = (i < _layerLabels.Count && !string.IsNullOrEmpty(_layerLabels[i]))
                ? _layerLabels[i]
                : $"Layer {i + 1}";

            SetButtonLabel(btn, label);

            Button b = btn.GetComponent<Button>();
            b.onClick.AddListener(() =>
            {
                _editor.SetActiveLayer(captured);
                RefreshHighlight();
            });
            _layerButtons.Add(b);
        }
    }

    private void BuildPaletteButtons()
    {
        if (_paletteBar == null || _buttonPrefab == null || _editor == null) return;

        for (int i = 0; i < _editor.PaletteCount; i++)
        {
            int captured = i;
            GameObject btn = Instantiate(_buttonPrefab, _paletteBar);

            if (i < _paletteIcons.Count && _paletteIcons[i] != null)
            {
                Image img = btn.GetComponentInChildren<Image>();
                if (img != null) img.sprite = _paletteIcons[i];
            }
            else
            {
                SetButtonLabel(btn, $"Item {i + 1}");
            }

            Button b = btn.GetComponent<Button>();
            b.onClick.AddListener(() =>
            {
                _editor.SetSelectedPrefab(captured);
                RefreshHighlight();
            });
            _paletteButtons.Add(b);
        }
    }

    // 
    //  Highlight active buttons
    // 

    private void RefreshHighlight()
    {
        if (_editor == null) return;

        for (int i = 0; i < _layerButtons.Count; i++)
            SetButtonHighlight(_layerButtons[i], i == _editor.ActiveLayerIndex);

        for (int i = 0; i < _paletteButtons.Count; i++)
            SetButtonHighlight(_paletteButtons[i], i == _editor.SelectedPaletteIndex);
    }

    private static void SetButtonHighlight(Button btn, bool active)
    {
        ColorBlock cb = btn.colors;
        cb.normalColor = active ? ActiveColor : InactiveColor;
        btn.colors = cb;
    }

    // 
    //  Helpers
    // 

    private static void SetButtonLabel(GameObject btn, string text)
    {
        TMP_Text tmp = btn.GetComponentInChildren<TMP_Text>();
        if (tmp != null)
        {
            tmp.text = text;
            return;
        }
        Text legacy = btn.GetComponentInChildren<Text>();
        if (legacy != null) legacy.text = text;
    }
}