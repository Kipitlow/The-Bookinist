using Unity.VisualScripting;
using UnityEngine;

public class Slot : MonoBehaviour
{
    private bool _isEmpty;
    private GameObject _currentObject;

    public bool IsEmpty() { return _isEmpty; }

    public void Fill(GameObject gameObject)
    {
        if (gameObject == null) return;
        Instantiate(gameObject);
        _isEmpty = true;
    }

    public void Clear()
    {
        if (_isEmpty || _currentObject == null) return;
        Destroy(_currentObject);
        _isEmpty = false;
    }
}
