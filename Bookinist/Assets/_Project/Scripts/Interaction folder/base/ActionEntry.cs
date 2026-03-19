using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ActionEntry
{
    public ActionType type;

    public GameObject thisObject;


    [Tooltip("the object you want to activate")]
    public GameObject otherObject;

    public bool activeState;


    [Tooltip("the door you want to open")]
    public OpenDoor openDoor;
}