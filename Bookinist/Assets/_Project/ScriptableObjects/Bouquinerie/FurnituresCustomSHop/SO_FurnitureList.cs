using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_FurnitureList", menuName = "Scriptable Objects/Bouquinerie/FurnituresCustomSHop/SO_FurnitureList")]
public class SO_FurnitureList : ScriptableObject
{
    [SerializeField] List<GameObject> _furnitureList;
    [SerializeField] Vector3 _furnitureRotationList;

    [SerializeField] int _currentFurnitureIndex;

    public GameObject GetFurniture(int index)
    {
        return _furnitureList[index];
    }

    public int GetFurnitureListLength()
    {
        return _furnitureList.Count;
    }

    public Vector3 GetFurnitureRotation()
    {
        return _furnitureRotationList;
    }

    public int GetCurrentFurnitureIndex()
    {
        return _currentFurnitureIndex;
    }

    public void AddFurniture(GameObject newFurniture)
    {
        _furnitureList.Add(newFurniture);
    }

    public bool UpdateCurrentFurnitureIndex(int newIndex)
    {
        if (newIndex < 0 || newIndex >= _furnitureList.Count) return false;

        _currentFurnitureIndex = newIndex;

        return true;
    }
}
