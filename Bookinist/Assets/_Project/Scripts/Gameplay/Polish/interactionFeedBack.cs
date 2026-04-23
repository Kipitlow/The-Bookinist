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
            if (_particles != null) SpawnParticles();
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
        Debug.Log("Particles");
        GameObject ps = Instantiate(_particles, gameObject.transform.position, Quaternion.identity);
        if (gameObject.GetComponent<SpriteRenderer>() == null)
        {
            ps.GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerID = gameObject.GetComponentInChildren<SpriteRenderer>().sortingLayerID;
        }
        else
        {
            ps.GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerID = gameObject.GetComponent<SpriteRenderer>().sortingLayerID;
        }
        ps.GetComponent<ParticleSystem>().Play();
    }

}
