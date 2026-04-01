using UnityEngine;

public class ItemModel : MonoBehaviour
{
    #region Variables

    [SerializeField] Item itemScriptable;

    [HideInInspector] public string itemName;
    [HideInInspector] public Sprite itemSprite;
    [HideInInspector] public GameObject itemObtain;
    [HideInInspector] public GameObject itemDropoff;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        itemName = itemScriptable.itemName;
        itemSprite = itemScriptable.itemSprite;
        itemObtain = itemScriptable.itemObtain;
        itemDropoff = itemScriptable.itemDropoff;
    }

    #endregion

    #region Methods

    public void SetScriptable(Item ItemScriptable)
    {
        itemScriptable = ItemScriptable;
        itemName = itemScriptable.itemName;
        itemSprite = itemScriptable.itemSprite;
        itemObtain = itemScriptable.itemObtain;
        itemDropoff = itemScriptable.itemDropoff;
    }

    #endregion
}
