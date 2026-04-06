using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Gestionnaire d'événements simple exposant des UnityEvent pour être utilisé via l'inspecteur ou d'autres scripts.
/// </summary>
public class EventHandler : MonoBehaviour
{
    #region Variables

    public UnityEvent OnDroppedItem;

    #endregion

    #region Methods

    /// <summary>
    /// Déclenche l'événement OnDroppedItem.
    /// </summary>
    public void DroppedItemCallEvent()
    {
        OnDroppedItem?.Invoke();
    }

    #endregion
}
