using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIAnimator : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private RectTransform _uiElement;

    [Header("Animation")]
    [SerializeField] private float _slideSpeed = 5f;


    [Header("Positions")]
    [SerializeField] private Vector2 _hiddenPosition;
    [SerializeField] private Vector2 _shownPosition;

    private Coroutine _currentAnimation;

    void Start()
    {
        if (_uiElement == null)
            _uiElement = GetComponent<RectTransform>();
        _uiElement.anchoredPosition = _hiddenPosition;
    }
    public void Show()
    {
        StartMove(_shownPosition);
    }

    public void Hide()
    {
        StartMove(_hiddenPosition);
    }

    private void StartMove(Vector2 target)
    {
        if (_currentAnimation != null)
            StopCoroutine(_currentAnimation);

        _currentAnimation = StartCoroutine(MoveTo(target));
    }

    IEnumerator MoveTo(Vector2 target)
    {
        while (Vector2.Distance(_uiElement.anchoredPosition, target) > 0.01f)
        {
            _uiElement.anchoredPosition = Vector2.Lerp(
                _uiElement.anchoredPosition,
                target,
                Time.deltaTime * _slideSpeed
            );

            yield return null;
        }

        _uiElement.anchoredPosition = target;
    }
}
