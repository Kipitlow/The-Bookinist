using UnityEngine;

[System.Serializable]
public class InteractionContext
{
    public GameObject instigator;
    public GameObject target;

    public bool isTouchEvent;

    public Item item;
}