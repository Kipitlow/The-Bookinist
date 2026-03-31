[System.Serializable]
public class SlotModel
{
    public ItemData ItemData { get; private set; }
    public int Amount { get; private set; }

    public bool IsEmpty => ItemData == null || Amount <= 0;

    public bool CanStack(ItemData data)
    {
        return !IsEmpty
            && ItemData == data
            && ItemData.stackable
            && Amount < ItemData.maxStack;
    }

    public void Set(ItemData data, int amount)
    {
        ItemData = data;
        Amount = amount;
    }

    public void AddOne()
    {
        if (ItemData == null) return;
        Amount++;
    }

    public void RemoveOne()
    {
        Amount--;
        if (Amount <= 0)
            Clear();
    }

    public void Clear()
    {
        ItemData = null;
        Amount = 0;
    }
}