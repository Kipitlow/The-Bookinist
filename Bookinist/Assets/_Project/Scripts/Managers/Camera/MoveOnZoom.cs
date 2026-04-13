using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MoveOnZoom : MonoBehaviour
{
    private enum ZoomState
    {
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

    [Header("Movement")]
    [SerializeField] private float _smoothTime = 0.2f;
    [SerializeField] private float _arrivalThreshold = 0.01f;
    private float _actualSmoothingTime;

    [SerializeField] private float _alphaVisible = 1f;
    [SerializeField] private float _alphaHidden = 0f;
    [SerializeField] private float _alphaSpeed = 1f;

    private float _currentAlpha;

    private CameraMovement _camMovement;
    private SpriteRenderer _spriteRenderer;

    private Vector3 _baseLocalPosition;
    private Vector3 _hiddenLocalPosition;
    private Vector3 _targetLocalPosition;
    private Vector3 _velocity;

    private ZoomState _state = ZoomState.Visible;

    private float _h;
    private float _s;
    private float _v;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        Camera mainCam = Camera.main;
        _camMovement = mainCam.GetComponent<CameraMovement>();

        _baseLocalPosition = transform.localPosition;

        Vector2 direction = _hideDirection.normalized;
        _hiddenLocalPosition = _baseLocalPosition + new Vector3(
            direction.x * _hideDistance,
            direction.y * _hideDistance,
            0f
        );

        _targetLocalPosition = _baseLocalPosition;

        _currentAlpha = _spriteRenderer.color.a;

        _alphaVisible = Mathf.Clamp01(_alphaVisible);
        _alphaHidden = Mathf.Clamp01(_alphaHidden);

    }

    private void OnEnable()
    {
        _camMovement.OnZoom += OnChangingLayer;
    }

    private void OnDisable()
    {
        _camMovement.OnZoom -= OnChangingLayer;
    }

    private void Update()
    {
        UpdateMovement();
        UpdateVisual();
    }

    public void OnChangingLayer(int layer, int lateralIndex)
    {
        if (layer == _myLayer)
        {
            if (lateralIndex == _lateral) StartShowing();
            if (lateralIndex != _lateral) QuickShowing();
        }
        else if (layer == _myLayer + 1)
        {
            if (lateralIndex == _lateral) StartHiding();
            if (lateralIndex != _lateral) QuickHiding();
        }
    }

    private void StartShowing()
    {
        _state = ZoomState.Showing;
        _targetLocalPosition = _baseLocalPosition;
        _actualSmoothingTime = _smoothTime;
        if (_currentAlpha < 0.5f) _currentAlpha = 0.5f;
    }

    private void StartHiding()
    {
        _state = ZoomState.Hiding;
        _targetLocalPosition = _hiddenLocalPosition;
        _actualSmoothingTime = _smoothTime;
    }

    private void QuickShowing()
    {
        _state = ZoomState.Showing;
        _targetLocalPosition = _baseLocalPosition;
        _actualSmoothingTime = 0f;
    }

    private void QuickHiding()
    {
        _state = ZoomState.Hiding;
        _targetLocalPosition = _hiddenLocalPosition;
        _actualSmoothingTime = 0f;
    }

    private void UpdateMovement()
    {
        if (_state != ZoomState.Showing && _state != ZoomState.Hiding)
            return;

        transform.localPosition = Vector3.SmoothDamp( transform.localPosition,_targetLocalPosition, ref _velocity, _actualSmoothingTime);

        if (Vector3.Distance(transform.localPosition, _targetLocalPosition) <= _arrivalThreshold)
        {
            transform.localPosition = _targetLocalPosition;
            _velocity = Vector3.zero;

            if (_state == ZoomState.Showing)
                _state = ZoomState.Visible;
            else if (_state == ZoomState.Hiding)
                _state = ZoomState.Hidden;
        }
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
                targetAlpha = _alphaHidden;
                break;
        }

        _currentAlpha = Mathf.MoveTowards(_currentAlpha, targetAlpha, _alphaSpeed * Time.deltaTime);

        Color color = _spriteRenderer.color;
        color.a = _currentAlpha;
        _spriteRenderer.color = color;
    }

    [ContextMenu("Reset To Visible")]
    public void ResetToVisible()
    {
        _state = ZoomState.Visible;
        _targetLocalPosition = _baseLocalPosition;
        transform.localPosition = _baseLocalPosition;
        _velocity = Vector3.zero;
    }

    [ContextMenu("Reset To Hidden")]
    public void ResetToHidden()
    {
        _state = ZoomState.Hidden;
        _targetLocalPosition = _hiddenLocalPosition;
        transform.localPosition = _hiddenLocalPosition;
        _velocity = Vector3.zero;
    }
}