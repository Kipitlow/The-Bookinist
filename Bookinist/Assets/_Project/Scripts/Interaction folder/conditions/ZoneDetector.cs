using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ZoneDetector : MonoBehaviour
{
    [SerializeField] private Transform ownerRoot;
    [SerializeField] private bool useRootObject = true;

    private readonly HashSet<GameObject> objectsInside = new();

    private void Reset()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;

        if (transform.parent != null)
            ownerRoot = transform.parent;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == null)
            return;

        if (ownerRoot != null && other.gameObject == ownerRoot.root.gameObject)
            return;

        objectsInside.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == null)
            return;

        if (ownerRoot != null && other.gameObject == ownerRoot.root.gameObject)
            return;

        objectsInside.Remove(other.gameObject);
    }

    public bool IsInside(GameObject obj)
    {
        if (obj == null) return false;
        return objectsInside.Contains(obj);
    }
}