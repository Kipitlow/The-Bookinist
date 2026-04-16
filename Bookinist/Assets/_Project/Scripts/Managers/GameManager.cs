using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<ScriptableObject> _itemsUnlockable;
    [SerializeField] private GameObject _displayObtained;

    public bool isBookStarted;
    public bool isBookEnded;

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

    public bool IsBookStarted()
    {
        return isBookStarted;
    }

    public bool IsBookEnded()
    {
        return isBookEnded;
    }

    public void StartBook()
    {
        isBookStarted = true;
        isBookEnded = false;
    }

    public void EndBook()
    {
        isBookEnded = true;
    }

}
