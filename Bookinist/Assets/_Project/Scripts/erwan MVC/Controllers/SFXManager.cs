using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }
    [SerializeField] private AudioSource _SFXObject;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    public void PlaySFXClip(AudioClip audioClip, Transform transform, float volume)
    {
        AudioSource audioSource = Instantiate(_SFXObject, transform.position, Quaternion.identity);

        audioSource.clip = audioClip;

        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);

    }
}
