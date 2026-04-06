using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LayerDetector : MonoBehaviour
{
    #region Variables

    [SerializeField] private Transform _ownerRoot;
    [SerializeField] private EventHandler _eventHandler;

    private InteractionRunner _interactionRunner;

    #endregion

    #region Unity Methods

    private void Start()
    {
        if (transform.parent != null)
        {
            _ownerRoot = transform.parent;
            _interactionRunner = GetComponentInParent<InteractionRunner>();
        }
    }

    #endregion

    #region Methods

    public bool IsInSameLayer(GameObject wantedObject, Page wantedLayer)
    {
        return wantedLayer.PageObjects.Contains(wantedObject);
    }

    // Exemples de hooks d'événements
    //private void OnEnable()
    //{
    //    _eventHandler.OnDroppedItem.AddListener(OnSenderTriggered);
    //}

    //private void OnDisable()
    //{
    //    _eventHandler.OnDroppedItem.RemoveListener(OnSenderTriggered);
    //}

    //private void OnSenderTriggered()
    //{
    //    InteractionContext context = new InteractionContext
    //    {
    //        instigator = null,
    //        target = hit.gameObject
    //    };
    //    _interactionRunner.TryExecuteAll(context);
    //}

    #endregion
}