using TMPro;
using UnityEngine;

public class NPCTalker : MonoBehaviour
{
    [Header("Bulle")]
    [SerializeField]
    private SpriteRenderer _bubbleRenderer;

    [SerializeField]
    private TextMeshPro _bubbleText;

    [SerializeField]
    private Transform _bubbleParent;

    [SerializeField]
    private SpriteRenderer _nameBubbleRenderer;

    [SerializeField]
    private TextMeshPro _nameBubbleText;

    [SerializeField]
    private Vector2 _padding = new Vector2(0.5f, 0.3f);

    private Vector2 _baseTextOffset = new Vector2(-4.27f, -0.17f);

    private Vector2 _nameTextOffset = new Vector2(-8, 0);

    private NPCDialogue _dialogue;

    public int _lineIndex { get; private set; } = 0;
    public bool _hasDialogueEnded { get; private set; } = false;

    private bool _bubbleVisible = false;

    private Vector3 _pivOffsetShop = new Vector3(1f, 0f, 0);

    [SerializeField]
    private Vector3 _pivOffsetBook = new Vector3(-1, -3, 0);

    void Start()
    {
        _bubbleRenderer.enabled = false;

        _bubbleText.enabled = false;

        _nameBubbleRenderer.enabled = false;

        _nameBubbleText.enabled = false;
    }

    public void StartDialogue(NPCDialogue SO_dialogue)
    {
        print("Starting Dialogue");

        _dialogue = SO_dialogue;

        if(_dialogue.)

        // Plus de r�pliques -> fermer la bulle

        if (_lineIndex >= _dialogue.lines.Length)
        {
            CloseBubble();
            _hasDialogueEnded = true;
            _dialogue.timesEnded++;
            return;
        }

        // Afficher la r�plique courante
        if(_dialogue.isLoopable)
        {
            ShowLine(_dialogue.lines[_lineIndex]);
            _lineIndex++;
        }
        else if (!_dialogue.isLoopable && timesEnded == 0)
        {
            ShowLine(_dialogue.lines[_lineIndex]);
            _lineIndex++;
        }
        else
            return;

        
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

            Vector2 nameSize =
                _nameBubbleText.textBounds.size + new Vector3(_padding.x, _padding.y, 0);

            _nameBubbleRenderer.size = nameSize;

            // Name bubble au dessus du coin sup�rieur gauche de la bulle principale

            float nameBubbleX = offsetX + nameSize.x / 5f;
            float nameBubbleY = offsetY + (newSize.y / 5f)*4; // bord sup�rieur de la bulle principale

            _nameBubbleRenderer.transform.localPosition = new Vector3(nameBubbleX, nameBubbleY, 0f);
        }
        else
        {
            // Ancrage bord gauche + bord sup�rieur fixe

            float offsetX = newSize.x / 5f;
            float offsetY = -newSize.y / 5f; // bord sup�rieur coll� au point d'ancrage

            _bubbleRenderer.transform.localPosition = new Vector3(offsetX, offsetY, 0f);

            _bubbleText.transform.localPosition =
                new Vector3(offsetX, offsetY, -0.5f) + (Vector3)_baseTextOffset;

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

        _lineIndex = 0; // reset pour rejouer si besoin
    }
}
