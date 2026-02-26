using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;
    
    public string Name => _name;
    public Sprite Icon => _icon;
}