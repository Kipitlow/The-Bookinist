using UnityEngine;
using UnityEngine.UI;

public class MoveObject : MonoBehaviour
{

    #region Variables
    [Header("Movement")]
    [SerializeField] private float smoothTime = 0.2f;
    [SerializeField] private float arrivalThreshold = 0.01f;

    private bool _hasMoved = false; 
    private bool _isMoving = false;

    private int _hasMovedThisManyTime; 

    private Vector3 _targetPosition;
    private Vector3 _velocity;
    #endregion

    private void Awake()
    {
        _targetPosition = transform.localPosition;
    }


    #region Update 
    private void Update()
    {
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, _targetPosition, ref _velocity, smoothTime);

        if (Vector3.Distance(transform.localPosition, _targetPosition) <= arrivalThreshold)
        {
            transform.localPosition = _targetPosition;
            _velocity = Vector3.zero;
            _isMoving = false;

            _hasMoved = true;
            _hasMovedThisManyTime++;
        }
    }
    #endregion

    #region Move
    public void Move(float offsetX, float offsetY)
    {
        _targetPosition += new Vector3(offsetX, offsetY, 0f);
        _isMoving = true;
    }
    #endregion

    #region ResetHasMoved
    public void ResethasMoved()
    {
        _hasMoved = false ;
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