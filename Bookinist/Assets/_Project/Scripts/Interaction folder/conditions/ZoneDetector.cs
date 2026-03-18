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

    private GameObject SafetyCheck(GameObject obj)
    {
        if (obj == null)
            return null;

        return useRootObject ? obj.transform.root.gameObject : obj;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject candidate = SafetyCheck(other.gameObject);

        if (candidate == null)
            return;

        if (ownerRoot != null && candidate == ownerRoot.root.gameObject)
            return;

        objectsInside.Add(candidate);
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject candidate = SafetyCheck(other.gameObject);

        if (candidate == null)
            return;

        if (ownerRoot != null && candidate == ownerRoot.root.gameObject)
            return;

        objectsInside.Remove(candidate);
    }

    public bool Contains(GameObject obj)
    {
        GameObject candidate = SafetyCheck(obj);
        return candidate != null && objectsInside.Contains(candidate);
    }
}