// MoveObject.cs
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    #region Variables
    [Header("Movement")]
    [SerializeField] private float _defaultSmoothTime = 0.2f;
    [SerializeField] private float _arrivalThreshold = 0.01f;
    public bool isDroppedItem;

    public Vector3 _baseLocalPosition { get; private set; }
    private Vector3 _targetLocalPosition;
    private Vector3 _velocity;
    private float _currentSmoothTime;

    private bool _shouldCountMoveOnArrival;
    private bool _shouldReset;

    private bool _hasMoved;
    private int _hasMovedThisManyTime;

    private Rigidbody _rb;
    private bool _restoreRigidbodyAfterMove;
    private bool _rbWasKinematicBeforeMove;

    public bool isMoving { get; private set; }
    public Vector3 CurrentTarget => _targetLocalPosition;
    #endregion

    #region UnityMethods

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _targetLocalPosition = transform.localPosition;
        _currentSmoothTime = _defaultSmoothTime;
        _velocity = Vector3.zero;

        _baseLocalPosition = transform.localPosition;

        isMoving = false;
        _hasMoved = false;
        _hasMovedThisManyTime = 0;
        _shouldCountMoveOnArrival = false;
        _shouldReset = false;
        _restoreRigidbodyAfterMove = false;
        _rbWasKinematicBeforeMove = false;
    }

    private void Update()
    {
        if (!isMoving)
        {
            if (_shouldReset)
                ResetMoveStateImmediate();

            return;
        }

        if (_currentSmoothTime <= 0f)
        {
            CompleteMovement();
            return;
        }

        transform.localPosition = Vector3.SmoothDamp(
            transform.localPosition,
            _targetLocalPosition,
            ref _velocity,
            _currentSmoothTime
        );

        float sqrDistance = (transform.localPosition - _targetLocalPosition).sqrMagnitude;
        float sqrThreshold = _arrivalThreshold * _arrivalThreshold;

        if (sqrDistance <= sqrThreshold)
            CompleteMovement();
    }
    #endregion

    #region Methods

    #region Complete Movement
    private void CompleteMovement()
    {
        transform.localPosition = _targetLocalPosition;
        _velocity = Vector3.zero;
        isMoving = false;

        if (_shouldCountMoveOnArrival)
        {
            _shouldCountMoveOnArrival = false;
            _hasMoved = true;
            _hasMovedThisManyTime++;
            _baseLocalPosition = _targetLocalPosition;
        }

        RestoreRigidbodyAfterMove();

        if (_shouldReset)
            ResetMoveStateImmediate();
    }
    #endregion

    #region RigidBody Handler

    private void PrepareRigidbodyForScriptMove()
    {
        if (_rb == null)
            return;

        _rbWasKinematicBeforeMove = _rb.isKinematic;

        if (!_rb.isKinematic)
        {
            _restoreRigidbodyAfterMove = true;
            _rb.isKinematic = true;
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
        else
        {
            _restoreRigidbodyAfterMove = false;
        }
    }

    private void RestoreRigidbodyAfterMove()
    {
        if (_rb == null || !_restoreRigidbodyAfterMove)
            return;

        _rb.isKinematic = _rbWasKinematicBeforeMove;
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _restoreRigidbodyAfterMove = false;
    }
    #endregion

    #region Move
    public void MoveTo(Vector3 targetLocalPosition, float smoothTime, bool countAsInteractionMove)
    {
        _targetLocalPosition = targetLocalPosition;
        _currentSmoothTime = Mathf.Max(0f, smoothTime);
        _shouldCountMoveOnArrival = countAsInteractionMove;

        PrepareRigidbodyForScriptMove();

        if (_currentSmoothTime <= 0f)
        {
            CompleteMovement();
            return;
        }

        isMoving = true;
    }

    public void MoveBy(Vector2 offset, float smoothTime)
    {
        MoveBy(new Vector3(offset.x, offset.y, 0f), smoothTime, false);
    }

    public void MoveBy(Vector2 offset, float smoothTime, bool countAsInteractionMove)
    {
        MoveTo(
            _targetLocalPosition + new Vector3(offset.x, offset.y, 0f),
            smoothTime,
            countAsInteractionMove
        );
    }

    public void MoveInteraction(float x, float y)
    {
        MoveBy(new Vector2(x, y), _defaultSmoothTime, true);
    }
    public void SetPositionImmediate(Vector3 localPosition)
    {
        _targetLocalPosition = localPosition;
        transform.localPosition = localPosition;
        _velocity = Vector3.zero;
        isMoving = false;
        _shouldCountMoveOnArrival = false;

        RestoreRigidbodyAfterMove();
    }
    #endregion

    #region Sets / Update / Reset

    public void SetBasePosition(Vector3 localPosition)
    {
        _baseLocalPosition = localPosition;
    }

    public void SetBaseToCurrentPosition()
    {
        _baseLocalPosition = transform.localPosition;
    }
    public void UpdateBasePos()
    {
        if (isDroppedItem)
            _baseLocalPosition = transform.localPosition;
    }

    private void ResetMoveStateImmediate()
    {
        _hasMoved = false;
        _hasMovedThisManyTime = 0;
        _shouldReset = false;
        _shouldCountMoveOnArrival = false;
    }

    public void ResetHasMoved()
    {
        if (isMoving)
        {
            _shouldReset = true;
            return;
        }

        ResetMoveStateImmediate();
    }
    #endregion

    #region Checks

    public bool IsAtPosition(Vector3 localPosition)
    {
        return (transform.localPosition - localPosition).sqrMagnitude <= _arrivalThreshold * _arrivalThreshold;
    }

    public bool HasMoved(bool hasMoved, int howManyTime)
    {
        if (hasMoved)
            return HasMovedAtLeast(howManyTime);

        return HasNeverMoved();
    }

    public bool HasMovedAtLeast(int howManyTime)
    {
        return _hasMovedThisManyTime >= howManyTime;
    }

    public bool HasNeverMoved()
    {
        return !_hasMoved;
    }
    #endregion

    #endregion
}