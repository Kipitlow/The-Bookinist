using UnityEngine;

public class interactionFeedBack : MonoBehaviour
{
    [SerializeField] private bool _isInteractable;
    SoundManager _soundManager;
    MoveObject _moveobject;

    private void Awake()
    {
        _moveobject = GetComponent<MoveObject>();
        _soundManager = SoundManager.Instance;
    }

    public void TryFeedback()
    {

    }

}
