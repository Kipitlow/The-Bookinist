using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using UnityEngine.Events;
using Unity.Cinemachine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private GraphicRaycaster _uiRaycaster;
    [SerializeField] private EventSystem _eventSystem;

    [Header("Debug")]
    [SerializeField] private RayCastDebugger raycastDebugger;

    [Header("Drag")]
    [SerializeField] private InputActionReference dragDelta;
    [SerializeField] private InputActionReference dragPress;
    [SerializeField] private float dragSpeed = 0.01f;

    [Header("Tap")]
    [SerializeField] private float tapMaxTime = 0.25f;
    [SerializeField] private float tapMaxMovement = 10f;
    [SerializeField] private LayerMask tapMask;
    [SerializeField] private float tapRange = 100f;

    [Header("Zoom")]
    [SerializeField] private InputActionReference scrollZoom;
    [SerializeField] private float minZ;
    [SerializeField] private float maxZ;

    [Header("Hold")]
    Camera _cam;
    [SerializeField] private GameObject _itemBase;
    [SerializeField] private float holdMinTime = 0.5f;
    private bool isHolding;

    [Header("Global Navigation")]
    public List<SnapPointManager> snapPointsManager = new();
    [SerializeField] private int SnapPointNumberOnOneLayer = 3;
    public int currentIndexLayer = 0;
    public int currentIndexByLayer = 2;

    [Header("Cinemachine")]
    [SerializeField] private CamManager _camManager;
    [SerializeField] private CinemachineBrain _cinemachineBrain;
    [SerializeField] private CameraBlendRuntime _CameraBlendRuntime;

    private float previousPinchDistance;

    private bool isPressing;
    private bool isDragging;
    private float pressStartTime;
    private Vector2 pressStartPosition;


    bool stopZooming;
    private float pinchThreshold = 30f;
    private float zoomCooldown = 0.4f;
    private float lastZoomTime = -999f;

    private bool _isPointerBlocked;
    private bool _actionsDisabled;
    private Coroutine edgeCheckCoroutine;

    public event Action<int, int> OnZoom;
    public event Action<int> OnZoomOrDezoom;
    public event Action OnSwipe;



    void OnEnable()
    {
        dragDelta?.action.Enable();
        dragPress?.action.Enable();
        scrollZoom?.action.Enable();
        EnhancedTouchSupport.Enable();
        _actionsDisabled = false;
    }

    void OnDisable()
    {
        dragDelta?.action.Disable();
        dragPress?.action.Disable();
        scrollZoom?.action.Disable();
        EnhancedTouchSupport.Disable();
        _actionsDisabled = false;
    }

    private void Awake()
    {
        currentIndexByLayer = 2;
        _cam = Camera.main;

        if (_cinemachineBrain == null && _cam != null)
            _cinemachineBrain = _cam.GetComponent<CinemachineBrain>();
    }

    private void Start()
    {
        int turn = PageManager.Instance.LayerHolder.Count;

        for (int i = 0; i < turn - 1; i++)
        {
            maxZ = PageManager.maxLayer * (PageManager.Instance.LayerSpread[i] / 2);
        
        }
        currentIndexByLayer =2;
        currentIndexLayer = 0;
        transform.position = snapPointsManager[currentIndexLayer].snapPoints[currentIndexByLayer].transform.position;
    }

    void Update()
    {
        // Évaluer une fois par frame si AU MOINS un pointeur est bloqué par l'UI
        _isPointerBlocked = IsAnyPointerBlocked();

        // Bloquer / débloquer les InputActions référencées pour "bloquer toutes les actions"
        ManageActionsEnabledState(!_isPointerBlocked);

        HandleTapAndDrag();
        HandleZoom();
    }

    private void CheckEdge()
    {
        if (_cinemachineBrain.IsBlending)
            return;

        int maxIndex = snapPointsManager[currentIndexLayer].snapPoints.Length - 1;

        if (currentIndexByLayer == 0)
        {
            currentIndexByLayer++;
            _camManager.NextCamera();
        }
        else if (currentIndexByLayer >= maxIndex)
        {
            currentIndexByLayer--;
            _camManager.PreviousCamera();
        }
    }
    private void StartEdgeCheckAfterBlend()
    {
        if (edgeCheckCoroutine != null)
            StopCoroutine(edgeCheckCoroutine);

        edgeCheckCoroutine = StartCoroutine(CheckEdgeAfterBlendRoutine());
    }

    private IEnumerator CheckEdgeAfterBlendRoutine()
    {
        yield return null;
        yield return null;

        while (_cinemachineBrain != null && _cinemachineBrain.IsBlending)
            yield return null;

        CheckEdge();
    }

    private void ManageActionsEnabledState(bool enable)
    {
        if (enable && _actionsDisabled)
        {
            dragDelta?.action.Enable();
            dragPress?.action.Enable();
            scrollZoom?.action.Enable();
            _actionsDisabled = false;
        }
        else if (!enable && !_actionsDisabled)
        {
            dragDelta?.action.Disable();
            dragPress?.action.Disable();
            scrollZoom?.action.Disable();
            _actionsDisabled = true;
        }
    }

    private bool IsPointerBlockedByUI(Vector2 screenPosition)
    {
        // sécurité si pas configuré
        if (_uiRaycaster == null || _eventSystem == null || _cam == null)
            return false;

        PointerEventData pointerData = new PointerEventData(_eventSystem)
        {
            position = screenPosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        _uiRaycaster.Raycast(pointerData, results);

        if (results.Count == 0)
            return false;

        foreach (var result in results)
        {
            // si un CanvasGroup parent bloque les raycasts (overlay noir)
            CanvasGroup cg = result.gameObject.GetComponentInParent<CanvasGroup>();
            if (cg != null && cg.blocksRaycasts && cg.alpha > 0.01f)
                return true;

            var shader = result.gameObject.GetComponentInParent<ShaderBasedRaycast>();

            if (shader != null)
            {
                // CORRECTION : si le shader dit que l'emplacement est VALIDE (opaque) => bloquer
                if (shader.IsRaycastLocationValid(screenPosition, _cam))
                    return true;
                else
                    continue; // transparent ici, vérifier le résultat suivant
            }
            else
            {
                // élément UI sans shader => bloque
                return true;
            }
        }

        return false;
    }

    // Nouveau : vérifie tous les pointeurs (touches actives + souris) et renvoie true si AU MOINS un pointeur est bloqué par l'UI
    private bool IsAnyPointerBlocked()
    {
        if (_uiRaycaster == null || _eventSystem == null || _cam == null)
            return false;

        // Touches actives (EnhancedTouch)
        foreach (var t in Touch.activeTouches)
        {
            if (IsPointerBlockedByUI(t.screenPosition))
                return true;
        }

        // Si pas de touch ou sur éditeur, vérifier la souris
        if (Touch.activeTouches.Count == 0 && Mouse.current != null)
        {
            if (IsPointerBlockedByUI(Mouse.current.position.ReadValue()))
                return true;
        }

        return false;
    }

    void HandleTapAndDrag()
    {
        if (Touch.activeTouches.Count > 1)
            return;

        // utiliser la valeur calculée en Update
        if (_isPointerBlocked)
        {
            isPressing = false;
            isDragging = false;
            isHolding = false;
            return;
        }

        bool pressed = dragPress.action.IsPressed();

        if (pressed && !isPressing)
        {
            isPressing = true;
            isDragging = false;
            pressStartTime = Time.time;
            pressStartPosition = GetPointerPosition();
        }

        if (pressed && isPressing)
        {
            Vector2 delta = dragDelta.action.ReadValue<Vector2>();

            if (!isDragging && delta.magnitude > tapMaxMovement)
            {
                isDragging = true;
            }

            if (!isDragging && !isHolding)
            {
                float heldTime = Time.time - pressStartTime;

                if (heldTime >= holdMinTime)
                {
                    isHolding = true;

                    // Raycast proprement sur l'UI pour détecter un Button sous le pointeur
                    PointerEventData pointerData = new PointerEventData(_eventSystem)
                    {
                        position = GetPointerPosition()
                    };

                    List<RaycastResult> results = new List<RaycastResult>();
                    _uiRaycaster.Raycast(pointerData, results);

                    foreach (var result in results)
                    {
                        Button button = result.gameObject.GetComponent<Button>();

                        if (button != null)
                        {
                            GameObject newObj = Instantiate(_itemBase, button.transform, false);
                            break;
                        }
                    }
                }
      
            }
        }

        // Press released
        if (!pressed && isPressing)
        {
            float pressDuration = Time.time - pressStartTime;
            float movement = Vector2.Distance(pressStartPosition, GetPointerPosition());

            isPressing = false;

            if (isHolding)
            {
            }
            else if (pressDuration <= tapMaxTime && movement <= tapMaxMovement)
            {
                // tap
            }
            else if (isDragging)
            {
                if (GetPointerPosition().x < pressStartPosition.x)
                {
                    if (currentIndexByLayer != 4)
                    { 
                        currentIndexByLayer++;
                        _camManager.NextCamera();
                        StartEdgeCheckAfterBlend();
                        OnSwipe?.Invoke();
                    }

                }
                else if (GetPointerPosition().x > pressStartPosition.x)
                {
                    if (currentIndexByLayer != 0)
                    { 
                        currentIndexByLayer--;
                        _camManager.PreviousCamera();
                        StartEdgeCheckAfterBlend();
                        OnSwipe?.Invoke();
                    }
                }
                if (currentIndexByLayer == 0 || currentIndexByLayer == SnapPointNumberOnOneLayer)
                    _CameraBlendRuntime.SetEaseInBlend(0.3f);
                else
                    _CameraBlendRuntime.SetEaseInBlend(1f);

            }
            isDragging = false;
            isHolding = false;
        }
    }

    private void HandleZoom()
    {
#if UNITY_EDITOR
        float zoomInput = scrollZoom.action.ReadValue<float>();
        ApplyZoom(zoomInput * 10f);
#endif

        // Utiliser la valeur calculée en Update pour bloquer le zoom quand l'UI doit le bloquer
        if (_isPointerBlocked)
            return;

        if (Touch.activeTouches.Count != 2)
        {
            previousPinchDistance = 0f;
            stopZooming = false;
            return;
        }

        Touch t0 = Touch.activeTouches[0];
        Touch t1 = Touch.activeTouches[1];

        // Double-sécurité : si l'un des deux doigts est sur l'UI, annuler le pinch
        if (IsPointerBlockedByUI(t0.screenPosition) || IsPointerBlockedByUI(t1.screenPosition))
        {
            previousPinchDistance = 0f;
            stopZooming = false;
            return;
        }

        float currentDistance = Vector2.Distance(t0.screenPosition, t1.screenPosition);

        if (previousPinchDistance <= 0f)
        {
            previousPinchDistance = currentDistance;
            return;
        }

        float delta = currentDistance - previousPinchDistance;
        previousPinchDistance = currentDistance;

        if (stopZooming) return;

        if (Mathf.Abs(delta) < pinchThreshold) return;

        ApplyZoom(delta);
        stopZooming = true;
    }

    private void ApplyZoom(float delta)
    {
        if (snapPointsManager.Count == 0) return;
        if (snapPointsManager[currentIndexLayer].snapPoints.Length == 0) return;

        if (delta < 0)
        {
            if (currentIndexLayer != 0)
            {
                currentIndexLayer--;
                for (int i = 0; i < SnapPointNumberOnOneLayer; i++)
                {
                    _camManager.PreviousCamera();
                }
                OnZoomOrDezoom?.Invoke(-1);
            }
        }
        else if (delta > 0)
        {
            if (currentIndexLayer != SnapPointNumberOnOneLayer)
            {
                currentIndexLayer++;
                for (int i = 0; i < SnapPointNumberOnOneLayer; i++)
                {
                    _camManager.NextCamera();
                }
                OnZoomOrDezoom?.Invoke(1);
            }
        }

        currentIndexLayer = Mathf.Clamp(currentIndexLayer, 0, snapPointsManager.Count - 1);
        currentIndexByLayer = Mathf.Clamp(currentIndexByLayer, 0, snapPointsManager[currentIndexLayer].snapPoints.Length - 1);

        OnZoom?.Invoke(currentIndexLayer, currentIndexByLayer);

        transform.position = snapPointsManager[currentIndexLayer].snapPoints[currentIndexByLayer].transform.position;
    }

    Vector2 GetPointerPosition()
    {
        if (Touch.activeTouches.Count > 0)
            return Touch.activeTouches[0].screenPosition;

        return Mouse.current.position.ReadValue();
    }
}