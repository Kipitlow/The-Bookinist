using UnityEngine;

[CreateAssetMenu(fileName = "SO_FinishBook", menuName = "Scriptable Objects/SO_FinishBook")]
public class SO_FinishBook : ScriptableObject
{
    public void FinishBook()
    {
        GameManager.Instance.bookFinish = true;
    }
}
