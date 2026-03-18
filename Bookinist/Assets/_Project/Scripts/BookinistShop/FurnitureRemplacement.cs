using UnityEngine;

public class FurnitureRemplacement : MonoBehaviour
{
    [SerializeField] GameObject[] _possibleFurniture;
    [SerializeField] int _startFurnitureIndex;
    [SerializeField] Vector3 _furnitureRotation;

    private GameObject _currentFurniture;

    void Start()
    {
        ChangeFurniture(_startFurnitureIndex);
    }

    public void ChangeFurniture(int index)
    {
        if (_currentFurniture != null) Destroy(_currentFurniture);

        Quaternion newRotation = Quaternion.Euler(_furnitureRotation);

        _currentFurniture = Instantiate(_possibleFurniture[index], transform.position, newRotation);
    }
}
