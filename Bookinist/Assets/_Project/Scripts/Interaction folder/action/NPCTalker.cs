using TMPro;
using UnityEngine;

public class NPCTalker : MonoBehaviour
{
    [Header("Bulle")]
    [SerializeField] private SpriteRenderer _bubbleRenderer;
    [SerializeField] private TextMeshPro _bubbleText;

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
    }

    private void CloseBubble()
    {
        _bubbleRenderer.enabled = false;
        _bubbleText.enabled = false;
        _bubbleVisible = false;
        _lineIndex = 0; // reset pour rejouer si besoin
    }
}