using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BarFill : MonoBehaviour
{
    [Header("Values")]
    public float curXP;
    public float maxXP = 100f;

    [Header("Visual")]
    public float animDuration = 0.4f;

    [Header("Refs")]
    [SerializeField] private RectTransform _fillBar;
    [SerializeField] private RectTransform _backgroundBar;
    [SerializeField] private Image _glowImage;
    [SerializeField] private Material _liquidMaterial;

    private float _displayedXP;
    private Tween _currentTween;

    private Vector2 _basePos;
    private float _waveOffset;

    void Start()
    {
        _fillBar.pivot = new Vector2(0.5f, 0f);

        _basePos = _fillBar.anchoredPosition;

        RectTransform rt = _fillBar;

        rt.sizeDelta = new Vector2(_backgroundBar.rect.width, _backgroundBar.rect.height);

        _displayedXP = curXP;

        _liquidMaterial = Instantiate(_liquidMaterial);
        _fillBar.GetComponent<Image>().material = _liquidMaterial;

        RefreshBarUI();
    }

    public void RefreshBarUI()
    {
        float ratio = _displayedXP / maxXP;

        _fillBar.GetComponent<Image>().fillAmount = 1f;

        UpdateGlow(ratio);

        _liquidMaterial.SetFloat("_Fill", ratio);
    }

    // -----------------------
    // XP MODIFICATION
    // -----------------------

    public void ModifCur(int newVal)
    {
        float targetXP = Mathf.Clamp(curXP + newVal, 0, maxXP);
        curXP = targetXP;

        _currentTween?.Kill();

        _currentTween = DOTween.To(
            () => _displayedXP,
            x =>
            {
                _displayedXP = x;
                RefreshBarUI();
            },
            targetXP,
            animDuration
        )
        .SetEase(Ease.OutCubic)
        .OnComplete(() => _displayedXP = targetXP);

        PlayGameFeel(newVal);
    }

    // -----------------------
    // GAME FEEL
    // -----------------------

    void PlayGameFeel(int value)
    {
        _fillBar.DOKill();
        // petit punch toujours
        _fillBar.DOPunchScale(new Vector3(0.03f, 0.05f, 0f), 0.15f);

        // shake si gros gain
        if (value > 20)
        {
            _fillBar.DOShakeAnchorPos(0.2f, 4f, 12, 90f);
        }
    }

    // -----------------------
    // GLOW SYSTEM
    // -----------------------

    void UpdateGlow(float ratio)
    {
        if (_glowImage == null) return;

        Color baseColor = new Color(1f, 1f, 1f, 0f);
        Color fullGlow = new Color(1f, 0.6f, 0.2f, 0.6f);

        _glowImage.color = Color.Lerp(baseColor, fullGlow, ratio);
    }
}