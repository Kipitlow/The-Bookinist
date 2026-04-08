using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

/// <summary>
/// Gestion des mouvements de la caméra : drag, tap, zoom, hold, navigation par snap points.
/// </summary>
public class CameraMovement : MonoBehaviour
{
    #region Variables

    [Header("Debug")]
    [SerializeField] private RayCastDebugger _raycastDebugger;

    [Header("Drag")]
    [SerializeField] private InputActionReference _dragDelta;
    [SerializeField] private InputActionReference _dragPress;
    [SerializeField] private float _dragSpeed = 0.01f;

    [Header("Tap")]
    [SerializeField] private float _tapMaxTime = 0.25f;
    [SerializeField] private float _tapMaxMovement = 10f;
    [SerializeField] private LayerMask _tapMask;
    [SerializeField] private float _tapRange = 100f;

    [Header("Zoom")]
    [SerializeField] private InputActionReference _scrollZoom;
    [SerializeField] private float _minZ;
    [SerializeField] private float _maxZ;

    [Header("Hold")]
    [SerializeField] private Camera _cam;
    [SerializeField] private GameObject _itemBase;
    [SerializeField] private float _holdMinTime = 0.5f;
    private bool _isHolding;

    [Header("Global Navigation")]
    public List<SnapPointManager> snapPointsManager = new();
    [SerializeField] private int _snapPointNumberOnOneLayer = 3;
    public int currentIndexLayer = 0;
    public int currentIndexByLayer;

    private float _previousPinchDistance;

    private bool _isPressing;
    private bool _isDragging;
    private float _pressStartTime;
    private Vector2 _pressStartPosition;

    private bool _stopZooming;
    private float _pinchThreshold = 30f;
    private float _zoomCooldown = 0.4f;
    private float _lastZoomTime = -999f;

    #endregion

    #region Unity Methods

    private void OnEnable()
    {
        _dragDelta.action.Enable();
        _dragPress.action.Enable();
        _scrollZoom.action.Enable();
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        _dragDelta.action.Disable();
        _dragPress.action.Disable();
        _scrollZoom.action.Disable();
        EnhancedTouchSupport.Disable();
    }

    private void Awake()
    {
        currentIndexByLayer = 1;
        _cam = Camera.main;
    }

    private void Start()
    {
        _maxZ = PageManager.maxLayer * (PageManager.layerSpread / 2);
    }

    private void Update()
    {
        HandleTapAndDrag();
        HandleZoom();
    }

    #endregion

    #region Methods

    private void HandleTapAndDrag()
    {
        if (Touch.activeTouches.Count > 1)
            return;

        bool pressed = _dragPress.action.IsPressed();

        if (pressed && !_isPressing)
        {
            _isPressing = true;
            _isDragging = false;
            _pressStartTime = Time.time;
            _pressStartPosition = GetPointerPosition();
        }

        if (pressed && _isPressing)
        {
            Vector2 delta = _dragDelta.action.ReadValue<Vector2>();

            if (!_isDragging && delta.magnitude > _tapMaxMovement)
            {
                _isDragging = true;
            }

            if (!_isDragging && !_isHolding)
            {
                float heldTime = Time.time - _pressStartTime;

                if (heldTime >= _holdMinTime)
                {
                    _isHolding = true;

                    PointerEventData pointerData = new(EventSystem.current)
                    {
                        position = GetPointerPosition()
                    };

                    List<RaycastResult> results = new();
                    EventSystem.current.RaycastAll(pointerData, results);

                    foreach (var result in results)
                    {
                        
                        if (result.gameObject.TryGetComponent<Button>(out var button))
                        {
                            Debug.Log("Hold detected on UI");

                            Instantiate(_itemBase, button.transform, false);
                            break;
                        }
                    }
                }
            }

            if (_isDragging)
            {
                // drag logic
            }
        }

        // Press released
        if (!pressed && _isPressing)
        {
            float pressDuration = Time.time - _pressStartTime;
            float movement = Vector2.Distance(_pressStartPosition, GetPointerPosition());

            _isPressing = false;

            if (_isHolding)
            {
                Debug.Log("Hold released");
            }
            else if (pressDuration <= _tapMaxTime && movement <= _tapMaxMovement)
            {
                // tap
            }
            else if (_isDragging)
            {
                if (GetPointerPosition().x < _pressStartPosition.x)
                {
                    currentIndexByLayer++;

                    if (currentIndexByLayer > snapPointsManager[currentIndexLayer].snapPoints.Length - 1)
                        currentIndexByLayer = snapPointsManager[currentIndexLayer].snapPoints.Length - 1;
                }
                else if (GetPointerPosition().x > _pressStartPosition.x)
                {
                    currentIndexByLayer--;

                    if (currentIndexByLayer < 0)
                        currentIndexByLayer = 0;
                }
                transform.position = snapPointsManager[currentIndexLayer].snapPoints[currentIndexByLayer].transform.position;
                Debug.Log(currentIndexByLayer);
            }

            _isDragging = false;
            _isHolding = false;
        }
    }

    private void HandleZoom()
    {
#if UNITY_EDITOR
        float zoomInput = _scrollZoom.action.ReadValue<float>();
        ApplyZoom(zoomInput * 10f);
#endif

        if (Touch.activeTouches.Count != 2)
        {
            _previousPinchDistance = 0f;
            _stopZooming = false;
            return;
        }

        Touch t0 = Touch.activeTouches[0];
        Touch t1 = Touch.activeTouches[1];
        float currentDistance = Vector2.Distance(t0.screenPosition, t1.screenPosition);

        if (_previousPinchDistance <= 0f)
        {
            _previousPinchDistance = currentDistance;
            return;
        }

        float delta = currentDistance - _previousPinchDistance;
        _previousPinchDistance = currentDistance;

        if (_stopZooming) return;

        if (Mathf.Abs(delta) < _pinchThreshold) return;

        ApplyZoom(delta);
        _stopZooming = true;
    }

    private void ApplyZoom(float delta)
    {
        if (snapPointsManager.Count == 0) return;
        if (snapPointsManager[currentIndexLayer].snapPoints.Length == 0) return;

        if (delta < 0)
            currentIndexLayer--;
        else if (delta > 0)
            currentIndexLayer++;

        currentIndexLayer = Mathf.Clamp(currentIndexLayer, 0, snapPointsManager.Count - 1);

        currentIndexByLayer = Mathf.Clamp(currentIndexByLayer, 0, snapPointsManager[currentIndexLayer].snapPoints.Length - 1);

        transform.position = snapPointsManager[currentIndexLayer].snapPoints[currentIndexByLayer].transform.position;
    }

    private Vector2 GetPointerPosition()
    {
        if (Touch.activeTouches.Count > 0)
            return Touch.activeTouches[0].screenPosition;

        return Mouse.current.position.ReadValue();
    }

    #endregion
}