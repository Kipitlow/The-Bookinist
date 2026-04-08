using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ZoneDetector : MonoBehaviour
{
    #region Variables

    [SerializeField] private Transform _ownerRoot;

    private InteractionRunner _interactionRunner;

    private readonly HashSet<GameObject> _objectsInside = new();

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

    public void TriggerCollision(bool shouldBeTriggered)
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

        _objectsInside.Add(hit.gameObject);

        InteractionContext context = new()
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

        _objectsInside.Remove(hit.gameObject);
    }

    public bool IsInside(GameObject obj)
    {
        if (obj == null) return false;
        return _objectsInside.Contains(obj);
    }

    #endregion
}