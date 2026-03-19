using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class CameraMovement : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] RayCastDebugger raycastDebugger;
    
    [Header("Drag")]
    [SerializeField] InputActionReference dragDelta;
    [SerializeField] InputActionReference dragPress;
    [SerializeField] float dragSpeed = 0.01f;

    [Header("Tap")]
    [SerializeField] float tapMaxTime = 0.25f;
    [SerializeField] float tapMaxMovement = 10f;
    [SerializeField] LayerMask tapMask;
    [SerializeField] float tapRange = 100f;

    [Header("Zoom")]
    [SerializeField] InputActionReference scrollZoom;
    [SerializeField] float minZ;
    [SerializeField] float maxZ;

    [Header("Global Navigation")]
    [SerializeField] GameObject[] snapPoints;
    [SerializeField] int SnapPointNumberOnOneLayer = 3;


    float previousPinchDistance;

    bool isPressing;
    bool isDragging;
    float pressStartTime;
    Vector2 pressStartPosition;
    int currentIndexSnapPoint = 1;

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

            if (!isDragging && pressDuration <= tapMaxTime && movement <= tapMaxMovement)
            {
                OnTap(pressStartPosition);
            }

            isPressing = false;
            isDragging = false;

            if (GetPointerPosition().x < pressStartPosition.x)
            {
                currentIndexSnapPoint++;

                if (currentIndexSnapPoint > snapPoints.Length - 1) currentIndexSnapPoint = snapPoints.Length - 1;
            }
            else if (GetPointerPosition().x > pressStartPosition.x)
            {
                currentIndexSnapPoint--;

                if (currentIndexSnapPoint < 0) currentIndexSnapPoint = 0;
            }

            transform.position = snapPoints[currentIndexSnapPoint].transform.position;
        }
    }

    void HandleZoom()
    {
        // Scroll pour debug sur PC
        float scroll = scrollZoom.action.ReadValue<float>();
        if (Mathf.Abs(scroll) > 0.01f)
        {
            ApplyZoom(scroll * 200);
            return;
        }

        // Scroll mobile
        if (Touch.activeTouches.Count == 2)
        {
            Touch t0 = Touch.activeTouches[0];
            Touch t1 = Touch.activeTouches[1];

            float currentDistance = Vector2.Distance(t0.screenPosition, t1.screenPosition);

            if (previousPinchDistance > 0f)
            {
                float delta = currentDistance - previousPinchDistance;
                ApplyZoom(delta);
            }

            previousPinchDistance = currentDistance;
        }
        else
        {
            previousPinchDistance = 0f;
        }
    }

    void ApplyZoom(float delta)
    {
        //Vector3 pos = transform.position;
        ////pos.z += delta * zoomSpeed;
        //pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
        //transform.position = pos;
        if (delta < 0)
        {
            currentIndexSnapPoint -= SnapPointNumberOnOneLayer;

            if (currentIndexSnapPoint < 0) currentIndexSnapPoint = 0;
        }
        else if (delta > 0)
        {
            currentIndexSnapPoint += SnapPointNumberOnOneLayer;
        
            if (currentIndexSnapPoint > snapPoints.Length - 1) currentIndexSnapPoint -= SnapPointNumberOnOneLayer;
        }

        transform.position = snapPoints[currentIndexSnapPoint].transform.position;

    }

    Vector2 GetPointerPosition()
    {
        if (Touch.activeTouches.Count > 0)
            return Touch.activeTouches[0].screenPosition;

        return Mouse.current.position.ReadValue();
    }

    void OnTap(Vector2 screenPosition)
    {
        Camera playerCam = GetComponent<Camera>();

        if (Physics.Raycast(playerCam.ScreenPointToRay(screenPosition), out RaycastHit hit, tapRange, tapMask))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                interactable.OnTap(hit.point);
            }
        }
    }
}
