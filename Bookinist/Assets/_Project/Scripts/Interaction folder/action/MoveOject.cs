using UnityEngine;

public class MoveObject : MonoBehaviour
{

    #region Variables
    [Header("Movement")]
    [SerializeField] private float smoothTime = 0.2f;

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
        _targetPosition += new Vector3(offsetX, offsetY, 0f);
    }
    #endregion
}