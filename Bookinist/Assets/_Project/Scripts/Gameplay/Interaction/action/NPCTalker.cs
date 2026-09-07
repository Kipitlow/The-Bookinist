using System;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private Vector3 _bubbleSize = new Vector3(1f, 1f, 1f);

    private NPCDialogue _dialogue;

    private float _animationInterval = 0.4f;

    private Coroutine _indicatorCoroutine;
    private Transform[] _notReadParts;
    private int _timesEnded;
    private Transform _thinkBubble;

    public int _lineIndex { get; private set; } = 0;
    public bool _hasDialogueEnded { get; private set; } = false;

    private bool _bubbleVisible = false;
    public bool _hasStarted = false;

    private Vector3 _pivOffsetShop = new Vector3(1f, 0f, 0);

    [SerializeField]
    private List<Sprite> _spritePoses;

    public event Action<bool> OnShowBook;
    public event Action<bool> OnDialogEnd;

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

        if (GameManager.Instance.bookFinish)
        {
            GetComponent<SpriteRenderer>().sprite = _spritePoses[1];
        }
    }

    public void StartDialogue(NPCDialogue SO_dialogue)
    {
        if (SO_dialogue.IsShopNPC)
            GetComponent<SpriteRenderer>().sprite = _spritePoses[2];
        
        _hasStarted = true;

        _dialogue = SO_dialogue;

        // Plus de répliques -> fermer la bulle

        if (_lineIndex >= _dialogue.lines.Length)
        {
            CloseBubble();
            _hasDialogueEnded = true;
            _timesEnded++;
            UpdateIndicator();
            OnDialogEnd?.Invoke(true);
            GameManager.Instance._isFirstCustomerFinishDialog = true;
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

        if (_dialogue.IsShopNPC && _lineIndex == 4 && GameManager.Instance.bookFinish == false)
        {
            GetComponent<SpriteRenderer>().sprite = _spritePoses[1];
            OnShowBook?.Invoke(true);
        }
    }

    public void CustomerLeave(NPCDialogue SO_dialogue, GameObject talker)
    {
        _dialogue = SO_dialogue;

        if (_lineIndex == 1)
        {
            GetComponent<SpriteRenderer>().sprite = _spritePoses[2];
        }

        if (_lineIndex >= _dialogue.lines.Length)
        {
            CloseBubble();
            _hasDialogueEnded = true;
            _timesEnded++;
            UpdateIndicator();
            OnDialogEnd?.Invoke(false);
            StartCoroutine(WaitToInvisible(0.7f, talker));
            return;
        }

        if (!_dialogue.isLoopable && _timesEnded == 0)
        {
            ShowLine(_dialogue.lines[_lineIndex]);
            _lineIndex++;
        }
    }

    IEnumerator WaitToInvisible(float delay, GameObject talker)
    {
        yield return new WaitForSeconds(delay);

        talker.gameObject.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
    }

    private void ShowLine(string text)
    {
        _thinkBubble.gameObject.SetActive(false);

        _bubbleText.text = text;

        _bubbleRenderer.enabled = true;

        _bubbleText.enabled = true;

        _bubbleVisible = true;

        Vector2 textSize = _bubbleText.textBounds.size;

        Vector2 newSize = textSize + _padding;


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

            _nameBubbleText.transform.localPosition = _nameTextOffset;
            _bubbleParent.transform.localPosition = _pivOffsetShop;
            _bubbleParent.transform.localScale = _bubbleSize;

            float offsetX = newSize.x / 5f;
            float offsetY = -newSize.y / 5f;

            Vector2 nameSize =
                _nameBubbleText.textBounds.size + new Vector3(_padding.x, _padding.y, 0);

            _nameBubbleRenderer.size = nameSize;

            // Name bubble au dessus du coin supérieur gauche de la bulle principale

            float nameBubbleX = offsetX + nameSize.x / 5f;
            float nameBubbleY = offsetY + (newSize.y / 5f)*4; // bord supérieur de la bulle principale

        }
        else
        {
            // Ancrage bord gauche + bord supérieur fixe

            float offsetX = newSize.x / 5f;
            float offsetY = -newSize.y / 5f; // bord supérieur collé au point d'ancrage
        }
    }

    public void CloseBubble()
    {
        _thinkBubble.gameObject.SetActive(true);

        _bubbleRenderer.enabled = false;

        _bubbleText.enabled = false;

        _bubbleVisible = false;

        _nameBubbleRenderer.enabled = false;

        _nameBubbleText.enabled = false;

        _lineIndex = 0; // reset pour rejouer si besoin
        if (_dialogue.isLoopable)
            _hasStarted = false;
    }

    public void UpdateIndicator()
    {
        bool hasBeenRead = _dialogue != null && _timesEnded > 0;
        //Debug.Log(hasBeenRead);

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
