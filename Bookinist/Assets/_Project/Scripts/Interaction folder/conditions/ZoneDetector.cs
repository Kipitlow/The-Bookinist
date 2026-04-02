using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ZoneDetector : MonoBehaviour
{
    [SerializeField] private Transform _ownerRoot;

    private InteractionRunner _interactionRunner;

    private readonly HashSet<GameObject> objectsInside = new();

    private void Start()
    {
        if (transform.parent != null)
        {
            _ownerRoot = transform.parent;
            _interactionRunner = GetComponentInParent<InteractionRunner>();
        }
    }

    public void triggerCollision(bool shouldBeTriggered)
    {
        Collider thisCollider = GetComponent<Collider>();
        thisCollider.isTrigger = shouldBeTriggered;
    }

    private void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject == null)
            return;

        if (_ownerRoot != null && hit.gameObject == _ownerRoot.root.gameObject)
            return;

        objectsInside.Add(hit.gameObject);


        InteractionContext context = new InteractionContext
        {
            instigator = null,
            target = hit.gameObject
        };

        _interactionRunner.TryExecuteAll(context);
    }

    private void OnTriggerExit(Collider hit)
    {
        if (hit.gameObject == null)
            return;

        if (_ownerRoot != null && hit.gameObject == _ownerRoot.root.gameObject)
            return;

        objectsInside.Remove(hit.gameObject);
    }

    public bool IsInside(GameObject Obj)
    {
        if (Obj == null) return false;
        return objectsInside.Contains(Obj);
    }
}