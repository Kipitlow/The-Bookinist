using System.Collections;
using UnityEngine;

public class ScriptStartAnimator : MonoBehaviour
{
    [SerializeField] private Animator _refAnimator;
    public void StartAnimator(string NameTrigger)
    {
        //_refAnimator.enabled = false;
        //_refAnimator.enabled = true;
        _refAnimator.Play(NameTrigger);
        //_refAnimator.SetTrigger(NameTrigger);
        //StartCoroutine(_coroutineAnimator(NameTrigger));
    }
    private IEnumerator _coroutineAnimator(string NameTrigger)
    {
        yield return new WaitForSeconds(0.1f);
        _refAnimator.enabled = false;
        yield return new WaitForSeconds(0.1f);
        _refAnimator.enabled = true;
        yield return new WaitForSeconds(0.1f);
        _refAnimator.SetTrigger(NameTrigger);
    }
}
