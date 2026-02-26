using UnityEngine;

public class ItemModel : MonoBehaviour
{
    [SerializeField] private ItemData _itemData;
    private Transform _originalParent;
    private bool _isDragging;

    public ItemData ItemData => _itemData;
    public Transform OriginalParent => _originalParent;
    public bool IsDragging => _isDragging;

    public ItemModel (ItemData itemData)
    {
        _itemData = itemData;
    }

    public void SetDragging(bool isDragging) 
    { 
        _isDragging = isDragging; 
    }

    public void SetOriginalParent(Transform originalParent) 
    {
        _originalParent = originalParent;
    }
}