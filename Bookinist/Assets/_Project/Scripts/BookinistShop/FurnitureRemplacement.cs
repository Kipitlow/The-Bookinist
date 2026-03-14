using UnityEngine;

public class FurnitureRemplacement : MonoBehaviour
{
    [SerializeField] GameObject[] _possibleFurniture;
    [SerializeField] int _startFurnitureIndex;

    private GameObject _currentFurniture;

    void Start()
    {
        ChangeFurniture(_startFurnitureIndex);
    }

    public void ChangeFurniture(int index)
    {
        if (_currentFurniture != null) Destroy(_currentFurniture);

        _currentFurniture = Instantiate(_possibleFurniture[index], transform.position, Quaternion.identity);
    }
}
