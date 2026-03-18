using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public CardModel CardModel { get; private set; }
    public Action<CardView> OnCardClicked;

    [SerializeField] private GameObject selectionHighlight;
    [Tooltip("Only 4, in this order : left-top-right-bottom")]
    [SerializeField] private TextMeshProUGUI[] sideTexts;
    [SerializeField] private TextMeshProUGUI totalPowerText;

    // Nouveau : image qui affiche le visuel de la carte
    [SerializeField] private Image artworkImage;
    [SerializeField] private Image bgplayer1;
    [SerializeField] private Image bgplayer2;

    // HOVER HANDLE
    [Header("Hover")]
    [SerializeField, Min(1f)] private float hoverScaleMultiplier = 1.15f;
    [Tooltip("Si activé, l'agrandissement/reduction est lissé au lieu d'être instantané.")]
    [SerializeField] private bool smoothHover = true;
    [Tooltip("Durée du lissage (en secondes) si Smooth Hover est activé.")]
    [SerializeField, Min(0.01f)] private float hoverSmoothDuration = 0.08f;

    [Header("FX")]
    [Tooltip("Animator qui contient les triggers 'CaptureCard' et 'ToBoard' (optionnel). Si null, rien ne sera joué.")]
    [SerializeField] private Animator _fxAnimator;

    private const string TriggerCaptureCard = "CaptureCard";
    private const string TriggerToBoard = "ToBoard";

    private Vector3 _baseScale;
    private Coroutine _scaleCoroutine;

    private void Awake()
    {
        // On mémorise l'échelle initiale (celle du prefab / layout) pour pouvoir restaurer correctement.
        _baseScale = transform.localScale;
    }

    private void OnDisable()
    {
        // Sécurité : si l'objet est désactivé pendant un hover, on remet l'échelle de base.
        StopScaleCoroutineIfNeeded();
        transform.localScale = _baseScale;
    }

    public void Init(CardModel cardModel)
    {
        CardModel = cardModel;
        for (int i = 0; i < sideTexts.Length; i++)
        {
            sideTexts[i].text = CardModel.GetPowerBySide((CardModel.EPowerSide)i).ToString();
        }
        totalPowerText.text = CardModel.CumulativPower.ToString();

        SetSelected(false);
        // Met à jour le sprite à partir de CardData si disponible
        if (artworkImage != null && CardModel != null && CardModel.Data != null)
        {
            artworkImage.sprite = CardModel.Data.cardtexture;
        }

        // Si l'échelle a été modifiée par un layout/parent avant Init, on la prend comme base.
        _baseScale = transform.localScale;

        ChangeCardTeam();

        CardModel.OnPlayerChange += ChangeCardTeam;
    }

    public void ChangeCardTeam()
    {
        if (CardModel.IsPlayer1Card)
        {
            bgplayer1.gameObject.SetActive(true);
            bgplayer2.gameObject.SetActive(false);
        }
        else
        {
            bgplayer1.gameObject.SetActive(false);
            bgplayer2.gameObject.SetActive(true);
        }
    }

    public void SetSelected(bool isSelected)
    {
        if (selectionHighlight != null)
            selectionHighlight.SetActive(isSelected);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnCardClicked?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ApplyHoverScale(isHovered: true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ApplyHoverScale(isHovered: false);
    }

    private void ApplyHoverScale(bool isHovered)
    {
        Vector3 targetScale = isHovered ? (_baseScale * hoverScaleMultiplier) : _baseScale;

        if (!smoothHover)
        {
            StopScaleCoroutineIfNeeded();
            transform.localScale = targetScale;
            return;
        }

        StopScaleCoroutineIfNeeded();
        _scaleCoroutine = StartCoroutine(ScaleTo(targetScale, hoverSmoothDuration));
    }

    private IEnumerator ScaleTo(Vector3 targetScale, float duration)
    {
        Vector3 startScale = transform.localScale;
        if (duration <= 0f)
        {
            transform.localScale = targetScale;
            yield break;
        }

        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float alpha = Mathf.Clamp01(t / duration);
            transform.localScale = Vector3.Lerp(startScale, targetScale, alpha);
            yield return null;
        }

        transform.localScale = targetScale;
        _scaleCoroutine = null;
    }

    private void StopScaleCoroutineIfNeeded()
    {
        if (_scaleCoroutine == null)
            return;

        StopCoroutine(_scaleCoroutine);
        _scaleCoroutine = null;
    }

    private void OnDestroy()
    {
        if (CardModel != null)
            CardModel.OnPlayerChange -= ChangeCardTeam;
    }

    // Nouveau: FX quand la carte est posée sur le terrain
    public void PlayToBoardFx()
    {
        if (_fxAnimator == null)
            return;

        _fxAnimator.ResetTrigger(TriggerCaptureCard);
        _fxAnimator.ResetTrigger(TriggerToBoard);
        _fxAnimator.SetTrigger(TriggerToBoard);
    }

    // Nouveau: FX quand la carte est capturée
    public void PlayCaptureFx()
    {
        if (_fxAnimator == null)
            return;

        _fxAnimator.ResetTrigger(TriggerToBoard);
        _fxAnimator.ResetTrigger(TriggerCaptureCard);
        _fxAnimator.SetTrigger(TriggerCaptureCard);
    }
}
