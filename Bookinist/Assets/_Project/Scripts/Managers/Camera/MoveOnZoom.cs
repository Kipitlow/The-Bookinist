using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveObject))]
public class MoveOnZoom : MonoBehaviour
{
    #region variables

    public enum ZoomState
    {
        Starting,
        Visible,
        Hidden,
        Showing,
        Hiding
    }

    [Header("Zoom Filter")]
    [SerializeField] private int _myLayer;
    [SerializeField] private int _lateral;

    [Header("Hide Movement")]
    [SerializeField] private Vector2 _hideDirection = Vector2.right;
    [SerializeField] private float _hideDistance = 2f;
    [SerializeField] private float _startOffsetDistance = 30f;

    [Header("Movement")]
    [SerializeField] private float _smoothTime = 0.2f;

    [Header("Visual")]
    [SerializeField] private float _alphaVisible = 1f;
    [SerializeField] private float _alphaHidden = 0f;
    [SerializeField] private float _alphaSpeed = 1f;

    [Header("Sprite Renderers")]
    [SerializeField] private bool _autoGetChildrenSprites = true;
    [SerializeField] private List<GameObject> _objToDesable = new();

    private List<SpriteRenderer> _spriteRenderers = new();
    private float _currentAlpha;
    private bool _isVisible;

    private CameraMovement _camMovement;
    private MoveObject _moveObject;

    public ZoomState _state = ZoomState.Visible;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        _moveObject = GetComponent<MoveObject>();

        Camera mainCam = Camera.main;
        if (mainCam != null)
            _camMovement = mainCam.GetComponent<CameraMovement>();
    }


    private void Start()
    {
        if (_autoGetChildrenSprites)
            AutoFillSpriteRenderers();
        else
            CleanRendererList();

        _alphaVisible = Mathf.Clamp01(_alphaVisible);
        _alphaHidden = Mathf.Clamp01(_alphaHidden);

        _currentAlpha = 0f;
        _state = ZoomState.Starting;
        _isVisible = false;

        ApplyAlphaToAllRenderers(_currentAlpha);

        StartShowingAtBeginning();
    }

    private void OnEnable()
    {
        if (_camMovement != null)
            _camMovement.OnZoom += OnChangingLayer;
    }

    private void OnDisable()
    {
        if (_camMovement != null)
            _camMovement.OnZoom -= OnChangingLayer;
    }

    private void Update()
    {
        UpdateStateFromMovement();
        UpdateVisual();
    }

    #endregion

    public void SetIndexs(int myLayer, int Lateral)
    {
        _myLayer = myLayer;
        _lateral = Lateral;
    }

    public void OnChangingLayer(int layer, int lateralIndex)
    {
        if (_state == ZoomState.Starting)
        {
            StartShowingAtBeginning();
            return;
        }

        if (layer == _myLayer && !_isVisible)
        {
            if (lateralIndex == _lateral)
                StartShowing();
            else
                QuickShowing();
        }
        else if (layer == _myLayer + 1)
        {
            if (lateralIndex == _lateral)
                StartHiding();
            else
                QuickHiding();
        }
    }

    private void UpdateStateFromMovement()
    {
        if (_state == ZoomState.Showing && _moveObject.IsAtPosition(GetCurrentBasePosition()))
        {
            _isVisible = true;
            _state = ZoomState.Visible;
        }
        else if (_state == ZoomState.Hiding && _moveObject.IsAtPosition(GetHiddenPosition()))
        {
            _isVisible = false;
            _state = ZoomState.Hidden;
        }
    }

    private Vector3 GetCurrentBasePosition()
    {
        return _moveObject._baseLocalPosition;
    }

    private Vector3 GetHiddenPosition()
    {
        Vector2 direction = _hideDirection.normalized;
        Vector3 basePos = GetCurrentBasePosition();

        return basePos + new Vector3(
            direction.x * _hideDistance,
            direction.y * _hideDistance,
            0f
        );
    }

    private Vector3 GetStartingPosition()
    {
        Vector2 direction = _hideDirection.normalized;
        Vector3 basePos = GetCurrentBasePosition();

        return basePos + new Vector3(
            direction.x * _startOffsetDistance,
            direction.y * _startOffsetDistance,
            0f
        );
    }

    private void StartShowingAtBeginning()
    {
        _moveObject.SetPositionImmediate(GetStartingPosition());
        _state = ZoomState.Showing;
        _currentAlpha = 0f;
        ApplyAlphaToAllRenderers(_currentAlpha);
        _moveObject.MoveTo(GetCurrentBasePosition(), _smoothTime);
    }

    private void StartShowing()
    {
        _state = ZoomState.Showing;
        _moveObject.MoveTo(GetCurrentBasePosition(), _smoothTime);

        foreach (GameObject obj in _objToDesable)
        {
            obj.SetActive(true);
        }

        if (_currentAlpha < 0.5f)
            _currentAlpha = 0.7f;
    }

    private void StartHiding()
    {
        _state = ZoomState.Hiding;
        _moveObject.MoveTo(GetHiddenPosition(), _smoothTime);

        foreach (GameObject obj in _objToDesable)
        {
            obj.SetActive(true);
        }
    }

    private void QuickShowing()
    {
        _state = ZoomState.Showing;
        _moveObject.MoveTo(GetCurrentBasePosition(), 0f);
    }

    private void QuickHiding()
    {
        _state = ZoomState.Hiding;
        _moveObject.MoveTo(GetHiddenPosition(), 0f);
    }

    private void UpdateVisual()
    {
        float targetAlpha = _currentAlpha;

        switch (_state)
        {
            case ZoomState.Visible:
            case ZoomState.Showing:
                targetAlpha = _alphaVisible;
                break;

            case ZoomState.Hidden:
            case ZoomState.Hiding:
            case ZoomState.Starting:
                targetAlpha = _alphaHidden;
                break;
        }

        _currentAlpha = Mathf.MoveTowards(_currentAlpha, targetAlpha, _alphaSpeed * Time.deltaTime);
        ApplyAlphaToAllRenderers(_currentAlpha);
    }

    private void ApplyAlphaToAllRenderers(float alpha)
    {
        alpha = Mathf.Clamp01(alpha);

        for (int i = 0; i < _spriteRenderers.Count; i++)
        {
            SpriteRenderer sr = _spriteRenderers[i];

            if (sr == null)
                continue;

            Color color = sr.color;
            color.a = alpha;
            sr.color = color;
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

    [ContextMenu("Reset To Visible")]
    public void ResetToVisible()
    {
        _state = ZoomState.Visible;
        _isVisible = true;
        _moveObject.SetPositionImmediate(GetCurrentBasePosition());
        SetAlphaImmediate(_alphaVisible);
    }

    [ContextMenu("Reset To Hidden")]
    public void ResetToHidden()
    {
        _state = ZoomState.Hidden;
        _isVisible = false;
        _moveObject.SetPositionImmediate(GetHiddenPosition());
        SetAlphaImmediate(_alphaHidden);
    }

    private void SetAlphaImmediate(float alpha)
    {
        _currentAlpha = Mathf.Clamp01(alpha);
        ApplyAlphaToAllRenderers(_currentAlpha);
    }
}