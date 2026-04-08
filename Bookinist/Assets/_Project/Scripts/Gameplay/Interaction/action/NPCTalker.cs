using TMPro;
using UnityEngine;

public class NPCTalker : MonoBehaviour
{
    #region Variables

    [Header("Bulle")]
    [SerializeField] private SpriteRenderer _bubbleRenderer;
    [SerializeField] private TextMeshPro _bubbleText;
    [SerializeField] private Transform _bubbleParent;
    [SerializeField] private SpriteRenderer _nameBubbleRenderer;
    [SerializeField] private TextMeshPro _nameBubbleText;
    [SerializeField] private Vector2 _padding = new(0.5f, 0.3f);

    private Vector2 _baseTextOffset = new(-4.27f, -0.17f);
    private Vector2 _nameTextOffset = new(-8, 0);
    private NPCDialogue _dialogue;

    public int LineIndex { get; private set; } = 0;
    public bool HasDialogueEnded { get; private set; } = false;

    private bool _bubbleVisible = false;
    private Vector3 _pivOffsetShop = new(1f, 0f, 0);
    private Vector3 _pivOffsetBook = new(-1, -3, 0);

    #endregion

    #region Unity Methods

    private void Start()
    {
        _bubbleRenderer.enabled = false;
        _bubbleText.enabled = false;
        _nameBubbleRenderer.enabled = false;
        _nameBubbleText.enabled = false;
    }

    #endregion

    #region Methods

    public void StartDialogue(NPCDialogue SO_dialogue)
    {
        print("Starting Dialogue");

        _dialogue = SO_dialogue;

        // Plus de répliques -> fermer la bulle
        if (LineIndex >= _dialogue.lines.Length)
        {
            CloseBubble();
            HasDialogueEnded = true;
            return;
        }

        // Afficher la réplique courante
        ShowLine(_dialogue.lines[LineIndex]);
        LineIndex++;
    }

    private void ShowLine(string text)
    {
        _bubbleText.text = text;
        _bubbleRenderer.enabled = true;
        _bubbleText.enabled = true;
        _bubbleVisible = true;

        _bubbleText.transform.localPosition = _baseTextOffset;
        _bubbleText.ForceMeshUpdate();
        Vector2 textSize = _bubbleText.textBounds.size;
        Vector2 newSize = textSize + _padding;
        _bubbleRenderer.size = newSize;

        if (_dialogue.IsShopNPC)
        {
            _bubbleRenderer.sortingLayerName = "Bubbles";
            _bubbleRenderer.sortingOrder = 0;
            _nameBubbleRenderer.sortingLayerName = "Bubbles";
            _nameBubbleRenderer.sortingOrder = 1;

            _bubbleText.GetComponent<TextMeshPro>().sortingOrder = 0;
            _nameBubbleText.GetComponent<TextMeshPro>().sortingOrder = 1;

            _nameBubbleRenderer.enabled = true;
            _nameBubbleText.enabled = true;
            _nameBubbleText.text = _dialogue.NPCName;
            _nameBubbleText.ForceMeshUpdate();

            _nameBubbleText.transform.localPosition = _nameTextOffset;
            _bubbleParent.transform.localPosition = _pivOffsetShop;
            _bubbleParent.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

            float offsetX = newSize.x / 5f;
            float offsetY = -newSize.y / 5f;

            Vector2 nameSize = _nameBubbleText.textBounds.size + new Vector3(_padding.x, _padding.y, 0);
            _nameBubbleRenderer.size = nameSize;

            float nameBubbleX = offsetX + nameSize.x / 5f;
            float nameBubbleY = offsetY + (newSize.y / 5f) * 4;
            _nameBubbleRenderer.transform.localPosition = new Vector3(nameBubbleX, nameBubbleY, 0f);
        }
        else
        {
            float offsetX = newSize.x / 5f;
            float offsetY = -newSize.y / 5f;

            _bubbleRenderer.transform.localPosition = new Vector3(offsetX, offsetY, 0f);
            _bubbleText.transform.localPosition = new Vector3(offsetX, offsetY, -0.5f) + (Vector3)_baseTextOffset;
            _bubbleParent.transform.localPosition = _pivOffsetBook;
        }
    }

    private void CloseBubble()
    {
        _bubbleRenderer.enabled = false;
        _bubbleText.enabled = false;
        _bubbleVisible = false;
        _nameBubbleRenderer.enabled = false;
        _nameBubbleText.enabled = false;
        LineIndex = 0; // reset pour rejouer si besoin
    }

    #endregion
}
