using UnityEngine;

/// <summary>
/// Composant rendant un GameObject récupérable et ajoutant l'item à l'inventaire.
/// </summary>

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(InteractionRunner))]
public class Pickable : MonoBehaviour
{
    #region Variables

    [SerializeField] private Item _item;
    [SerializeField] private InventoryController _invController;

    #endregion

    #region Methods

    public void Pick(GameObject objetclicked)
    {
        Destroy(objetclicked);
        _invController.AddInventoryItem(_item);
    }

    #endregion
}
