using UnityEngine;
using System.Collections.Generic;

public class GambleManager : MonoBehaviour
{
    [SerializeField] private List<ScriptableObject> _itemsUnlockable;
    [SerializeField] private GameObject _displayObtained;

    public void Gamble()
    {
        DisplayScriptable(PullItem());
    }

    public ScriptableObject PullItem()
    {
        RandomService rng = new RandomService();
        return _itemsUnlockable[rng.Range(0, _itemsUnlockable.Count)];
    }

    public void DisplayScriptable(ScriptableObject itemObtained)
    {
        Debug.Log(itemObtained.name);
    }

}
