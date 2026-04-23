using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFadeIn : MonoBehaviour
{
    [SerializeField] private float duration = 1f;

    private SpriteRenderer sr;
    private Material runtimeMat;

    // URP Lit/Unlit commonly expose Base Color as _BaseColor
    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
    // Fallback for some shaders/materials
    private static readonly int ColorId = Shader.PropertyToID("_Color");

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        // IMPORTANT: material creates an instance so we don't edit shared asset
        runtimeMat = sr.material;
    }

    void OnEnable()
    {
        StopAllCoroutines();
        SetAlpha(0f);
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float a = Mathf.Clamp01(t / duration);
            SetAlpha(a);
            yield return null;
        }

        SetAlpha(1f);
    }

    private void SetAlpha(float a)
    {
        if (runtimeMat == null) return;

        if (runtimeMat.HasProperty(BaseColorId))
        {
            Color c = runtimeMat.GetColor(BaseColorId);
            c.a = a;
            runtimeMat.SetColor(BaseColorId, c);
        }
        else if (runtimeMat.HasProperty(ColorId))
        {
            Color c = runtimeMat.GetColor(ColorId);
            c.a = a;
            runtimeMat.SetColor(ColorId, c);
        }
    }

    void OnDestroy()
    {
        // Clean up instantiated material
        if (runtimeMat != null)
            Destroy(runtimeMat);
    }
}