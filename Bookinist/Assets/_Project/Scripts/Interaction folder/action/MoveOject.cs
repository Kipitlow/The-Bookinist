using UnityEngine;
using UnityEngine.UI;

public class MoveObject : MonoBehaviour
{

    #region Variables
    [Header("Movement")]
    [SerializeField] private float smoothTime = 0.2f;

    private bool _hasMoved = false; 
    private int _hasMovedThisManyTime; 

    private Vector3 _targetPosition;
    private Vector3 _velocity;
    #endregion

    private void Awake()
    {
        _targetPosition = transform.localPosition;
    }


    #region Variables
    private void Update()
    {
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, _targetPosition, ref _velocity, smoothTime);
    }

    public void Move(float offsetX, float offsetY)
    {
        Debug.Log("Move : " + _hasMoved);
        _targetPosition += new Vector3(offsetX, offsetY, 0f);
        _hasMoved = true;
        _hasMovedThisManyTime++;
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