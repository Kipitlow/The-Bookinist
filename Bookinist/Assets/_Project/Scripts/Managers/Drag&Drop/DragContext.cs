/// <summary>
/// Conteneur statique léger qui stocke l'état du drag en cours.
/// Accessible depuis n'importe quel script sans référence directe.
/// </summary>
public static class DragContext
{
    /// <summary>L'item actuellement en cours de drag. Null si aucun drag actif.</summary>
    public static Item DraggedItem { get; private set; }

    /// <summary>Le controller qui a initié le drag (pour callbacks).</summary>
    public static ItemController SourceController { get; private set; }

    public static bool IsDragging => DraggedItem != null;

    public static void BeginDrag(Item item, ItemController source)
    {
        DraggedItem = item;
        SourceController = source;
    }

    public static void EndDrag()
    {
        DraggedItem = null;
        SourceController = null;
    }
}