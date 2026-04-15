using UnityEngine;

public class MoveObject : MonoBehaviour
{
    #region Variables

    [Header("Movement")]
    [SerializeField] private float _defaultSmoothTime = 0.2f;
    [SerializeField] private float _arrivalThreshold = 0.01f;

    public Vector3 _baseLocalPosition { get; private set; }
    private Vector3 _targetLocalPosition;
    private Vector3 _velocity;
    private float _currentSmoothTime;

    private bool _shouldCountMoveOnArrival;
    private bool _shouldReset;

    private bool _hasMoved;
    private int _hasMovedThisManyTime;

    public bool IsMoving { get; private set; }
    public Vector3 CurrentTarget => _targetLocalPosition;

    #endregion

    #region UnityMethods

    private void Awake()
    {
        _targetLocalPosition = transform.localPosition;
        _currentSmoothTime = _defaultSmoothTime;
        _velocity = Vector3.zero;

        _baseLocalPosition = transform.localPosition;

        IsMoving = false;
        _hasMoved = false;
        _hasMovedThisManyTime = 0;
        _shouldCountMoveOnArrival = false;
        _shouldReset = false;
    }


    private void Update()
    {
        if (!IsMoving)
        {
            if (_shouldReset)
            {
                ResetMoveStateImmediate();
            }

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
        {
            CompleteMovement();
        }
    }

    #endregion

    #region Methods

    #region CompleteMovement
    private void CompleteMovement()
    {
        transform.localPosition = _targetLocalPosition;
        _velocity = Vector3.zero;
        IsMoving = false;

        if (_shouldCountMoveOnArrival)
        {
            _shouldCountMoveOnArrival = false;
            _hasMoved = true;
            _hasMovedThisManyTime++;
            _baseLocalPosition = _targetLocalPosition;
        }

        if (_shouldReset)
        {
            ResetMoveStateImmediate();
        }
    }

    #endregion

    #region Move

    #region MoveTo

    //main move
    public void MoveTo(Vector3 targetLocalPosition, float smoothTime, bool countAsInteractionMove)
    {
        _targetLocalPosition = targetLocalPosition;
        _currentSmoothTime = Mathf.Max(0f, smoothTime);
        _shouldCountMoveOnArrival = countAsInteractionMove;

        if (_currentSmoothTime <= 0f)
        {
            CompleteMovement();
            return;
        }

        IsMoving = true;
    }

    public void MoveTo(Vector3 targetLocalPosition)
    {
        MoveTo(targetLocalPosition, _defaultSmoothTime, false);
    }

    public void MoveTo(Vector3 targetLocalPosition, float smoothTime)
    {
        MoveTo(targetLocalPosition, smoothTime, false);
    }
    #endregion

    #region MoveBy
    public void MoveBy(Vector3 offset)
    {
        MoveBy(offset, _defaultSmoothTime, false);
    }

    public void MoveBy(Vector3 offset, float smoothTime)
    {
        MoveBy(offset, smoothTime, false);
    }

    public void MoveBy(Vector3 offset, float smoothTime, bool countAsInteractionMove)
    {
        MoveTo(_targetLocalPosition + offset, smoothTime, countAsInteractionMove);
    }


    public void MoveInteraction(float x, float y)
    {
        MoveBy(new Vector3(x, y, 0), _defaultSmoothTime, true);
    }

    public void MoveInteraction(float x, float y, float smoothTime)
    {
        MoveBy(new Vector3(x, y, 0), smoothTime, true);
    }

    #endregion

    #endregion

    #region checks

    public void SetPositionImmediate(Vector3 localPosition)
    {
        _targetLocalPosition = localPosition;
        transform.localPosition = localPosition;
        _velocity = Vector3.zero;
        IsMoving = false;
        _shouldCountMoveOnArrival = false;
    }

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

    // Méthodes plus claires si tu veux les utiliser ailleurs
    public bool HasMovedAtLeast(int howManyTime)
    {
        return _hasMovedThisManyTime >= howManyTime;
    }

    public bool HasNeverMoved()
    {
        return !_hasMoved;
    }
    #endregion

    #region reset

    private void ResetMoveStateImmediate()
    {
        _hasMoved = false;
        _hasMovedThisManyTime = 0;
        _shouldReset = false;
        _shouldCountMoveOnArrival = false;
    }

    public void ResetHasMoved()
    {
        if (IsMoving)
        {
            _shouldReset = true;
            return;
        }

        ResetMoveStateImmediate();
    }
    #endregion

    #endregion
}