// DroppedObjectTracker.cs
using UnityEngine;

[RequireComponent(typeof(MoveObject))]
[RequireComponent(typeof(Rigidbody))]
public class DroppedObjectTracker : MonoBehaviour
{
    #region Variables
    [Header("Tracking")]
    [SerializeField] private bool _trackContinuouslyWhileDynamic = true;
    [SerializeField] private bool _ignoreWhileScriptMoveIsRunning = true;

    [Header("Settling")]
    [SerializeField] private float _velocityThreshold = 0.03f;
    [SerializeField] private float _angularVelocityThreshold = 0.03f;
    [SerializeField] private float _settledDelay = 0.15f;
    [SerializeField] private float _minimumLocalDeltaToRefresh = 0.0001f;

    private MoveObject _moveObject;
    private MoveOnZoom _moveOnZoom;
    private Rigidbody _rb;

    private Vector3 _lastRecordedLocalPosition;
    private float _settledTimer;

    public bool isSettled { get; private set; }
    #endregion

    #region UnityMethods
    private void Awake()
    {
        _moveObject = GetComponent<MoveObject>();
        _moveOnZoom = GetComponent<MoveOnZoom>();
        _rb = GetComponent<Rigidbody>();

        _moveObject.isDroppedItem = true;
    }

    private void Start()
    {
        CaptureCurrentAsBase();
        isSettled = false;
        _settledTimer = 0f;
    }

    private void LateUpdate()
    {
        if (_rb == null || _moveObject == null)
            return;

        if (_ignoreWhileScriptMoveIsRunning && _moveObject.isMoving)
            return;

        if (_rb.isKinematic)
        {
            if (HasMovedEnoughSinceLastCapture())
                CaptureCurrentAsBase();

            return;
        }

        bool isPhysicallyMoving =
            _rb.linearVelocity.sqrMagnitude > _velocityThreshold * _velocityThreshold ||
            _rb.angularVelocity.sqrMagnitude > _angularVelocityThreshold * _angularVelocityThreshold;

        if (isPhysicallyMoving)
        {
            isSettled = false;
            _settledTimer = 0f;

            if (_trackContinuouslyWhileDynamic || HasMovedEnoughSinceLastCapture())
                CaptureCurrentAsBase();

            return;
        }

        if (HasMovedEnoughSinceLastCapture())
            CaptureCurrentAsBase();

        _settledTimer += Time.deltaTime;

        if (_settledTimer >= _settledDelay)
        {
            CaptureCurrentAsBase();
            isSettled = true;
        }
    }
    #endregion

    #region Methods
    public void ForceRefreshBaseNow()
    {
        CaptureCurrentAsBase();
        isSettled = true;
        _settledTimer = 0f;
    }

    private bool HasMovedEnoughSinceLastCapture()
    {
        return (transform.localPosition - _lastRecordedLocalPosition).sqrMagnitude >
               _minimumLocalDeltaToRefresh * _minimumLocalDeltaToRefresh;
    }

    private void CaptureCurrentAsBase()
    {
        _lastRecordedLocalPosition = transform.localPosition;
        _moveObject.SetBasePosition(_lastRecordedLocalPosition);

        if (_moveOnZoom != null && _moveOnZoom.State == MoveOnZoom.ZoomState.Hidden)
            _moveOnZoom.ResetToHidden();
    }
    #endregion
}