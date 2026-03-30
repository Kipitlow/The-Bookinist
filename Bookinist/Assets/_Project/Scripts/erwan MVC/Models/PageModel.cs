public class PageModel
{
    public ItemData PlacedItem { get; private set; }

    public bool IsEmpty => PlacedItem == null;

    public bool TryPlace(ItemData item)
    {
        if (!IsEmpty || item == null || !item.canBePlacedOnPage)
            return false;

        PlacedItem = item;
        return true;
    }

    public ItemData TryTakeBack()
    {
        if (PlacedItem == null || !PlacedItem.canBeTakenBack)
            return null;

        ItemData item = PlacedItem;
        PlacedItem = null;
        return item;
    }
}