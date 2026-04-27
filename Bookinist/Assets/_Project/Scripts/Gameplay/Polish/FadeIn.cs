using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFadeIn : MonoBehaviour
{
    [SerializeField] private float _duration = 1f;

    private SpriteRenderer _sr;
    private Material _runtimeMat;

    // URP Lit/Unlit commonly expose Base Color as _BaseColor
    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
    // Fallback for some shaders/materials
    private static readonly int ColorId = Shader.PropertyToID("_Color");

    void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();

        // IMPORTANT: material creates an instance so we don't edit shared asset
        _runtimeMat = _sr.material;
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
        while (t < _duration)
        {
            t += Time.deltaTime;
            float a = Mathf.Clamp01(t / _duration);
            SetAlpha(a);
            yield return null;
        }

        SetAlpha(1f);
    }

    private void SetAlpha(float a)
    {
        if (_runtimeMat == null) return;

        if (_runtimeMat.HasProperty(BaseColorId))
        {
            Color c = _runtimeMat.GetColor(BaseColorId);
            c.a = a;
            _runtimeMat.SetColor(BaseColorId, c);
        }
        else if (_runtimeMat.HasProperty(ColorId))
        {
            Color c = _runtimeMat.GetColor(ColorId);
            c.a = a;
            _runtimeMat.SetColor(ColorId, c);
        }
    }

    void OnDestroy()
    {
        // Clean up instantiated material
        if (_runtimeMat != null)
            Destroy(_runtimeMat);
    }
}