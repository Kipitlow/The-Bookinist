using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class LayerDetector : MonoBehaviour
{
    [SerializeField] private Transform _ownerRoot;

    private InteractionRunner _interactionRunner;

    private void Start()
    {
        if (transform.parent != null)
        {
            _ownerRoot = transform.parent;
            _interactionRunner = GetComponentInParent<InteractionRunner>();
        }
    }

    public bool IsInSameLayer(GameObject wantedObject, Page wantedLayer)
    {
        return wantedLayer.PageObjects.Contains(wantedObject);
    }

    [SerializeField] private EventHandler _eventHandler;


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

    //    _interactionRunner.TryExecuteAll()
    //}

}