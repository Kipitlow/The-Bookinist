using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomShopManager : MonoBehaviour
{
    #region Variables

    public static CustomShopManager Instance { get; private set; }

    [SerializeField] List<SO_FurnitureList> _customFurnitureList;
    [SerializeField] List<GameObject> _spawnPointList;
    [SerializeField] CamManager _changeCustomView;

    [SerializeField] GameObject _horizontalPanelPrefab;
    [SerializeField] GameObject _horizontalPanelParent;
    [SerializeField] GameObject _buttonPrefab;
    private List<List<GameObject>> _furnitureButtons;

    private List<GameObject> _currentFurnitureList;
    private List<GameObject> _horizontalPanelMemList;

    private bool _isAlreadySeeCustomShop;
    private int _previousIndexFurnitureActivated;

    #endregion

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

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
                CreateButton(i, j);
            }

            _horizontalPanelMemList[i].SetActive(false);
        }

        _horizontalPanelMemList[0].SetActive(true);
    }

    /// <summary>
    /// Crée un bouton pour le meuble à l'index donné dans le slot slotIndex.
    /// Extrait en méthode pour être réutilisé à l'achat.
    /// </summary>
    private void CreateButton(int slotIndex, int furnitureIndex)
    {
        int capturedIndex = furnitureIndex;

        GameObject button = Instantiate(_buttonPrefab, _horizontalPanelMemList[slotIndex].transform);
        button.GetComponent<Button>().onClick.RemoveAllListeners();
        button.GetComponent<Button>().onClick.AddListener(() => ChangeFurniture(capturedIndex));

        _furnitureButtons[slotIndex].Add(button);
    }

    private void ChangeButtons(int index, int offset)
    {
        if (_isAlreadySeeCustomShop)
            _horizontalPanelMemList[_previousIndexFurnitureActivated].SetActive(false);

        _horizontalPanelMemList[index].SetActive(true);
        _previousIndexFurnitureActivated = index;
        _isAlreadySeeCustomShop = true;
    }

    /// <summary>
    /// Appelé par ShopItemUI à l'achat d'un meuble.
    /// Ajoute le SO à la liste du slot actif ET crée le bouton correspondant dynamiquement.
    /// </summary>
    public void AddObject(ShopItemData newObject)
    {
        int currentSlot = _changeCustomView.GetCurrentIndexView();

        _customFurnitureList[currentSlot].AddFurniture(newObject);

        // FIX : création du bouton au moment de l'achat
        int newFurnitureIndex = _customFurnitureList[currentSlot].GetFurnitureListLength() - 1;
        CreateButton(currentSlot, newFurnitureIndex);
    }

    public void ChangeFurniture(int index)
    {
        int currentIndex = _changeCustomView.GetCurrentIndexView();

        if (_customFurnitureList[currentIndex].UpdateCurrentFurnitureIndex(index) == false) return;

        if (_currentFurnitureList[currentIndex] != null)
            Destroy(_currentFurnitureList[currentIndex]);

        Quaternion newRotation = Quaternion.Euler(_customFurnitureList[currentIndex].GetFurnitureRotation());

        _currentFurnitureList[currentIndex] = Instantiate(
            _customFurnitureList[currentIndex].GetFurniture(index),
            _spawnPointList[currentIndex].transform.position,
            newRotation
        );
    }
}