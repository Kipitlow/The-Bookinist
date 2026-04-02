using NUnit.Framework.Internal;
using System;
using System.ComponentModel.Design;
using UnityEngine;

[Serializable]
public class ConditionEntry
{
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

    [Tooltip("slot que tu veux verifier")]
    public Slot slot;

    [Tooltip("verifier l'etat du slot")]
    public bool shouldBeEmpty;

    [Tooltip("SO de l'item")]
    public Item item;

    [Tooltip("Script qui compare les SO")]
    public SelectedItemIsWanted selectedItemIsWanted;

    [Tooltip("perso sur lequel le dialogue doit avoir commencé")]
    public NPCTalker npcTalker;

    [Tooltip("script de cycle de l'objet que tu veux verifier")]
    public CycleThroughSprite cycleThroughSprite;

    [Tooltip("frame que tu veux check")]
    public int WantedFrame;

    [Tooltip("si tu veux verifier si c'est sur la frame et celles d'apres")]
    public bool trueIfMore;


}