using UnityEngine;

[CreateAssetMenu(fileName = "ScriptTaskPlayer", menuName = "Scriptable Objects/ScriptTaskPlayer")]
public class TaskPlayer : ScriptableObject
{
    [SerializeField] public string _NameTask;
    [SerializeField] public string _descriptionTask;
    [SerializeField] public int _point = 0;
    [SerializeField] public int _pointMax = 1;
}
