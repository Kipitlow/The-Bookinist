using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScriptBalance : MonoBehaviour
{

    #region Class Serializable

    [Serializable]
    private class ItemWeightEntry
    {
        public Item item;
        public int weight = 1;
    }

    private class DepositedItem
    {

        public Item Item;
        public GameObject Instance;
        public int Weight;

        public DepositedItem(Item item, GameObject instance, int weight)
        {
            Item = item;
            Instance = instance;
            Weight = weight;
        }
    }

    #endregion

    #region Variables

    [Header("Scene References")]
    [SerializeField] private Transform _itemSpawnPoint;
    [SerializeField] private Transform _counterweightSpawnPoint;
    [SerializeField] private Transform _rewardSpawnPoint;
    [SerializeField] private Transform _spawnedItemsRoot;
    [SerializeField] private InventoryController _inventoryController;

    [Header("Optional Balance Visuals")]
    [SerializeField] private Transform _balancePivot;
    [SerializeField] private Transform _balanceSprite;
    [SerializeField] private Transform _plateau1;
    [SerializeField] private Transform _plateau2;

    [Header("Prefabs / Items")]
    [SerializeField] private GameObject _balanceItemPrefab;
    [SerializeField] private GameObject _rewardLyrePrefab;
    [SerializeField] private Item _lyreItem;

    [Header("Rules")]
    [SerializeField] private int _maxItems = 3;
    [SerializeField] private int _targetWeight = 100;
    [SerializeField] private int _counterweightWeight = 100;
    [SerializeField] private float _solveDelay = 0.5f;

    [Header("Information On Weight")]
    [SerializeField] private TextMeshPro _textValue;
    [SerializeField] private GameObject _resetBalance;


    [Header("Spawned Item Visual")]
    [SerializeField] private Vector3 _spawnedItemScale = new Vector3(0.1f, 0.1f, 0.1f);

    [Header("Item Weights")]
    [SerializeField] private List<ItemWeightEntry> _itemWeights = new();

    private readonly Dictionary<string, int> _weightByItem = new();
    private readonly List<DepositedItem> _depositedItems = new();

    private GameObject _counterweightInstance;
    private bool _isSolved;
    private bool _isSolving;
    public int _currentWeight { get; private set; }

    public int CurrentWeight => _currentWeight;
    public int DepositedCount => _depositedItems.Count;
    public bool IsSolved => _isSolved;
    public bool IsFull => _depositedItems.Count >= _maxItems;

    #endregion

    #region Unity Methods
    private void Awake()
    {
        RebuildWeightTable();
    }

    private void Update()
    {
        UpdateBalanceVisuals();
    }

    #endregion

    #region Methods

    #region Try

    public bool TryAddItem(Item item)
    {
        if (!CanAcceptItem(item))
            return false;

        TrySpawnCounterWeight();

        if (!_weightByItem.TryGetValue(item.itemName, out int weight))
            return false;

        GameObject instance = SpawnBalanceObject(item, weight, _itemSpawnPoint);
        if (instance == null)
            return false;

        _depositedItems.Add(new DepositedItem(item, instance, weight));
        _currentWeight += weight;

        if (_currentWeight >= _targetWeight)
        {
            StartSolveFlow();
        }

        return true;
    }


    public void TryTakeBackItems()
    {
        if (!CanTakeBackAtLeastOneItem())
            return;

        int returnedCount = 0;

        for (int i = _depositedItems.Count - 1; i >= 0; i--)
        {
            if (_inventoryController == null || !_inventoryController.IsInventoryHasPlace())
                break;

            DepositedItem depositedItem = _depositedItems[i];

            _inventoryController.AddInventoryItem(depositedItem.Item);
            _currentWeight -= depositedItem.Weight;

            if (depositedItem.Instance != null)
            {
                Destroy(depositedItem.Instance);
            }

            _depositedItems.RemoveAt(i);
            returnedCount++;
        }

        if (_depositedItems.Count == 0)
        {
            _currentWeight = 0;
            DestroyCounterweight();
            ResetBalanceVisuals();
        }
        return;
    }

    #endregion

    #region Check

    public bool CanAcceptItem(Item item)
    {
        #region FaireLeDialogue
        //Debug.Log($"<color=green> Name Tester: {item.name} </color>");

        //_interactionRunner.CallTry();
        #endregion
        if (_isSolved || _isSolving)
            return false;

        if (item == null)
            return false;

        if (IsFull)
            return false;

        if (item.itemSprite == null)
            return false;

        return _weightByItem.ContainsKey(item.itemName);
        
    }

    public bool ContainsItem(Item item)
    {
        if (item == null)
            return false;

        for (int i = 0; i < _depositedItems.Count; i++)
        {
            if (_depositedItems[i].Item == item)
                return true;
        }

        return false;
    }

    public bool CanTakeBackAtLeastOneItem()
    {
        if (_isSolved || _isSolving)
            return false;

        if (_depositedItems.Count == 0)
            return false;

        return _inventoryController != null && _inventoryController.IsInventoryHasPlace();
    }

    #endregion

    #region Reset Balance

    public void ResetBalance()
    {
        StopAllCoroutines();

        _isSolved = false;
        _isSolving = false;
        _currentWeight = 0;
        _textValue.text = $"{_currentWeight} >= 100Value";
        ClearDepositedItems();
        DestroyCounterweight();
        ResetBalanceVisuals();
    }

    private void ResetBalanceVisuals()
    {
        if (_balancePivot != null)
        {
            _balancePivot.localRotation = Quaternion.identity;
        }
    }

    #endregion

    #region Safety

    // clean le dictionaire au awake si des poids son null ou autre
    public void RebuildWeightTable()
    {
        _weightByItem.Clear();

        for (int i = 0; i < _itemWeights.Count; i++)
        {
            ItemWeightEntry entry = _itemWeights[i];
            if (entry == null || entry.item == null)
                continue;

            _weightByItem[entry.item.itemName] = Mathf.Max(0, entry.weight);
        }
    }

#endregion

    #region Solve

    private void StartSolveFlow()
    {
        if (_isSolved || _isSolving)
            return;

        _isSolving = true;

        if (_solveDelay <= 0f)
        {
            CompleteSolve();
            return;
        }

        StartCoroutine(SolveRoutine());
    }

    private IEnumerator SolveRoutine()
    {
        yield return new WaitForSeconds(_solveDelay);
        CompleteSolve();
    }

    private void CompleteSolve()
    {
        _isSolving = false;
        _isSolved = true;

        SpawnRewardLyre();
        ClearDepositedItems();
        DestroyCounterweight();

        _currentWeight = 0;
        ResetBalanceVisuals();
    }

#endregion

    #region Spawn Object

    private void TrySpawnCounterWeight()
    {
        if (_counterweightInstance != null || _counterweightSpawnPoint == null || _lyreItem == null)
            return;

        _counterweightInstance = SpawnBalanceObject(_lyreItem, _counterweightWeight, _counterweightSpawnPoint);
    }

    private GameObject SpawnBalanceObject(Item item, int weight, Transform spawnPoint)
    {
        if (_balanceItemPrefab == null || spawnPoint == null || item == null)
            return null;

        GameObject instance;

        if (_spawnedItemsRoot != null)
            instance = Instantiate(_balanceItemPrefab, spawnPoint.position, spawnPoint.rotation, _spawnedItemsRoot);
        else
            instance = Instantiate(_balanceItemPrefab, spawnPoint.position, spawnPoint.rotation);

        ConfigureSpawnedObject(instance, item, weight);
        return instance;
    }

    private void ConfigureSpawnedObject(GameObject instance, Item item, int weight)
    {
        if (instance == null || item == null)
            return;

        instance.transform.localScale = _spawnedItemScale;

        SpriteRenderer spriteRenderer = instance.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = item.itemSprite;
        }

        Rigidbody2D rigidbody2D = instance.GetComponent<Rigidbody2D>();
        if (rigidbody2D != null)
        {
            rigidbody2D.mass = weight;
        }

        BoxCollider2D boxCollider2D = instance.GetComponent<BoxCollider2D>();
        if (boxCollider2D != null && item.itemSprite != null)
        {
            boxCollider2D.size = item.itemSprite.bounds.size;
            boxCollider2D.offset = item.itemSprite.bounds.center;
        }
    }

