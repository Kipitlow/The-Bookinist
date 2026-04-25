using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class BookActivation : MonoBehaviour
{
    [SerializeField] private GameObject _bookToActivate;
    [SerializeField] private Animator _bookToActivateAnimator;
    [SerializeField] private NPCTalker _npcTalker;
    [SerializeField] private LibraryManager _libManager;

    private void Awake()
    {
        _npcTalker.OnShowBook += NpcTalkerOnShowBook;
        _npcTalker.OnDialogEnd += NpcTalkerOnDialogEnd;
    }

    private void OnDestroy()
    {
        _npcTalker.OnShowBook -= NpcTalkerOnShowBook;
        _npcTalker.OnDialogEnd -= NpcTalkerOnDialogEnd;
    }

    private void NpcTalkerOnDialogEnd()
    {
        _bookToActivateAnimator.SetTrigger("OpenBook");
    }

    private void NpcTalkerOnShowBook()
    {
        _libManager.SpawnBook(0); //sadly hardcode here
    }
}
