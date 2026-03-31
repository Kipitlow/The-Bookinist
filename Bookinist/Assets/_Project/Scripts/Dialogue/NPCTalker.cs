using TMPro;
using UnityEngine;

public class NPCTalker : MonoBehaviour
{
    [Header("Données")]
    [SerializeField] private NPCDialogue dialogue;

    [Header("Bulle")]
    [SerializeField] private SpriteRenderer bubbleRenderer;
    [SerializeField] private TextMeshPro bubbleText;

    private int _lineIndex = 0;
    private bool _bubbleVisible = false;

    void Start()
    {
        bubbleRenderer.enabled = false;
        bubbleText.enabled = false;
    }

    void Update()
    {

        // Plus de répliques -> fermer la bulle
        if (_lineIndex >= dialogue.lines.Length)
        {
            CloseBubble();
            return;
        }

        // Afficher la réplique courante
        ShowLine(dialogue.lines[_lineIndex]);
        _lineIndex++;
    }

    private void ShowLine(string text)
    {
        bubbleText.text = text;
        bubbleRenderer.enabled = true;
        bubbleText.enabled = true;
        _bubbleVisible = true;
    }

    private void CloseBubble()
    {
        bubbleRenderer.enabled = false;
        bubbleText.enabled = false;
        _bubbleVisible = false;
        _lineIndex = 0; // reset pour rejouer si besoin
    }
}