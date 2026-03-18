using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ActionEntry
{
    public ActionType type;

    public GameObject thisObject;
    public GameObject otherObject;

    public bool activeState;

    public OpenDoor openDoor;
}