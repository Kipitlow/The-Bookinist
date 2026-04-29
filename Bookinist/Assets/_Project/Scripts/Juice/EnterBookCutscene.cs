using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class EnterBookCutscene : MonoBehaviour
{
    [SerializeField] private PlayableDirector _sceneStarter;

    [SerializeField] private TimelineAsset _loadTimeline;

    [SerializeField] private TimelineAsset _enterTimeline;
    [SerializeField] private GameObject _onBoardingPanelToDeactivate;
    [SerializeField] private GameObject _hand;

    public void EnterBook(bool enterBook)
    {
        if (enterBook)
        {
            _onBoardingPanelToDeactivate.SetActive(false);
            _hand.SetActive(false);
            _sceneStarter.playableAsset = _enterTimeline;
        }
        else
            _sceneStarter.playableAsset = _loadTimeline;
    }
}
