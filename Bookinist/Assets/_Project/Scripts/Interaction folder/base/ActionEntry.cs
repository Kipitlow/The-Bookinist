using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ActionEntry
{
    public ActionType type;

    public GameObject thisObject;
    public GameObject target;

    public bool activeState;


    [Tooltip("the door you want to open")]
    public OpenDoor openDoor;

    public NPCDialogue npcDialogue;
    public NPCTalker npcTalker;
}