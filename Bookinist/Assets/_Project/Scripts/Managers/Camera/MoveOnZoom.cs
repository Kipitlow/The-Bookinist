using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(MoveObject))]
public class MoveOnZoom : MonoBehaviour
{
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
    private bool _isVisible = false;

    [Header("Visual")]
    [SerializeField] private float _alphaVisible = 1f;
    [SerializeField] private float _alphaHidden = 0f;
    [SerializeField] private float _alphaSpeed = 1f;

    private float _currentAlpha;

    private CameraMovement _camMovement;
    private SpriteRenderer _spriteRenderer;
    private MoveObject _moveObject;

    private Vector3 _hiddenLocalPosition;
    private Vector3 _startingLocalPosition;

    public ZoomState _state = ZoomState.Visible;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _moveObject = GetComponent<MoveObject>();

        Camera mainCam = Camera.main;
        if (mainCam != null)
            _camMovement = mainCam.GetComponent<CameraMovement>();


        Vector2 direction = _hideDirection.normalized;

        _hiddenLocalPosition = _moveObject._baseLocalPosition + new Vector3(
            direction.x * _hideDistance,
            direction.y * _hideDistance,
            0f
        );

        _startingLocalPosition = _moveObject._baseLocalPosition + new Vector3(
            direction.x * _startOffsetDistance,
            direction.y * _startOffsetDistance,
            0f
        );

        _alphaVisible = Mathf.Clamp01(_alphaVisible);
        _alphaHidden = Mathf.Clamp01(_alphaHidden);

        _currentAlpha = 0f;
        _state = ZoomState.Starting;


        Color color = _spriteRenderer.color;
        color.a = _currentAlpha;
        _spriteRenderer.color = color;
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

    private void StartShowingAtBeginning()
    {
        _moveObject.SetPositionImmediate(new Vector3(_moveObject._baseLocalPosition.x + (30 * _hideDirection.x), _moveObject._baseLocalPosition.y, _moveObject._baseLocalPosition.z));
        _state = ZoomState.Showing;
        _currentAlpha = 0f;
        _moveObject.MoveTo(_moveObject._baseLocalPosition, 0.2f);
    }

    private void StartShowing()
    {
        _state = ZoomState.Showing;
        _moveObject.MoveTo(_moveObject._baseLocalPosition, _smoothTime);

        if (_currentAlpha < 0.5f)
            _currentAlpha = 0.7f;
    }

    private void StartHiding()
    {
        _state = ZoomState.Hiding;
        _moveObject.MoveTo(_hiddenLocalPosition, _smoothTime);
    }

    private void QuickShowing()
    {
        _state = ZoomState.Showing;
        _moveObject.MoveTo(_moveObject._baseLocalPosition, 0f);
    }

    private void QuickHiding()
    {
        _state = ZoomState.Hiding;
        _moveObject.MoveTo(_hiddenLocalPosition, 0f);
    }

    private void UpdateStateFromMovement()
    {
        if (_state == ZoomState.Showing && _moveObject.IsAtPosition(_moveObject._baseLocalPosition))
        {
            _isVisible = true;
            _state = ZoomState.Visible;
        }
        else if (_state == ZoomState.Hiding && _moveObject.IsAtPosition(_hiddenLocalPosition))
        {
            _isVisible = false;
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
            case ZoomState.Starting:
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
        _moveObject.SetPositionImmediate(_moveObject._baseLocalPosition);
        SetAlphaImmediate(_alphaVisible);
    }

    [ContextMenu("Reset To Hidden")]
    public void ResetToHidden()
    {
        _state = ZoomState.Hidden;
        _moveObject.SetPositionImmediate(_hiddenLocalPosition);
        SetAlphaImmediate(_alphaHidden);
    }

    private void SetAlphaImmediate(float alpha)
    {
        _currentAlpha = Mathf.Clamp01(alpha);

        Color color = _spriteRenderer.color;
        color.a = _currentAlpha;
        _spriteRenderer.color = color;
    }
}