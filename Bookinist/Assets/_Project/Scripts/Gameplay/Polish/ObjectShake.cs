// ObjectShake.cs
using System.Collections;
using UnityEngine;

public class ObjectShake : MonoBehaviour
{
    #region Variables
    [SerializeField] private float _defaultDuration = 0.15f;
    [SerializeField] private float _defaultMagnitude = 0.05f;
    [SerializeField] private AnimationCurve _falloff = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

    private Coroutine _shakeRoutine;
    private Transform _target;
    #endregion

    #region UnityMethods
    private void Awake()
    {
        if (_target == null)
            _target = transform;
    }
    #endregion

    #region Methods
    public void Shake()
    {
        Shake(_defaultDuration, _defaultMagnitude);
    }

    public void Shake(float duration, float magnitude)
    {
        if (_shakeRoutine != null)
            StopCoroutine(_shakeRoutine);

        _shakeRoutine = StartCoroutine(ShakeRoutine(duration, magnitude));
    }

    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        Vector3 startLocalPosition = _target.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = duration <= 0f ? 1f : elapsed / duration;
            float strength = _falloff != null ? _falloff.Evaluate(t) : 1f - t;

            Vector2 random = Random.insideUnitCircle * magnitude * strength;
            _target.localPosition = startLocalPosition + new Vector3(random.x, random.y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        _target.localPosition = startLocalPosition;
        _shakeRoutine = null;
    }
    #endregion
}