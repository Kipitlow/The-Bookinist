using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class CameraMovement : MonoBehaviour
{
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

    [Header("Global Navigation")]
    public List<SnapPointManager> snapPointsManager = new();
    [SerializeField] private int SnapPointNumberOnOneLayer = 3;
    public int currentIndexLayer { get; private set; } = 0;
    public int currentIndexByLayer { get; private set; }

    private float previousPinchDistance;

    private bool isPressing;
    private bool isDragging;
    private float pressStartTime;
    private Vector2 pressStartPosition;


    bool stopZooming;
    private float pinchThreshold = 30f;
    private float zoomCooldown = 0.4f;
    private float lastZoomTime = -999f;

    void OnEnable()
    {
        dragDelta.action.Enable();
        dragPress.action.Enable();
        scrollZoom.action.Enable();
        EnhancedTouchSupport.Enable();
    }

    void OnDisable()
    {
        dragDelta.action.Disable();
        dragPress.action.Disable();
        scrollZoom.action.Disable();
        EnhancedTouchSupport.Disable();
    }

    private void Awake()
    {
        currentIndexByLayer = 1;
    }

    private void Start()
    {
        maxZ = PageManager.maxLayer * (PageManager.layerSpread / 2);
    }

    void Update()
    {
        HandleTapAndDrag();
        HandleZoom();
    }

    void HandleTapAndDrag()
    {
        if (Touch.activeTouches.Count > 1)
            return;

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

            if (isDragging)
            {
                //transform.position += new Vector3(
                //    -delta.x * dragSpeed,
                //    -delta.y * dragSpeed,
                //    0f
                //);
            }
        }

        // Press released
        if (!pressed && isPressing)
        {
            float pressDuration = Time.time - pressStartTime;
            float movement = Vector2.Distance(pressStartPosition, GetPointerPosition());

            isPressing = false;
            isDragging = false;

            if (pressDuration <= tapMaxTime && movement <= tapMaxMovement)
            {
                // tap
            }
            else
            {
                if (GetPointerPosition().x < pressStartPosition.x)
                {
                    currentIndexByLayer++;

                    if (currentIndexByLayer > snapPointsManager[currentIndexLayer].snapPoints.Length - 1) 
                        currentIndexByLayer = snapPointsManager[currentIndexLayer].snapPoints.Length - 1;
                }
                else if (GetPointerPosition().x > pressStartPosition.x)
                {
                    currentIndexByLayer--;

                    if (currentIndexByLayer < 0) 
                        currentIndexByLayer = 0;
                }
                transform.position = snapPointsManager[currentIndexLayer].snapPoints[currentIndexByLayer].transform.position;
                print(currentIndexByLayer);
            }
        }
    }

    private void HandleZoom()
    {
#if UNITY_EDITOR
        float zoomInput = scrollZoom.action.ReadValue<float>();
        ApplyZoom(zoomInput * 10f);
#endif
        if (Touch.activeTouches.Count != 2)
        {
            previousPinchDistance = 0f;
            stopZooming = false;
            return;
        }

        Touch t0 = Touch.activeTouches[0];
        Touch t1 = Touch.activeTouches[1];
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
            currentIndexLayer--;

        else if (delta > 0)
            currentIndexLayer++;

            currentIndexLayer = Mathf.Clamp(currentIndexLayer, 0, snapPointsManager.Count - 1);

        currentIndexByLayer = Mathf.Clamp(currentIndexByLayer, 0, snapPointsManager[currentIndexLayer].snapPoints.Length - 1);

        transform.position = snapPointsManager[currentIndexLayer].snapPoints[currentIndexByLayer].transform.position;
    }

    Vector2 GetPointerPosition()
    {
        if (Touch.activeTouches.Count > 0)
            return Touch.activeTouches[0].screenPosition;

        return Mouse.current.position.ReadValue();
    }
}