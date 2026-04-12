using UnityEngine;
using UnityEngine.UI;

public class MoveObject : MonoBehaviour
{

    #region Variables
    [Header("Movement")]
    [SerializeField] private float _smoothTime = 0.2f;
    [SerializeField] private float _arrivalThreshold = 0.01f;

    private bool _hasMoved = false; 
    private bool _isMoving = false;
    private bool _shouldReset = false;

    private int _hasMovedThisManyTime; 

    private Vector3 _targetPosition;
    private Vector3 _velocity;
    #endregion

    private void Awake()
    {
        _targetPosition = transform.localPosition;
        _hasMovedThisManyTime = 0;
    }


    #region Update 
    private void Update()
    {
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, _targetPosition, ref _velocity, _smoothTime);

        if (Vector3.Distance(transform.localPosition, _targetPosition) <= _arrivalThreshold && _isMoving)
        {
            transform.localPosition = _targetPosition;
            _velocity = Vector3.zero;
            _isMoving = false;

            if( _shouldReset)
            {
                _shouldReset = false;
                _hasMoved = false;
                _hasMovedThisManyTime = 0;
                return;
            }

            _hasMoved = true;
            _hasMovedThisManyTime++;

        }
    }
    #endregion

    #region Move
    public void Move(float offsetX, float offsetY)
    {
        _targetPosition += new Vector3(offsetX, offsetY, 0f);
        if (_isMoving) return;
        _isMoving = true;

    }
    #endregion

    #region ResetHasMoved
    public void ResethasMoved()
    {
            _shouldReset = true;
    }

    #endregion

    #region Check

    public bool HasMoved(bool hasMoved, int HowManytime)
    {
        if (hasMoved)
            return _hasMovedThisManyTime >= HowManytime;
        else
            return !_hasMoved;
    }
    #endregion
}