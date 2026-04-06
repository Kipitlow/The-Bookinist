using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ActionEntry
{
    #region Variables

    public ActionType type;

    [Tooltip("quel est ta cible")]
    public GameObject target;

    [Tooltip("veut tu activer ou desactiver ton objet")]
    public bool activeState;

    [Tooltip("objet qui porte le sript")]
    public Slot slot;

    [Tooltip("the door you want to open")]
    public OpenDoor openDoor;

    [Tooltip("quel est le dialogue tu tu veux activer")]
    public NPCDialogue npcDialogue;

    [Tooltip("objet qui porte le sript")]
    public NPCTalker npcTalker;

    [Tooltip("apelle une fonction voulue")]
    public UnityEvent onExecute;

    [Tooltip("objet qui porte le sript")]
    public CycleThroughSprite cycleThroughSprite;

    [Tooltip("est-ce que la list reviens au premier sprite ou reste bloqué sur le dernier")]
    public bool cycle;

    [Tooltip("liste de sprites à cycler")]
    public List<Sprite> sprites;

    [Tooltip("objet qui porte le sript")]
    public MoveObject move;

    [Tooltip("Offset sur l'axe X")]
    public float offsetX;

    [Tooltip("Offset sur l'axe Y")]
    public float offsetY;

    [Tooltip("InventaireManager")]
    public Pickable pickable;

    #endregion
}