using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] GameObject[] _possibleFurniture;
    [SerializeField] int _startFurnitureIndex;

    void Start()
    {
        ChangeFurniture(_startFurnitureIndex);
    }

    public void ChangeFurniture(int index)
    {
        Instantiate(_possibleFurniture[index], transform.position, Quaternion.identity);
    }
}
