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

    private void Start()
    {
        if (GameManager.Instance.bookFinish)
            _bookToActivateAnimator.SetTrigger("OpenBook");
    }

    private void OnDestroy()
    {
        _npcTalker.OnShowBook -= NpcTalkerOnShowBook;
        _npcTalker.OnDialogEnd -= NpcTalkerOnDialogEnd;
    }

    private void NpcTalkerOnDialogEnd(bool isAppearing)
    {
        if (isAppearing)
            _bookToActivateAnimator.SetTrigger("OpenBook");
        else
        {
            _bookToActivateAnimator.SetTrigger("CloseBook");
            StartCoroutine(WaitToDestroy(0.7f, _bookToActivate));
        }
    }

    private void NpcTalkerOnShowBook(bool isAppearing)
    {
        if (isAppearing)
        {
            _bookToActivateAnimator.SetTrigger("OpenBook");
        }

        _libManager.SpawnBook(0, isAppearing); //sadly hardcode here
    }

    IEnumerator WaitToDestroy(float delay, GameObject targetToDestroy)
    {
        yield return new WaitForSeconds(delay);

        Destroy(targetToDestroy);
    }
}
