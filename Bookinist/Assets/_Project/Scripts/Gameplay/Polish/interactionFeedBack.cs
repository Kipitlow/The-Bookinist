using UnityEngine;

public class InteractionFeedBack : MonoBehaviour
{
    [SerializeField] private bool _isInteractable;
    [SerializeField] private string _sfxTitle;

    private SoundManager _soundManager;
    private MoveObject _moveobject;
    private ObjectShake _objectShake;
    private ChangeColor _changeColor;



    private void Awake()
    {
        _moveobject = GetComponent<MoveObject>();
        _soundManager = SoundManager.Instance;
        _objectShake = GetComponent<ObjectShake>();
        _changeColor = GetComponent<ChangeColor>();
    }

    public void TryFeedback()
    {
        if (_isInteractable)
        {
            _soundManager.PlaySound(_sfxTitle);
        }
        else
        {
            NonInteractableObject();
        }
    }

    public void NonInteractableObject()
    {
        _objectShake.Shake();
        //_changeColor.PingPongColor(Color.gray);
    }


}
