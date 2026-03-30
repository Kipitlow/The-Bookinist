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

    [Tooltip("the object you want to interact with")]
    public GameObject otherObject;

    public string requiredTag;

    public ZoneDetector zone;
 
    public int layer;

    //public OnTouch touch 
}