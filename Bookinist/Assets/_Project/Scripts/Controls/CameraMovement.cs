using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class CameraMovement : MonoBehaviour
{
    [Header("Drag")]
    [SerializeField] private InputActionReference dragDelta;
    [SerializeField] private InputActionReference dragPress;
    [SerializeField] private float dragSpeed = 0.01f;

    [Header("Zoom")]
    [SerializeField] private InputActionReference scrollZoom;
    [SerializeField] private float zoomSpeed = 0.02f;
    [SerializeField] private float minZ;
    [SerializeField] private float maxZ;

    private float previousPinchDistance;

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
        HandleDrag();
        HandleZoom();
    }

    void HandleDrag()
    {
        // Only allow drag with ONE finger or mouse
        if (!dragPress.action.IsPressed() || Touch.activeTouches.Count > 1)
        {
            return;
        }

        Vector2 delta = dragDelta.action.ReadValue<Vector2>();

        transform.position += new Vector3(
            -delta.x * dragSpeed,
            -delta.y * dragSpeed,
            0f
        );
    }

    void HandleZoom()
    {
        // Mouse scroll
        float scroll = scrollZoom.action.ReadValue<float>();
        if (Mathf.Abs(scroll) > 0.01f)
        {
            ApplyZoom(scroll * 20);
            return;
        }

        // Touch pinch
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
        Vector3 pos = transform.position;
        pos.z += delta * zoomSpeed;
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
        transform.position = pos;
    }
}
