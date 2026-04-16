using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(InteractionRunner))]

public class Pickable : MonoBehaviour
{
    [SerializeField] private Item _item;
    [SerializeField] private InventoryController _invController;
    
    private void Start()
    {
        if (_invController == null) _invController = GameObject.Find("InventoryManager").GetComponent<InventoryController>();
    }

    public void SetItem(Item item) {  _item = item; }

    public void Pick(GameObject objetclicked)
    {
        if (_invController.IsInventoryHasPlace())
        {
            Destroy(objetclicked);
            _invController.AddInventoryItem(_item);
        }
    }
}
