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
    [SerializeField] private RectTransform _palierFillBar;  
    [SerializeField] private RectTransform _profileFillBar;  
    [SerializeField] private RectTransform _backgroundBar;
    [SerializeField] private Image _glowImage;
    [SerializeField] private Material _liquidMaterial;

    private float _displayedXP;
    private Tween _currentTween;

    private Image _palierImage;
    private Image _profileImage;

    void Start()
    {
        _palierImage = _palierFillBar.GetComponent<Image>();
        _profileImage = _profileFillBar.GetComponent<Image>();

        _palierFillBar.pivot = new Vector2(0.5f, 0f);
        _palierFillBar.sizeDelta = new Vector2(_backgroundBar.rect.width, _backgroundBar.rect.height);

        _liquidMaterial = Instantiate(_liquidMaterial);
        _palierImage.material = _liquidMaterial;

        _displayedXP = curXP;

        RefreshBarUI();
    }

    public void RefreshBarUI()
    {
        float ratio = Mathf.Clamp01(_displayedXP / maxXP);

        _palierImage.fillAmount = 1f; 
        _liquidMaterial.SetFloat("_Fill", ratio);

        if (_profileImage != null)
        {
            _profileImage.fillAmount = ratio;
        }

        UpdateGlow(ratio);
    }

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

    void PlayGameFeel(int value)
    {
        _palierFillBar.DOKill();

        _palierFillBar.DOPunchScale(new Vector3(0.03f, 0.05f, 0f), 0.15f);

        if (value > 20)
        {
            _palierFillBar.DOShakeAnchorPos(0.2f, 4f, 12, 90f);
        }
    }

    void UpdateGlow(float ratio)
    {
        if (_glowImage == null) return;

        Color baseColor = new Color(1f, 1f, 1f, 0f);
        Color fullGlow = new Color(1f, 0.6f, 0.2f, 0.6f);

        _glowImage.color = Color.Lerp(baseColor, fullGlow, ratio);
    }
}