using System.Collections.Generic;
using UnityEngine;

public class Machine1ItemsDict : MonoBehaviour
{
    [SerializeField] private Dictionary<string, ScriptableObject> _machine1ItemsDict;

    public ScriptableObject IDToScriptable(string itemID)
    {
        _machine1ItemsDict.TryGetValue(itemID, out ScriptableObject scriptable);
        return scriptable;
    }
}
