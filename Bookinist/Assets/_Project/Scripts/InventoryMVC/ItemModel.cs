using UnityEditor;
using UnityEngine;

public class ItemModel : MonoBehaviour
{
    #region Variables

    public Item itemScriptable;

    [HideInInspector] public string itemName;
    [HideInInspector] public Sprite itemSprite;
    [HideInInspector] public GameObject itemObtain;
    [HideInInspector] public GameObject itemDropoff;
    //[HideInInspector] public EventManager itemEventManager

    #endregion

    #region Unity Methods

    #endregion

    #region Methods

    public void SetScriptable(Item ItemScriptable)
    {
        itemScriptable = ItemScriptable;
        itemName = itemScriptable.itemName;
        itemSprite = itemScriptable.itemSprite;
    }
    /*
    public void GetEventManager(EventManager eventManager)
    {
        itemEventManager = eventManager;
    }
    */
    #endregion
}
