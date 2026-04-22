using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class BookActivation : MonoBehaviour
{
    [SerializeField] private GameObject _bookToActivate;
    [SerializeField] private NPCTalker _npcTalker;

    private void Awake()
    {
        _npcTalker.OnShowBook += NpcTalkerOnShowBook;
        _npcTalker.OnDialogEnd += NpcTalkerOnDialogEnd;
    }

    private void NpcTalkerOnDialogEnd()
    {
        _bookToActivate.GetComponent<Button>().interactable = true;
    }

    private void NpcTalkerOnShowBook()
    {
        _bookToActivate.SetActive(true);
    }
}
