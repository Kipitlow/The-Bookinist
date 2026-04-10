/// <summary>
/// Conteneur statique léger qui stocke l'état du drag en cours.
/// Accessible depuis n'importe quel script sans référence directe.
/// </summary>
public static class DragContext
{
    public static Item DraggedItem { get; private set; }
    public static ItemController SourceController { get; private set; }
    public static bool IsDragging => DraggedItem != null;

    public static bool WasDroppedThisFrame { get; private set; }

    public static void BeginDrag(Item item, ItemController source)
    {
        DraggedItem = item;
        SourceController = source;
        WasDroppedThisFrame = false;
    }

    public static void EndDrag()
    {
        DraggedItem = null;
        SourceController = null;
        WasDroppedThisFrame = true;
    }

    public static void ConsumeDropFlag()
    {
        WasDroppedThisFrame = false;
    }
}