#endregion

    #region Lyre + Clean

    private void SpawnRewardLyre()
    {
        if (_rewardSpawnPoint == null)
            return;

        if (_rewardLyrePrefab != null)
        {
            Instantiate(_rewardLyrePrefab, _rewardSpawnPoint.position, _rewardSpawnPoint.rotation);
            return;
        }

        if (_lyreItem != null && _balanceItemPrefab != null)
        {
            GameObject reward = Instantiate(_balanceItemPrefab, _rewardSpawnPoint.position, _rewardSpawnPoint.rotation);
            ConfigureSpawnedObject(reward, _lyreItem, _counterweightWeight);

            Rigidbody2D rigidbody2D = reward.GetComponent<Rigidbody2D>();
            if (rigidbody2D != null)
            {
                rigidbody2D.mass = _counterweightWeight;
            }
        }
    }

    private void ClearDepositedItems()
    {
        for (int i = 0; i < _depositedItems.Count; i++)
        {
            if (_depositedItems[i].Instance != null)
            {
                Destroy(_depositedItems[i].Instance);
            }
        }

        _depositedItems.Clear();
    }

    private void DestroyCounterweight()
    {
        if (_counterweightInstance == null)
            return;

        Destroy(_counterweightInstance);
        _counterweightInstance = null;
    }

    #endregion

    #region Update Visual

    private void UpdateBalanceVisuals()
    {
        if (_balancePivot != null && _balanceSprite != null)
        {
            Vector3 euler = _balancePivot.eulerAngles;
            _balanceSprite.localRotation = Quaternion.Euler(euler.x, euler.y, euler.z);
        }

        if (_plateau1 != null)
        {
            _plateau1.rotation = Quaternion.identity;
        }

        if (_plateau2 != null)
        {
            _plateau2.rotation = Quaternion.identity;
        }

        _textValue.text = $"{_currentWeight} / 100";

        if (_currentWeight >  0) _resetBalance.SetActive(true);
        else _resetBalance.SetActive(false);
    }

    #endregion

    #endregion
}