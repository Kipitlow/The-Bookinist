using System.IO;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class MainMenuLoader : MonoBehaviour
{
    [SerializeField] private GameObject _registerUI;

    [SerializeField] private GameObject _loadUI;

    [SerializeField] private UIAnimator _uiAnimator;

    [SerializeField] private PlayableDirector _director;

    [SerializeField] private TimelineAsset _firstTimeTimeline;

    [SerializeField] private TimelineAsset _repeatTimeline;


    public void ValidateSaveSlot()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "saveData.json");

        if (File.Exists(filePath))
        {
            _uiAnimator.Hide();
            SetFirstLoginTimeline(false);
            _loadUI.SetActive(true);
            return;
        }
        _uiAnimator.Show();
        SetFirstLoginTimeline(true);
        _registerUI.SetActive(true);
    }

    public void SetFirstLoginTimeline(bool firstLogin)
    {
        if (firstLogin)
            _director.playableAsset = _firstTimeTimeline;
        else
            _director.playableAsset = _repeatTimeline;
    }
}
