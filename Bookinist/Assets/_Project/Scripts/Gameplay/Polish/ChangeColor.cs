using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    [Header("Renderers")]
    [SerializeField] private bool _autoGetChildrenSprites = true;
    [SerializeField] private List<SpriteRenderer> _spriteRenderers = new();

    [Header("Color")]
    [SerializeField] private Color _defaultColor = Color.white;
    [SerializeField] private bool _useCurrentColorAsDefaultAtStart = true;

    [Header("Timing")]
    [SerializeField] private float _defaultDuration = 0.2f;
    [SerializeField] private bool _useUnscaledTime = false;

    private Coroutine _colorRoutine;

    private void Awake()
    {
        if (_autoGetChildrenSprites)
            AutoFillSpriteRenderers();
        else
            CleanRendererList();
    }

    private void Start()
    {
        if (_spriteRenderers.Count == 0)
            return;

        if (_useCurrentColorAsDefaultAtStart)
            _defaultColor = _spriteRenderers[0].color;
        else
            SetColorImmediate(_defaultColor);
    }

    public void SetColorImmediate(Color newColor)
    {
        StopColorRoutine();

        for (int i = 0; i < _spriteRenderers.Count; i++)
        {
            if (_spriteRenderers[i] == null)
                continue;

            _spriteRenderers[i].color = newColor;
        }
    }

    public void ChangeObjColor(Color targetColor)
    {
        ChangeObjColor(targetColor, _defaultDuration);
    }

    public void ChangeObjColor(Color targetColor, float duration)
    {
        StopColorRoutine();
        _colorRoutine = StartCoroutine(ChangeColorRoutine(GetCurrentColor(), targetColor, duration));
    }

    public void PingPongColor(Color targetColor)
    {
        PingPongColor(targetColor, _defaultDuration);
    }

    public void PingPongColor(Color targetColor, float duration)
    {
        PingPongColor(targetColor, duration, duration, 0f);
    }

    public void PingPongColor(Color targetColor, float forwardDuration, float backwardDuration, float holdOnTarget)
    {
        StopColorRoutine();
        _colorRoutine = StartCoroutine(PingPongColorRoutine(GetCurrentColor(), targetColor, forwardDuration, backwardDuration, holdOnTarget));
    }

    public void ResetToDefaultColor()
    {
        ChangeObjColor(_defaultColor, _defaultDuration);
    }

    public void ResetToDefaultColorImmediate()
    {
        SetColorImmediate(_defaultColor);
    }

    public Color GetCurrentColor()
    {
        for (int i = 0; i < _spriteRenderers.Count; i++)
        {
            if (_spriteRenderers[i] != null)
                return _spriteRenderers[i].color;
        }

        return Color.white;
    }

    public void SetDefaultColorToCurrent()
    {
        _defaultColor = GetCurrentColor();
    }

    public void SetDefaultColor(Color newDefaultColor)
    {
        _defaultColor = newDefaultColor;
    }

    public void StopColor()
    {
        StopColorRoutine();
    }

    private IEnumerator ChangeColorRoutine(Color startColor, Color targetColor, float duration)
    {
        if (duration <= 0f)
        {
            SetColorImmediate(targetColor);
            _colorRoutine = null;
            yield break;
        }

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += GetDeltaTime();
            float t = Mathf.Clamp01(elapsed / duration);
            ApplyColor(Color.Lerp(startColor, targetColor, t));
            yield return null;
        }

        ApplyColor(targetColor);
        _colorRoutine = null;
    }

    private IEnumerator PingPongColorRoutine(Color startColor, Color targetColor, float forwardDuration, float backwardDuration, float holdOnTarget)
    {
        yield return ChangeColorRoutine(startColor, targetColor, forwardDuration);

        if (holdOnTarget > 0f)
        {
            float elapsedHold = 0f;

            while (elapsedHold < holdOnTarget)
            {
                elapsedHold += GetDeltaTime();
                yield return null;
            }
        }

        yield return ChangeColorRoutine(targetColor, startColor, backwardDuration);
        _colorRoutine = null;
    }

    private void ApplyColor(Color newColor)
    {
        for (int i = 0; i < _spriteRenderers.Count; i++)
        {
            if (_spriteRenderers[i] == null)
                continue;

            _spriteRenderers[i].color = newColor;
        }
    }

    private float GetDeltaTime()
    {
        return _useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
    }

    private void StopColorRoutine()
    {
        if (_colorRoutine != null)
        {
            StopCoroutine(_colorRoutine);
            _colorRoutine = null;
        }
    }

    public void AutoFillSpriteRenderers()
    {
        _spriteRenderers.Clear();

        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>(true);
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] != null)
                _spriteRenderers.Add(renderers[i]);
        }
    }

    private void CleanRendererList()
    {
        for (int i = _spriteRenderers.Count - 1; i >= 0; i--)
        {
            if (_spriteRenderers[i] == null)
                _spriteRenderers.RemoveAt(i);
        }
    }

    [ContextMenu("Refresh Child Sprite Renderers")]
    private void RefreshChildSpriteRenderers()
    {
        AutoFillSpriteRenderers();
    }
}