using UnityEngine;
using UnityEngine.Events;

public class EventHandler : MonoBehaviour
{

    public UnityEvent OnDroppedItem;

    public void DroppedItemCallEvent()
    {
        OnDroppedItem?.Invoke();
    }
}
