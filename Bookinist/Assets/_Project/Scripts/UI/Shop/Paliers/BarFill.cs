using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BarFill : MonoBehaviour
{
    [Header("Values")]
    public float curXP;
    public float maxXP = 100f;

    [Header("Visual")]
    public float Width;
    public float animDuration = 0.4f;
    [Range(0f, 0.3f)]
    [SerializeField] private float waveAmplification = 0.015f;

    [Header("Refs")]
    [SerializeField] private RectTransform FillBar;
    [SerializeField] private RectTransform backgroundBar;
    [SerializeField] private Image glowImage;
    [SerializeField] private Material liquidMaterial;

    private float displayedXP;
    private Tween currentTween;

    private Vector2 basePos;
    private float waveOffset;

    void Start()
    {
        FillBar.pivot = new Vector2(0.5f, 0f);

        basePos = FillBar.anchoredPosition;

        RectTransform rt = FillBar;

        rt.sizeDelta = new Vector2(backgroundBar.rect.width, backgroundBar.rect.height);

        displayedXP = curXP;

        liquidMaterial = Instantiate(liquidMaterial);
        FillBar.GetComponent<Image>().material = liquidMaterial;

        RefreshBarUI();
    }

    void Update()
    {
        //ApplyLiquidEffect();
    }

    // -----------------------
    // CORE UI UPDATE
    // -----------------------

    public void RefreshBarUI()
    {
        float ratio = displayedXP / maxXP;

        FillBar.GetComponent<Image>().fillAmount = 1f;

        UpdateGlow(ratio);

        liquidMaterial.SetFloat("_Fill", ratio);
    }

    // -----------------------
    // XP MODIFICATION
    // -----------------------

    public void ModifCur(int newVal)
    {
        float targetXP = Mathf.Clamp(curXP + newVal, 0, maxXP);
        curXP = targetXP;

        currentTween?.Kill();

        currentTween = DOTween.To(
            () => displayedXP,
            x =>
            {
                displayedXP = x;
                RefreshBarUI();
            },
            targetXP,
            animDuration
        )
        .SetEase(Ease.OutCubic)
        .OnComplete(() => displayedXP = targetXP);

        PlayGameFeel(newVal);
    }

    // -----------------------
    // GAME FEEL
    // -----------------------

    void PlayGameFeel(int value)
    {
        FillBar.DOKill();
        // petit punch toujours
        FillBar.DOPunchScale(new Vector3(0.03f, 0.05f, 0f), 0.15f);

        // shake si gros gain
        if (value > 20)
        {
            FillBar.DOShakeAnchorPos(0.2f, 4f, 12, 90f);
        }
    }

    // -----------------------
    // LIQUID EFFECT
    // -----------------------

    void ApplyLiquidEffect()
    {
        waveOffset += Time.deltaTime * 4f;

        float wave = Mathf.Sin(waveOffset) * 1.5f;

        // léger mouvement vertical
        //FillBar.anchoredPosition = basePos + new Vector2(0f, wave);

        // léger stretch pour effet liquide
        FillBar.localScale = new Vector3(1f, 1f + Mathf.Sin(waveOffset) * waveAmplification, 1f);
    }

    // -----------------------
    // GLOW SYSTEM
    // -----------------------

    void UpdateGlow(float ratio)
    {
        if (glowImage == null) return;

        Color baseColor = new Color(1f, 1f, 1f, 0f);
        Color fullGlow = new Color(1f, 0.6f, 0.2f, 0.6f);

        glowImage.color = Color.Lerp(baseColor, fullGlow, ratio);
    }
}