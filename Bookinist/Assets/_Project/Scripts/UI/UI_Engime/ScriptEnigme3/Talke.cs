using System;
using UnityEngine;

public class Talke : MonoBehaviour
{
    [SerializeField] private UTalk Discution;
    private void Start()
    {
        switch (Discution)
        {
            case UTalk.BeginCharron:
                break;
            case UTalk.TalkCharronPiece:
                break;
            case UTalk.TalkCharronCafar:
                break;
            case UTalk.TalkCharronFinaly:
                break;
        }
    }
}
public enum UTalk
{
    BeginCharron,
    TalkCharronPiece,
    TalkCharronCafar,
    TalkCharronFinaly,

}

[Serializable]
public class ActionTalk
{
    public ActionType type;

    [Tooltip("Target fils de c'est grand morts XD ")]
    public GameObject target;
}