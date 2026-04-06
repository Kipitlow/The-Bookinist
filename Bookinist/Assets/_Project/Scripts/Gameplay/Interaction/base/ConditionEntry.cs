using System;
using UnityEngine;

[Serializable]
public class ConditionEntry
{
    #region Variables

    public ConditionType type;

    [Tooltip("objet ciblé")]
    public GameObject target;

    [Tooltip("tag requis")]
    public string requiredTag;

    [Tooltip("zone de detection")]
    public ZoneDetector zone;

    [Tooltip("page sur laquel tu veux que l'objet soit")]
    public Page checkedPage;

    [Tooltip("script de detection du layer")]
    public LayerDetector layerDetector;

    [Tooltip("objet que tu veux check")]
    public Slot slot;

    [Tooltip("verifier l'etat du slot")]
    public bool shouldBeEmpty;

    [Tooltip("SO de l'item")]
    public Item item;

    [Tooltip("Script qui compare les SO")]
    public SelectedItemIsWanted selectedItemIsWanted;

    [Tooltip("objet que tu veux check")]
    public NPCTalker npcTalker;

    [Tooltip("objet que tu veux check")]
    public CycleThroughSprite cycleThroughSprite;

    [Tooltip("frame que tu veux check")]
    public int wantedFrame;

    [Tooltip("si tu veux verifier si c'est sur la frame et celles d'apres")]
    public bool trueIfMore;

    [Tooltip("objet que tu veux check")]
    public MoveObject move;

    [Tooltip("si tu veux detecter que l'objet a bougé ou non")]
    public bool hasMoved;

    [Tooltip("check how many times object has moved")]
    public int howManyTimes;

    #endregion
}