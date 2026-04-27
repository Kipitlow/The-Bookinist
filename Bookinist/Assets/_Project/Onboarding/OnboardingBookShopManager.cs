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
    [SerializeField] private List<GameObject> _specificLockedMenus;
    [SerializeField] private BookshopUIManager _bookShopUIManager;
    [SerializeField] private BarFill _barFill;
    [SerializeField] private NPCTalker _talker;

    [SerializeField] private List<Animator> _handAnimatorList;
    [SerializeField] private List<GameObject> _handList;

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

            return;
        }
        else
        {
            foreach (GameObject go in _specificLockedMenus)
            {
                go.SetActive(false);
            }
        }

        // Button listeners (FIXED CLOSURE BUG)
        for (int i = 0; i < _onboardingButtonList.Count; i++)
        {
            int index = i; // important fix

            if (_onboardingButtonList[index] == null) continue;

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
            {
                _onboardingPanelList[0].SetActive(false);
                _onboardingPanelList[1].SetActive(true);

                _handList[0].SetActive(false);
                _handList[1].SetActive(true);
                _handAnimatorList[1].SetBool("IsOnboardingTap", true);
            }
        }
    }

    private IEnumerator WaitCloseBook(float delay)
    {
        yield return new WaitForSeconds(delay);

        _bookShopUIManager.NavigateTo(1);
        CheckOnboarding(0);
        yield return new WaitForSeconds(0.5f);
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

    public void StartOnboarding()
    {
        if (_onboardingPanelList.Count > 0)
        {
            _onboardingPanelList[0].SetActive(true);

            _handList[0].SetActive(true);
            _handAnimatorList[0].SetBool("IsOnboardingTap", true);
        }

        _isAlreadyStartFirstDialog = true;
    }
    public void CheckOnboarding(int index)
    {
        if (_isOnboardingPageActivated == null || index >= _isOnboardingPageActivated.Count)
            return;

        if (_isOnboardingPageActivated[index])
            return;

        if (index == 9 && _currentCustomPage == 0) return;

        _isOnboardingPageActivated[index] = true;

        Debug.Log(index);

        switch (index)
        {
            case 0:
                _secondOnboardingPanelList[0].SetActive(true);
                StartCoroutine(WaitBeforeAutoChangeOnboardingPanel(3.0f, _secondOnboardingPanelList[0], _secondOnboardingPanelList[1]));
                break;
            case 2:
                _bookShopUIManager.NavigateTo(2);
                _secondOnboardingPanelList[1].SetActive(false);
                break;
            case 3:
                _secondOnboardingPanelList[2].SetActive(true);
                break;
            case 4:
                _secondOnboardingPanelList[2].SetActive(false);
                _secondOnboardingPanelList[3].SetActive(true);
                break;
            case 5:
                _secondOnboardingPanelList[3].SetActive(false);
                _secondOnboardingPanelList[4].SetActive(true);
                _specificLockedMenus[0].SetActive(true);
                break;
            case 6:
                _secondOnboardingPanelList[4].SetActive(false);
                StartCoroutine(WaitBeforeActivatingPanel(0.5f, 5));
                break;
            case 7:
                if (_currentCustomPage == 0)
                {
                    _isOnboardingPageActivated[index] = false;
                    _currentCustomPage++;
                }
                else
                {
                    _secondOnboardingPanelList[5].SetActive(false);
                    _secondOnboardingPanelList[6].SetActive(true);
                }
                break;
            case 8:
                _secondOnboardingPanelList[6].SetActive(false);
                _secondOnboardingPanelList[7].SetActive(true);
                break;
            case 9:
                _secondOnboardingPanelList[7].SetActive(false);
                _specificLockedMenus[1].SetActive(true);

                break;
            case 10:
                _secondOnboardingPanelList[8].SetActive(true);

                break;
            case 11:
                _secondOnboardingPanelList[8].SetActive(false);
                _specificLockedMenus[3].SetActive(true);

                break;
            case 12:
                _secondOnboardingPanelList[10].SetActive(true);
                break;
            case 13:
                _secondOnboardingPanelList[9].SetActive(true);
                break;
            case 14:
                _secondOnboardingPanelList[9].SetActive(false);
                _specificLockedMenus[2].SetActive(true);

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

    IEnumerator WaitBeforeActivatingPanel(float delay, int index)
    {
        yield return new WaitForSeconds(delay); 

        _secondOnboardingPanelList[index].SetActive(true);
    }
}