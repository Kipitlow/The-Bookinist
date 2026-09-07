using UnityEditor;
using UnityEngine;

public static  class AnchorTools
{
    /// <summary>
    /// Sets the anchors so they fit exactly the current rect size and position.
    /// </summary>
    [MenuItem("Tools/UI Anchors/Snap Anchors To Rect")]
    private static void SnapAnchorsToRect()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            RectTransform rect = obj.GetComponent<RectTransform>();
            if (rect == null) continue;

            RectTransform parent = rect.parent as RectTransform;
            if (parent == null) continue;

            Undo.RecordObject(rect, "Snap Anchors To Rect");

            Rect parentRect = parent.rect;

            Vector2 offsetMin = rect.offsetMin;
            Vector2 offsetMax = rect.offsetMax;

            Vector2 newAnchorMin = new Vector2(
                rect.anchorMin.x + offsetMin.x / parentRect.width,
                rect.anchorMin.y + offsetMin.y / parentRect.height
            );

            Vector2 newAnchorMax = new Vector2(
                rect.anchorMax.x + offsetMax.x / parentRect.width,
                rect.anchorMax.y + offsetMax.y / parentRect.height
            );

            rect.anchorMin = newAnchorMin;
            rect.anchorMax = newAnchorMax;

            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }
    }

    /// <summary>
    /// Sets the rect to perfectly match the current anchors.
    /// </summary>
    [MenuItem("Tools/UI Anchors/Snap Rect To Anchors")]
    private static void SnapRectToAnchors()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            RectTransform rect = obj.GetComponent<RectTransform>();
            if (rect == null) continue;

            Undo.RecordObject(rect, "Snap Rect To Anchors");

            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }
    }
}
