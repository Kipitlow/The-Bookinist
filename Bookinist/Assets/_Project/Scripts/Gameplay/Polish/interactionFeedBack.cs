using UnityEngine;

public class InteractionFeedBack : MonoBehaviour
{
    [SerializeField] private bool _isInteractable;
    [SerializeField] private GameObject _particles;

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

    public void SpawnParticles()
    {
        GameObject ps = Instantiate(_particles, gameObject.transform.position, Quaternion.identity);
        ps.GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerID = gameObject.GetComponent<SpriteRenderer>().sortingLayerID;
        ps.GetComponent<ParticleSystem>().Play();
    }

}
