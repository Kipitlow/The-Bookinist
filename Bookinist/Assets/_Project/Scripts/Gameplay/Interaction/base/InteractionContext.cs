using UnityEngine;

[System.Serializable]
public class InteractionContext
{
    #region Variables

    public GameObject instigator;
    public GameObject target;

    public bool isTouchEvent;

    #endregion
}