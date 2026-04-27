using System.Collections.Generic;
using UnityEditor.SettingsManagement;
using UnityEngine;

/// <summary>
/// Le manager de son : joue des effets, pool d'AudioSource, dictionnaire des clips.
/// Singleton DDOL.
/// </summary>
public class SoundManager : MonoBehaviour
{
    #region Types

    [System.Serializable]
    public class SoundEffect
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0.1f, 3f)] public float pitch = 1f;
        public bool loop = false;
    }

    #endregion

    #region Variables

    [SerializeField] private SoundEffect[] _soundEffects;
    [SerializeField][Range(1, 100)] private int _poolSize = 10;
    private List<AudioSource> _audioSources = new();
    private Dictionary<string, AudioClip> _soundDictionary = new();
    [SerializeField] private float _globalSfxVolume = 1f;

    public static SoundManager Instance;

    public float GlobalSfxVolume
    {
        get { return _globalSfxVolume; }
        set { _globalSfxVolume = Mathf.Clamp01(value); }
    }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        if (Instance == null)
        {
            Debug.Log("DDOL");
            Instance = this;
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

    private void Start()
    {
        SaveSystem.instance.OnDataUpdate += UpdateVolume;

        _globalSfxVolume = SaveSystem.instance.settings.playerSoundGeneral;
    }

    #endregion

    #region Methods

    private AudioSource GetAvailableAudioSource()
    {
        foreach (var source in _audioSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        return _audioSources.Count > 0 ? _audioSources[0] : null;
    }

    public void PlaySound(string soundName)
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
                    source.volume = sound.volume * _globalSfxVolume;
                    source.pitch = 1f;
                    source.loop = false;
                    source.Play();
                    return;
                }
            }
        }
    }

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
                    source.volume = sound.volume * _globalSfxVolume;
                    source.pitch = pitch * pitch; 
                    source.loop = loop;
                    source.Play();
                    return;
                }
            }
        }
    }

    public void StopAllSounds()
    {
        foreach (var source in _audioSources)
        {
            source.Stop();
        }
    }

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

    public void SetPoolSize(int size)
    {
        _poolSize = Mathf.Clamp(size, 1, 100);

        while (_audioSources.Count < _poolSize)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            _audioSources.Add(source);
        }
        while (_audioSources.Count > _poolSize)
        {
            AudioSource source = _audioSources[^1];
            _audioSources.RemoveAt(_audioSources.Count - 1);
            Destroy(source);
        }
    }


    public void UpdateVolume()
    {
        _globalSfxVolume = SaveSystem.instance.settings.playerSoundGeneral;
    }

    #endregion
}
