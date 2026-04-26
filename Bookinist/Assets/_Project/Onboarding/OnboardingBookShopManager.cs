using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookShopOnboardingManager : MonoBehaviour
{
    [SerializeField] private List<Button> _onboardingButtonList;
    [SerializeField] private List<GameObject> _onboardingPanelList;
    [SerializeField] private List<GameObject> _secondOnboardingPanelList;
    [SerializeField] private GameObject _allMenus;
    [SerializeField] private BookshopUIManager _bookShopUIManager;
    [SerializeField] private BarFill _barFill;
    [SerializeField] private NPCTalker _talker;

    private bool _isAlreadyStartFirstDialog = false;

    [SerializeField] private List<bool> _isOnboardingPageActivated;

    private int _currentIndex = -1;

    private int _currentCustomPage = 0;

    private void Awake()
    {
        _talker.OnDialogEnd += TalkerOnDialogEnd;
    }

    private void Start()
    {
        // First-time onboarding flow
        if (GameManager.Instance.bookFinish == false)
        {
            if (_allMenus != null)
                _allMenus.SetActive(false);

            if (_onboardingPanelList.Count > 0)
                _onboardingPanelList[0].SetActive(true);

            return;
        }

        // Button listeners (FIXED CLOSURE BUG)
        for (int i = 0; i < _onboardingButtonList.Count; i++)
        {
            int index = i; // important fix
            _onboardingButtonList[i].onClick.AddListener(() => CheckOnboarding(index + 1));
            Debug.Log($"Button {i}: {_onboardingButtonList[i]}");
        }
    }

    private void OnDestroy()
    {
        if (_talker != null)
            _talker.OnDialogEnd -= TalkerOnDialogEnd;
    }

    private void TalkerOnDialogEnd(bool isAppearing)
    {
        if (GameManager.Instance.bookFinish)
        {
            StartCoroutine(WaitCloseBook(1f));
        }
        else
        {
            if (_onboardingPanelList.Count > 1)
                _onboardingPanelList[1].SetActive(true);
        }
    }

    private IEnumerator WaitCloseBook(float delay)
    {
        yield return new WaitForSeconds(delay);

        _bookShopUIManager.NavigateTo(1);
        CheckOnboarding(0);
        _barFill.ModifCur(20);
    }

    public void TalkToNPC()
    {
        if (_isAlreadyStartFirstDialog) return;

        if (_onboardingPanelList.Count > 0)
            _onboardingPanelList[0].SetActive(false);

        _isAlreadyStartFirstDialog = true;
    }

    public void OnboardingOpenBook()
    {
        if (_onboardingPanelList.Count > 1)
            _onboardingPanelList[1].SetActive(false);

        if (_onboardingPanelList.Count > 2)
            _onboardingPanelList[2].SetActive(true);
    }

    public void CheckOnboarding(int index)
    {
        if (_isOnboardingPageActivated == null || index >= _isOnboardingPageActivated.Count)
            return;

        if (_isOnboardingPageActivated[index])
            return;

        _isOnboardingPageActivated[index] = true;

        switch (index)
        {
            case 0:
                _secondOnboardingPanelList[index].SetActive(true);
                StartCoroutine(WaitBeforeAutoChangeOnboardingPanel(3.0f, _secondOnboardingPanelList[index], _secondOnboardingPanelList[index + 1]));
                break;
            case 1:
                _bookShopUIManager.NavigateTo(2);
                _secondOnboardingPanelList[_currentIndex + 1].SetActive(false);
                break;
            case 2:
                _secondOnboardingPanelList[_currentIndex + 1].SetActive(false);
                _secondOnboardingPanelList[index + 1].SetActive(true);
                break;
            case 3:
                _secondOnboardingPanelList[_currentIndex].SetActive(false);
                _secondOnboardingPanelList[index].SetActive(true);
                break;
            case 4:
                if (_currentCustomPage == 0)
                {
                    _isOnboardingPageActivated[index] = false;
                    _currentCustomPage++;                
                }
                else
                {
                    _secondOnboardingPanelList[_currentIndex].SetActive(false);
                    _secondOnboardingPanelList[index].SetActive(true);
                }
                break;
            case 5:
                _secondOnboardingPanelList[_currentIndex + 1].SetActive(false);
                _secondOnboardingPanelList[index + 1].SetActive(true);
                break;
            case 6:
                _secondOnboardingPanelList[_currentIndex + 1].SetActive(false);
                _secondOnboardingPanelList[index + 1].SetActive(true);
                break;
            case 7:
                _secondOnboardingPanelList[_currentIndex + 1].SetActive(false);
                _secondOnboardingPanelList[index + 1].SetActive(true);
                break;
            case 8:
                _secondOnboardingPanelList[_currentIndex + 1].SetActive(false);
                _secondOnboardingPanelList[index + 1].SetActive(true);
                break;
            default:
                if (_currentIndex + 1 != -1 && _currentIndex + 1 < _secondOnboardingPanelList.Count)
                    _secondOnboardingPanelList[_currentIndex + 1].SetActive(false);

                if (index < _secondOnboardingPanelList.Count)
                    _secondOnboardingPanelList[index + 1].SetActive(true);
                break;
        }

        _currentIndex = index;
    }

    IEnumerator WaitBeforeAutoChangeOnboardingPanel(float delay, GameObject panelDeactivate, GameObject panelActivate)
    {
        yield return new WaitForSeconds(delay);

        panelActivate?.SetActive(true);
        panelDeactivate?.SetActive(false);
    }
}