using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Le manager de son, gere le fait de jouer un son, et le stockage des sound effect
/// Singleton DDOL
/// </summary>
public class SoundManager : MonoBehaviour
{
    #region Variables

    [System.Serializable]
    public class SoundEffect
    {
        public string name;

        public AudioClip clip;

        [Range(0f, 1f)] public float volume = 1f;

        [Range(0.1f, 3f)] public float pitch = 1f;

        public bool loop = false;

    }

    [SerializeField] private SoundEffect[] _soundEffects;

    [SerializeField][Range(1, 100)] private int _poolSize = 10;

    private List<AudioSource> _audioSources = new List<AudioSource>();

    private Dictionary<string, AudioClip> _soundDictionary = new Dictionary<string, AudioClip>();

    private float _gloablSFXVolume = 1f;

    public static SoundManager instance { get; private set; }

    public float GloablSFXVolume
    {
        get { return _gloablSFXVolume; }
        set { _gloablSFXVolume = Mathf.Clamp01(value); }
    }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        for (int i = 0; i < _poolSize; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            _audioSources.Add(source);
        }

        foreach (SoundEffect sound in _soundEffects)
        {
            if (sound.clip != null)
            {
                _soundDictionary[sound.name] = sound.clip;
            }
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Retourne un AudioSource disponible dans la pool
    /// SI aucun n'est disponible, recycle le premier
    /// </summary>
    private AudioSource GetAvailableAudioSource()
    {

        // On prend le premier AudioSource libre
        foreach (var source in _audioSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        // Forcer le recyclage du premier AudioSource si tous sont occupés
        return _audioSources.Count > 0 ? _audioSources[0] : null;
    }

    /// <summary>
    /// Joue un son par son nom
    /// </summary>
    public void PlaySound(string soundName, float pitch = 1f, bool loop = false)
    {
        if (!_soundDictionary.TryGetValue(soundName, out AudioClip clip))
        {
            Debug.LogWarning($"Sound '{soundName}' not found!");
            return;
        }

        AudioSource source = GetAvailableAudioSource();
        if (source != null)
        {

            foreach (SoundEffect sound in _soundEffects)
            {
                if (sound.name == soundName)
                {
                    source.clip = clip;
                    source.volume = sound.volume * _gloablSFXVolume;
                    source.pitch = pitch;
                    source.loop = loop;
                    source.Play();
                    return;
                }
            }
        }
    }

    /// <summary>
    /// Arrête tous les sons en cours de lecture
    /// </summary>
    public void StopAllSounds()
    {
        foreach (var source in _audioSources)
        {
            source.Stop();
        }
    }

    /// <summary>
    /// Arrête un son par son nom
    /// </summary>
    public void StopSound(string soundName)
    {
        foreach (var source in _audioSources)
        {
            if (source.isPlaying && source.clip != null && source.clip.name == soundName)
            {
                source.Stop();
            }
        }
    }


    /// <summary>
    /// Vérifie si un son est en cours de lecture
    /// </summary>
    public bool IsSoundPlaying(string soundName)
    {
        foreach (var source in _audioSources)
        {
            if (source.isPlaying && source.clip != null && source.clip.name == soundName)
            {
                return true;
            }
        }
        return false;
    }


    /// <summary>
    /// Définit la taille [1-100] de la pool d'AudioSource 
    /// </summary>
    public void SetpoolSize(int size)
    {
        _poolSize = Mathf.Clamp(size, 1, 100);

        // Ajuster la taille de la pool
        while (_audioSources.Count < _poolSize)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            _audioSources.Add(source);
        }
        while (_audioSources.Count > _poolSize)
        {
            AudioSource source = _audioSources[_audioSources.Count - 1];
            _audioSources.RemoveAt(_audioSources.Count - 1);
            Destroy(source);
        }
    }

    #endregion
}
