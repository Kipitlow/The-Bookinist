using UnityEngine;

/// <summary>
/// Model wrapper pour un ScriptableObject Item utilisé par la vue.
/// </summary>
public class ItemModel : MonoBehaviour
{
    #region Variables

    public Item itemScriptable;

    [HideInInspector] public string itemName;
    [HideInInspector] public Sprite itemSprite;
    [HideInInspector] public GameObject itemObtain;
    [HideInInspector] public GameObject itemDropoff;

    #endregion

    #region Methods

    public void SetScriptable(Item itemScriptable)
    {
        this.itemScriptable = itemScriptable;
        if (itemScriptable != null)
        {
            itemName = itemScriptable.itemName;
            itemSprite = itemScriptable.itemSprite;
        }
    }

    #endregion
}
