using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomShopManager : MonoBehaviour
{
    [SerializeField] List<SO_FurnitureList> _customFurnitureList;
    [SerializeField] List<GameObject> _spawnPointList;

    [SerializeField] ChangeCustomView _changeCustomView;

    [SerializeField] GameObject _horizontalPanel;
    [SerializeField] Button _buttonPrefab;
    private List<List<Button>> _furnitureButtons;

    private GameObject _currentFurniture;

    private int _currentIndexCamMemory;

    private void Awake()
    {
        _changeCustomView.OnViewChanged += ChangeButtons;
    }

    private void OnDestroy()
    {
        _changeCustomView.OnViewChanged -= ChangeButtons;
    }

    private void Start()
    {
        _currentFurniture = null;
        _furnitureButtons = new List<List<Button>>();

        for (int i = 0; i < _customFurnitureList.Count; i++)
        {
            _furnitureButtons.Add(new List<Button>());
            for (int j = 0; j < _customFurnitureList[i].GetFurnitureListLength(); j++)
            {
                Button button = Instantiate(_buttonPrefab, _horizontalPanel.transform);
                _furnitureButtons[i].Add(button);
                button.gameObject.SetActive(false);
            }
        }
    }
    private void ChangeButtons(int index, int offset)
    {
        //for (int i = 0; i < _furnitureButtons[index].Count; i++)
        //{
        //    _furnitureButtons[index][i] = Instantiate(_buttonPrefab);
        //}
        Debug.Log(index);
    }
    public void AddObject(GameObject newObject)
    {
        _customFurnitureList[_changeCustomView.GetCurrentIndexView()].AddFurniture(newObject);
    }

    public void ChangeFurniture(int index)
    {
        if (_customFurnitureList[_changeCustomView.GetCurrentIndexView()].UpdateCurrentFurnitureIndex(index) == false) return;

        if (_currentFurniture != null && _currentIndexCamMemory == _changeCustomView.GetCurrentIndexView()) Destroy(_currentFurniture);

        Quaternion newRotation = Quaternion.Euler(_customFurnitureList[_changeCustomView.GetCurrentIndexView()].GetFurnitureRotation());

        _currentFurniture = Instantiate(_customFurnitureList[_changeCustomView.GetCurrentIndexView()].GetFurniture(index), _spawnPointList[_changeCustomView.GetCurrentIndexView()].transform.position, newRotation);

        UpdateCurrentIndexCamMemory();
    }

    private void UpdateCurrentIndexCamMemory()
    {
        _currentIndexCamMemory = _changeCustomView.GetCurrentIndexView();
    }
}
