using TMPro;
using UnityEngine;

public class DialogueBubble : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private SpriteRenderer _background;
    [SerializeField] private TextMeshPro _text;

    [Header("Position")]
    [Tooltip("Décalage en unités monde par rapport au pivot du PNJ.")]
    [SerializeField] private Vector3 _offset = new Vector3(0.5f, 2.5f, -0.1f);

    [Header("Apparence")]
    [Tooltip("Marge autour du texte en unités monde.")]
    [SerializeField] private Vector2 _padding = new Vector2(0.3f, 0.2f);
    [SerializeField] private float _maxWidth = 3f;

    // ──────────────────────────────────────────────────────────
    //  Unity lifecycle
    // ──────────────────────────────────────────────────────────

    private void Awake()
    {
        Hide();
    }

    // ──────────────────────────────────────────────────────────
    //  API publique
    // ──────────────────────────────────────────────────────────

    public void Show(string content)
    {
        _background.gameObject.SetActive(true);
        _text.gameObject.SetActive(true);

        // Positionne la bulle au-dessus du PNJ
        _background.transform.localPosition = _offset;
        _text.transform.localPosition = _offset + new Vector3(0f, 0f, -0.01f);

        // Applique le texte
        _text.text = content;
        _text.maxVisibleCharacters = int.MaxValue;

        // Adapte la largeur du texte puis redimensionne le fond
        _text.rectTransform.sizeDelta = new Vector2(_maxWidth, 0f);
        _text.ForceMeshUpdate();

        float textW = _text.renderedWidth;
        float textH = _text.renderedHeight;

        _background.size = new Vector2(
            textW + _padding.x * 2f,
            textH + _padding.y * 2f);
    }

    public void Hide()
    {
        if (_background != null) _background.gameObject.SetActive(false);
        if (_text != null) _text.gameObject.SetActive(false);
    }
}