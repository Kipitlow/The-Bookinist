public class SlotData
{
    private bool _isFilled;
    private ItemModel _item;

    public bool IsFilled => _isFilled;
    public ItemModel Item => _item;

    public void SetItem(ItemModel newItem)
    {
        _item = newItem;
        _isFilled = true;
    }

    public void ClearItem()
    {
        _item = null;
        _isFilled = false;
    }
}