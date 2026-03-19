using System;
using UnityEngine;

[Serializable]
public class ConditionEntry
{
    public ConditionType type;

    public GameObject thisObject;
    public GameObject otherObject;

    public string requiredTag;

    public ZoneDetector zone;
 
    public int layer;
}