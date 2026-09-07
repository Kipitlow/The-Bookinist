using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Book", menuName = "Scriptable Objects/SO_Book")]
public class SO_Book : ScriptableObject
{
    
    public enum BookTitle
    {
        Orpheus
    }

    [SerializeField] private bool _isUnlocked;

    [SerializeField] private List<bool> _isRunFinishedList;
    [SerializeField] private Sprite _sprite;
}
