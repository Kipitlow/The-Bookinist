using NUnit.Framework.Internal;
using System;
using System.ComponentModel.Design;
using UnityEngine;

[Serializable]
public class ConditionEntry
{
    public ConditionType type;

    [Tooltip("this object")]
    public GameObject thisObject;
    public GameObject target;

    public string requiredTag;

    public ZoneDetector zone;
 
    public int layer;

    //public OnTouch touch 
}