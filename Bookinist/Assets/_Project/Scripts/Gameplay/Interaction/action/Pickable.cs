using UnityEngine;
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(InteractionRunner))]

public class Pickable : MonoBehaviour
{
    [SerializeField] private Item _item;
    [SerializeField] private InventoryController _invController;

    public void Pick(GameObject objetclicked)
    {
        Destroy(objetclicked);
        _invController.AddInventoryItem(_item);
    }
}
