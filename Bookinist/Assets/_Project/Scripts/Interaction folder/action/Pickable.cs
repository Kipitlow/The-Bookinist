using UnityEngine;
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(InteractionRunner))]

public class Pickable : MonoBehaviour
{
    [SerializeField] private Item _item;
    [SerializeField] private InventoryController _invController;

    private void Awake()
    {
        if (_invController = null)
        _invController = GameObject.Find("InventorySystem").GetComponent<InventoryController>();
    }

    public void Pick(GameObject objetclicked)
    {
        Destroy(objetclicked);
        _invController.AddInventoryItem(_item);
    }
}
