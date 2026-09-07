// InteractionChecks.cs
using UnityEngine;

[RequireComponent(typeof(MoveObject))]
public class MoveCheck : MonoBehaviour
{
    #region Variables
    private MoveObject _moveObject;
    private MoveOnZoom _moveOnZoom;
    private DroppedObjectTracker _droppedTracker;
    #endregion

    #region UnityMethods
    private void Awake()
    {
        _moveObject = GetComponent<MoveObject>();
        _moveOnZoom = GetComponent<MoveOnZoom>();
        _droppedTracker = GetComponent<DroppedObjectTracker>();
    }
    #endregion

    #region Methods
    public bool HasMoved()
    {
        return _moveObject != null && _moveObject.HasMovedAtLeast(1);
    }

    public bool HasNeverMoved()
    {
        return _moveObject != null && _moveObject.HasNeverMoved();
    }

    public bool HasMovedAtLeast(int count)
    {
        return _moveObject != null && _moveObject.HasMovedAtLeast(count);
    }

    public bool IsDroppedSettled()
    {
        return _droppedTracker == null || _droppedTracker.isSettled;
    }

    public bool IsVisible()
    {
        return _moveOnZoom != null && _moveOnZoom.State == MoveOnZoom.ZoomState.Visible;
    }

    public bool IsHidden()
    {
        return _moveOnZoom != null && _moveOnZoom.State == MoveOnZoom.ZoomState.Hidden;
    }

    public bool IsShowing()
    {
        return _moveOnZoom != null && _moveOnZoom.State == MoveOnZoom.ZoomState.Showing;
    }

    public bool IsHiding()
    {
        return _moveOnZoom != null && _moveOnZoom.State == MoveOnZoom.ZoomState.Hiding;
    }
    #endregion
}