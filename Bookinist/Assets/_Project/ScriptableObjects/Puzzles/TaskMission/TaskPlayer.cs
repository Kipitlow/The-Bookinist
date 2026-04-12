using UnityEngine;

[CreateAssetMenu(fileName = "ScriptTaskPlayer", menuName = "Scriptable Objects/ScriptTaskPlayer")]
public class TaskPlayer : ScriptableObject
{
    [SerializeField] private string _NameTask;
    [SerializeField] private string _descriptionTask;
    [SerializeField] private int _point = 0;
    [SerializeField] private int _pointMax = 1;
}
