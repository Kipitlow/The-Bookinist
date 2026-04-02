using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;



[Serializable]
public class ActionEntry
{
    public ActionType type;

    [Tooltip("quel est ta cible")]
    public GameObject target;

    [Tooltip("veut tu activer ou desactiver ton objet")]
    public bool activeState;

    [Tooltip("quel est le slot que tu veux remplire")]
    public Slot slot;

    [Tooltip("the door you want to open")]
    public OpenDoor openDoor;

    [Tooltip("quel est le dialogue tu tu veux activer")]
    public NPCDialogue npcDialogue;

    [Tooltip("quel est le npc que tu veux faire parler")]
    public NPCTalker npcTalker;

    [Tooltip("apelle une fonction voulue")]
    public UnityEvent onExecute;

    [Tooltip("script sur lequel le sprite cycle")]
    public CycleThroughSprite cycleThroughSprite;

    [Tooltip("est-ce que la list reviens au premier sprite ou reste bloqué sur le dernier")]
    public bool cycle;

    [Tooltip("apelle une fonction voulue")]
    public List<Sprite> sprites;
}