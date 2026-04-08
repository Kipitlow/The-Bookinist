using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomShopManager : MonoBehaviour
{
    [SerializeField] List<SO_FurnitureList> _customFurnitureList;
    [SerializeField] List<GameObject> _spawnPointList;

    [SerializeField] ChangeCustomView _changeCustomView;

    [SerializeField] GameObject _horizontalPanelPrefab;
    [SerializeField] GameObject _horizontalPanelParent;
    [SerializeField] GameObject _buttonPrefab;
    private List<List<GameObject>> _furnitureButtons;

    private List<GameObject> _currentFurnitureList;

    private List<GameObject> _horizontalPanelMemList;

    private bool _isAlreadySeeCustomShop;
    private int _previousIndexFurnitureActivated;

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
        _currentFurnitureList = new List<GameObject>();
        _furnitureButtons = new List<List<GameObject>>();
        _horizontalPanelMemList = new List<GameObject>();

        for (int i = 0; i < _customFurnitureList.Count; i++)
        {
            _currentFurnitureList.Add(null);
            _furnitureButtons.Add(new List<GameObject>());

            GameObject horizontalPanel = Instantiate(_horizontalPanelPrefab, _horizontalPanelParent.transform);

            _horizontalPanelMemList.Add(horizontalPanel);

            for (int j = 0; j < _customFurnitureList[i].GetFurnitureListLength(); j++)
            {
                int capturedIndex = j;

                GameObject button = Instantiate(_buttonPrefab, _horizontalPanelMemList[i].transform);

                button.GetComponent<Button>().onClick.RemoveAllListeners();
                button.GetComponent<Button>().onClick.AddListener(() => ChangeFurniture(capturedIndex));

                //button.GetComponent<MeshRenderer>().material = _customFurnitureList[j].GetFurniture(i).GetComponent<MeshRenderer>().material;
                _furnitureButtons[i].Add(button);
                //button.gameObject.SetActive(false);

            }

            _horizontalPanelMemList[i].SetActive(false);
        }

        _horizontalPanelMemList[0].SetActive(true);
    }
    private void ChangeButtons(int index, int offset)
    {
        if (_isAlreadySeeCustomShop)
        {
            _horizontalPanelMemList[_previousIndexFurnitureActivated].SetActive(false);
        }
        
        _horizontalPanelMemList[index].SetActive(true);

        _previousIndexFurnitureActivated = index;

        _isAlreadySeeCustomShop = true;

        Debug.Log(index);
    }
    public void AddObject(GameObject newObject)
    {
        _customFurnitureList[_changeCustomView.GetCurrentIndexView()].AddFurniture(newObject);
    }

    public void ChangeFurniture(int index)
    {
        Debug.Log(index);

        int currentIndex = _changeCustomView.GetCurrentIndexView();

        if (_customFurnitureList[currentIndex].UpdateCurrentFurnitureIndex(index) == false) return;

        if (_currentFurnitureList[currentIndex] != null) 
            Destroy(_currentFurnitureList[currentIndex]);

        Quaternion newRotation = Quaternion.Euler(_customFurnitureList[currentIndex].GetFurnitureRotation());

        _currentFurnitureList[currentIndex] = Instantiate(_customFurnitureList[currentIndex].GetFurniture(index), _spawnPointList[currentIndex].transform.position, newRotation);

        //UpdateCurrentIndexCamMemory();
    }

    //private void UpdateCurrentIndexCamMemory()
    //{
    //    _currentIndexCamMemory = _changeCustomView.GetCurrentIndexView();
    //}
}
