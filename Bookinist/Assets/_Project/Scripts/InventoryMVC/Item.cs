using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/ItemScriptable" + "", order = 1)]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemSprite;
    public GameObject itemObtain;
    public GameObject itemDropoff;
}
