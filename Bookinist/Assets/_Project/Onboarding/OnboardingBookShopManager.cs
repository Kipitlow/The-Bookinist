using System.Collections.Generic;
using UnityEngine;

public class BookShopOnboardingManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _onboardingButtonList;
    [SerializeField] private List<GameObject> _onboardingPanelList;
    [SerializeField] private GameObject _allMenus;
    [SerializeField] private NPCTalker _talker;

    private bool _isAlreadyStartFirstDialog = false;

    private void Awake()
    {
        _talker.OnDialogEnd += TalkerOnDialogEnd;

    }

    private void OnDestroy()
    {
        _talker.OnDialogEnd -= TalkerOnDialogEnd;
    }

    private void TalkerOnDialogEnd()
    {
        if (GameManager.Instance.bookFinish) return;

        _onboardingPanelList[1].SetActive(true);
    }

    public void TalkToNPC()
    {
        if (_isAlreadyStartFirstDialog) return;

        _onboardingPanelList[0].SetActive(false);
        _isAlreadyStartFirstDialog = true;
    }

    public void OnboardingOpenBook()
    {
        _onboardingPanelList[1].SetActive(false);
        _onboardingPanelList[2].SetActive(true);
    
    }

    private void Start()
    {
        if (GameManager.Instance.bookFinish == false)
        {
            _allMenus.SetActive(false);
            _onboardingPanelList[0].SetActive(true);

            return;
        }

        foreach (GameObject go in _onboardingButtonList)
        {
            go.SetActive(true);
        }
    }

    public void CheckOnboarding(int index)
    {
        
    }
}
