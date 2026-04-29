using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class EnterBookCutscene : MonoBehaviour
{
    [SerializeField] private PlayableDirector _sceneStarter;

    [SerializeField] private TimelineAsset _loadTimeline;

    [SerializeField] private TimelineAsset _enterTimeline;

    public void EnterBook(bool enterBook)
    {
        if (enterBook)
            _sceneStarter.playableAsset = _enterTimeline;
        else
            _sceneStarter.playableAsset = _loadTimeline;
    }
}
