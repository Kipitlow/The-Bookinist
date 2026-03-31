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

    public OpenDoor openDoor;

    public NPCDialogue npcDialogue;
    public NPCTalker npcTalker;
}