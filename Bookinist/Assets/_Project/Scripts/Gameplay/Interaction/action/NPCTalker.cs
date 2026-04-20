using System;
using System.Collections;
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

    [Header("Bulle de pensée")]
    [SerializeField]
    private Transform _alreadyRead;
    [SerializeField]
    private Transform _notRead;

    [Header("Autre")]
    [SerializeField]
    private Vector2 _padding = new Vector2(0.5f, 0.3f);

    private Vector2 _baseTextOffset = new Vector2(-4.27f, -0.17f);

    private Vector2 _nameTextOffset = new Vector2(-8, 0);

    private NPCDialogue _dialogue;

    private float _animationInterval = 0.4f;

    private Coroutine _indicatorCoroutine;
    private Transform[] _notReadParts;
    private int _timesEnded;
    private Transform _thinkBubble;

    public int _lineIndex { get; private set; } = 0;
    public bool _hasDialogueEnded { get; private set; } = false;

    private bool _bubbleVisible = false;

    private Vector3 _pivOffsetShop = new Vector3(1f, 0f, 0);

    [SerializeField]
    private Vector3 _pivOffsetBook = new Vector3(-1, -3, 0);

    public event Action OnShowBook;
    public event Action OnDialogEnd;

    void Start()
    {
        _bubbleRenderer.enabled = false;

        _bubbleText.enabled = false;

        _nameBubbleRenderer.enabled = false;

        _nameBubbleText.enabled = false;

        _thinkBubble = _notRead.parent.transform;

        _notReadParts = new Transform[_notRead.childCount];
        for (int i = 0; i < _notRead.childCount; i++)
            _notReadParts[i] = _notRead.GetChild(i);

        UpdateIndicator();
    }

    public void StartDialogue(NPCDialogue SO_dialogue)
    {
        print("Starting Dialogue");

        _dialogue = SO_dialogue;

        // Plus de répliques -> fermer la bulle

        if (_lineIndex >= _dialogue.lines.Length)
        {
            CloseBubble();
            _hasDialogueEnded = true;
            _timesEnded++;
            UpdateIndicator();
            OnDialogEnd?.Invoke();
            return;
        }

        // Afficher la réplique courante

        if (_dialogue.isLoopable)
        {
            ShowLine(_dialogue.lines[_lineIndex]);
            _lineIndex++;
        }
        else if (!_dialogue.isLoopable && _timesEnded == 0)
        {
            ShowLine(_dialogue.lines[_lineIndex]);
            _lineIndex++;
        }

        if (_dialogue.IsShopNPC && _lineIndex == 4)
        {
            OnShowBook?.Invoke();
        }
    }

    private void ShowLine(string text)
    {
        _thinkBubble.gameObject.SetActive(false);

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

            // Name bubble au dessus du coin supérieur gauche de la bulle principale

            float nameBubbleX = offsetX + nameSize.x / 5f;
            float nameBubbleY = offsetY + (newSize.y / 5f)*4; // bord supérieur de la bulle principale

            _nameBubbleRenderer.transform.localPosition = new Vector3(nameBubbleX, nameBubbleY, 0f);
        }
        else
        {
            // Ancrage bord gauche + bord supérieur fixe

            float offsetX = newSize.x / 5f;
            float offsetY = -newSize.y / 5f; // bord supérieur collé au point d'ancrage

            _bubbleRenderer.transform.localPosition = new Vector3(offsetX, offsetY, 0f);

            _bubbleText.transform.localPosition =
                new Vector3(offsetX, offsetY, -0.5f) + (Vector3)_baseTextOffset;

            _bubbleParent.transform.localPosition = _pivOffsetBook;
        }
    }

    private void CloseBubble()
    {
        _thinkBubble.gameObject.SetActive(true);

        _bubbleRenderer.enabled = false;

        _bubbleText.enabled = false;

        _bubbleVisible = false;

        _nameBubbleRenderer.enabled = false;

        _nameBubbleText.enabled = false;

        _lineIndex = 0; // reset pour rejouer si besoin
    }

    public void UpdateIndicator()
    {
        bool hasBeenRead = _dialogue != null && _timesEnded > 0;

        _alreadyRead.gameObject.SetActive(hasBeenRead);
        _notRead.gameObject.SetActive(!hasBeenRead);

        if (_indicatorCoroutine != null)
            StopCoroutine(_indicatorCoroutine);

        if (!hasBeenRead)
            _indicatorCoroutine = StartCoroutine(AnimateNotRead());

        if( _dialogue == null)
        {
            _indicatorCoroutine = StartCoroutine(AnimateNotRead());
            return;
        }

        if(!_dialogue.isLoopable &&  _timesEnded > 0)
        {
            _thinkBubble.gameObject.SetActive(false);
        }
    }

    private IEnumerator AnimateNotRead()
    {
        int index = 0;
        while (true)
        {
            for (int i = 0; i < _notReadParts.Length; i++)
                _notReadParts[i].gameObject.SetActive(i == index);

            index = (index + 1) % _notReadParts.Length;
            yield return new WaitForSeconds(_animationInterval);
        }
    }
}
