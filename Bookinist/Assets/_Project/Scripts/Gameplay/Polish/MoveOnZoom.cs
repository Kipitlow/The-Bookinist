// MoveOnZoom.cs
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveObject))]
public class MoveOnZoom : MonoBehaviour
{
    #region Variables
    public enum ZoomState
    {
        Starting,
        Visible,
        Hidden,
        Showing,
        Hiding,
        Shaking
    }

    [Header("Zoom Filter")]
    [SerializeField] private int _myLayer = 0;
    [SerializeField] private int _lateral;

    [Header("Hide Movement")]
    [SerializeField] private Vector2 _hideDirection = Vector2.right;
    [SerializeField] private float _hideDistance = 2f;
    [SerializeField] private float _startOffsetDistance = 30f;

    [Header("Movement")]
    [SerializeField] private float _smoothTime = 0.2f;
    [SerializeField] private bool _playStartShowAnimation = true;

    [Header("Visual")]
    [SerializeField] private float _alphaVisible = 1f;
    [SerializeField] private float _alphaHidden = 0f;
    [SerializeField] private float _alphaSpeed = 1f;

    [Header("Objects")]
    [SerializeField] private bool _autoGetChildrenSprites = true;
    [SerializeField] private bool _autoGetChildrenColliders = true;
    [SerializeField] private List<GameObject> _objToDesable = new();
    [SerializeField] private List<GameObject> _childs = new();

    private readonly List<SpriteRenderer> _spriteRenderers = new();
    private float _currentAlpha;

    private CameraMovement _camMovement;
    private MoveObject _moveObject;

    public ZoomState _state = ZoomState.Visible;
    public ZoomState State => _state;
    #endregion

    #region UnityMethods
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

        if (_autoGetChildrenColliders)
            AutoFillChilds();

        _moveObject.UpdateBasePos();

        _alphaVisible = Mathf.Clamp01(_alphaVisible);
        _alphaHidden = Mathf.Clamp01(_alphaHidden);

        if (_playStartShowAnimation)
        {
            _currentAlpha = 0f;
            _state = ZoomState.Starting;
            ApplyAlphaToAllRenderers(_currentAlpha);
            StartShowingAtBeginning();
        }
        else
        {
            _state = ZoomState.Visible;
            _moveObject.SetPositionImmediate(_moveObject._baseLocalPosition);
            _currentAlpha = _alphaVisible;
            ApplyAlphaToAllRenderers(_currentAlpha);
        }
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

    #region Methods

    #region Sets
    public void SetIndexs(int myLayer, int lateral)
    {
        _myLayer = myLayer;
        _lateral = lateral;
    }
    public void SetObjectEnable(bool enableState)
    {
        for (int i = 0; i < _objToDesable.Count; i++)
        {
            if (_objToDesable[i] != null)
                _objToDesable[i].SetActive(enableState);
        }
    }

    public void SetObjCollier(bool enableState)
    {
        for (int i = 0; i < _childs.Count; i++)
        {
            if (_childs[i] == null)
                continue;

            Collider[] colliders = _childs[i].GetComponents<Collider>();
            for (int j = 0; j < colliders.Length; j++)
                colliders[j].enabled = enableState;
        }

        Collider[] selfColliders = GetComponents<Collider>();
        for (int i = 0; i < selfColliders.Length; i++)
            selfColliders[i].enabled = enableState;
    }
    #endregion

    #region Gets

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

    public int GetLayer()
    {
        return _myLayer;
    }
    #endregion

    #region changing Layer Handler
    public void OnChangingLayer(int layer, int lateralIndex)
    {
        if (_state == ZoomState.Starting)
        {
            StartShowingAtBeginning();
            return;
        }

        if (layer == _myLayer)
        {
            if (_state == ZoomState.Visible || _state == ZoomState.Showing)
                return;

            if (lateralIndex == _lateral)
                StartShowing();
            else
                QuickShowing();
        }
        else if (layer == _myLayer + 1)
        {
            if (_state == ZoomState.Hidden || _state == ZoomState.Hiding)
                return;

            if (lateralIndex == _lateral)
                StartHiding();
            else
                QuickHiding();
        }
    }
    #endregion

    #region update State
    private void UpdateStateFromMovement()
    {
        if (_state == ZoomState.Showing && _moveObject.IsAtPosition(GetCurrentBasePosition()))
            _state = ZoomState.Visible;
        else if (_state == ZoomState.Hiding && _moveObject.IsAtPosition(GetHiddenPosition()))
            _state = ZoomState.Hidden;
    }
    #endregion

    #region Showing / hiding
    private void StartShowingAtBeginning()
    {
        _moveObject.SetPositionImmediate(GetStartingPosition());
        _state = ZoomState.Showing;
        _currentAlpha = 0f;
        ApplyAlphaToAllRenderers(_currentAlpha);
        _moveObject.MoveTo(GetCurrentBasePosition(), _smoothTime, false);
    }

    private void StartShowing()
    {
        _state = ZoomState.Showing;
        _moveObject.MoveTo(GetCurrentBasePosition(), _smoothTime, false);
        SetObjectEnable(true);
        SetObjCollier(true);

        if (_currentAlpha < 0.5f)
            _currentAlpha = 0.7f;
    }

    private void StartHiding()
    {
        _state = ZoomState.Hiding;
        _moveObject.MoveTo(GetHiddenPosition(), _smoothTime, false);
        SetObjectEnable(false);
        SetObjCollier(false);
    }

    private void QuickShowing()
    {
        _state = ZoomState.Showing;
        _moveObject.MoveTo(GetCurrentBasePosition(), 0f, false);
        SetObjectEnable(true);
        SetObjCollier(true);
    }

    private void QuickHiding()
    {
        _state = ZoomState.Hiding;
        _moveObject.MoveTo(GetHiddenPosition(), 0f, false);
        SetObjectEnable(false);
        SetObjCollier(false);
    }
    #endregion

    #region Update Visuals
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
    #endregion

    #region childs
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

    public void AutoFillChilds()
    {
        _childs.Clear();

        Transform[] transforms = GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < transforms.Length; i++)
        {
            if (transforms[i] != null && transforms[i] != transform)
                _childs.Add(transforms[i].gameObject);
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
    #endregion

    #region Menu
    [ContextMenu("Refresh Child Sprite Renderers")]
    private void RefreshChildSpriteRenderers()
    {
        AutoFillSpriteRenderers();
    }

    [ContextMenu("Refresh Child Objects")]
    private void RefreshChildObjects()
    {
        AutoFillChilds();
    }

    [ContextMenu("Reset To Visible")]
    public void ResetToVisible()
    {
        _state = ZoomState.Visible;
        _moveObject.SetPositionImmediate(GetCurrentBasePosition());
        SetAlphaImmediate(_alphaVisible);
        SetObjectEnable(true);
        SetObjCollier(true);
    }

    [ContextMenu("Reset To Hidden")]
    public void ResetToHidden()
    {
        _state = ZoomState.Hidden;
        _moveObject.SetPositionImmediate(GetHiddenPosition());
        SetAlphaImmediate(_alphaHidden);
        SetObjectEnable(false);
        SetObjCollier(false);
    }

    private void SetAlphaImmediate(float alpha)
    {
        _currentAlpha = Mathf.Clamp01(alpha);
        ApplyAlphaToAllRenderers(_currentAlpha);
    }
    #endregion

    #endregion
}