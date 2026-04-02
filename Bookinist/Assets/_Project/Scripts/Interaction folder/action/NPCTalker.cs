using TMPro;
using UnityEngine;

public class NPCTalker : MonoBehaviour
{
    [Header("Bulle")]
    [SerializeField] private SpriteRenderer _bubbleRenderer;
    [SerializeField] private TextMeshPro _bubbleText;
    [SerializeField] private Transform _bubbleParent;
    [SerializeField] private SpriteRenderer _nameBubbleRenderer;
    [SerializeField] private TextMeshPro _nameBubbleText;
    [SerializeField] private Vector2 _padding = new Vector2(0.5f, 0.3f);
    
    private Vector2 _baseTextOffset = new Vector2(-4.27f, -0.17f);
    private Vector2 _nameTextOffset = new Vector2(-8,0);

    private NPCDialogue _dialogue;

    private int _lineIndex = 0;
    private bool _bubbleVisible = false;

    private Vector3 _pivOffsetShop = new Vector3(1f,0f,0);
    private Vector3 _pivOffsetBook = new Vector3(5,5,0);

    void Start()
    {
        _bubbleRenderer.enabled = false;
        _bubbleText.enabled = false;
        _nameBubbleRenderer.enabled = false;
        _nameBubbleText.enabled = false;

    }

    public void StartDialogue(NPCDialogue SO_dialogue)
    {
        _dialogue = SO_dialogue;

        // Plus de répliques -> fermer la bulle
        if (_lineIndex >= _dialogue.lines.Length)
        {
            CloseBubble();
            return;
        }

        // Afficher la réplique courante
        ShowLine(_dialogue.lines[_lineIndex]);
        _lineIndex++;
    }

    private void ShowLine(string text)
    {
        _bubbleText.text = text;
        _bubbleRenderer.enabled = true;
        _bubbleText.enabled = true;
        _bubbleVisible = true;

        // Calcul de la bulle de dialogue
        _bubbleText.ForceMeshUpdate();
        Vector2 textSize = _bubbleText.textBounds.size;
        Vector2 newSize = textSize + _padding;
        _bubbleRenderer.size = newSize;

        // Ancrage bord gauche, bord supérieur fixe
        float offsetX = newSize.x / 2f;
        float offsetY = -newSize.y / 2f;

        _bubbleRenderer.transform.localPosition = new Vector3(offsetX, offsetY, 0f);
        _bubbleText.transform.localPosition = new Vector3(offsetX, offsetY, -0.1f);

        if (_dialogue.IsShopNPC)
        {
            _nameBubbleRenderer.enabled = true;
            _nameBubbleText.enabled = true;
            _nameBubbleText.text = _dialogue.NPCName;
            _nameBubbleText.ForceMeshUpdate();

            Vector2 nameSize = _nameBubbleText.textBounds.size + (Vector3)_padding;
            _nameBubbleRenderer.size = nameSize;

            // Coin supérieur gauche de la bulle dialogue = (0, 0) + décalages
            // Bord gauche aligné, collé juste au dessus
            float nameX = nameSize.x / 2f;
            float nameY = offsetY + newSize.y / 2f
                          + nameSize.y / 2f;

            _nameBubbleRenderer.transform.localPosition = new Vector3(nameX, nameY, 0f);
            _nameBubbleText.transform.localPosition = new Vector3(nameX, nameY, -0.1f);

            _bubbleParent.transform.localPosition = _pivOffsetShop;
        }
        else
        {
            _nameBubbleRenderer.enabled = false;
            _nameBubbleText.enabled = false;
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