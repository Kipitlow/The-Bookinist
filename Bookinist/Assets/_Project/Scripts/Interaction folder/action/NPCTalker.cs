using TMPro;
using UnityEngine;

public class NPCTalker : MonoBehaviour
{
    [Header("Bulle")]
    [SerializeField] private SpriteRenderer _bubbleRenderer;
    [SerializeField] private TextMeshPro _bubbleText;
    [SerializeField] private Transform _bubbleParent;
    [SerializeField] private Vector2 _padding = new Vector2(0.5f, 0.3f);
    [SerializeField] private Vector2 _baseTextOffset = new Vector2(-4.27f, -0.17f);

    private NPCDialogue _dialogue;

    private int _lineIndex = 0;
    private bool _bubbleVisible = false;

    void Start()
    {
        _bubbleRenderer.enabled = false;
        _bubbleText.enabled = false;
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

        _bubbleText.transform.localPosition = _baseTextOffset;

        _bubbleText.ForceMeshUpdate();

        Vector2 textSize = _bubbleText.textBounds.size;
        Vector2 newSize = textSize + _padding;
        _bubbleRenderer.size = newSize;

        // Décaler le sprite et le texte vers la droite pour ancrer leur bord gauche au parent
        float offsetX = newSize.x / 5f;

        _bubbleRenderer.transform.localPosition = new Vector3(offsetX, 0, 0);
        _bubbleText.transform.localPosition = new Vector3(offsetX, 0, 0) + new Vector3(_baseTextOffset.x, _baseTextOffset.y, -0.1f);
    }

    private void CloseBubble()
    {
        _bubbleRenderer.enabled = false;
        _bubbleText.enabled = false;
        _bubbleVisible = false;
        _lineIndex = 0; // reset pour rejouer si besoin
    }
}