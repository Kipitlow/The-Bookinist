using System;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class OnboardingManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _onboardingPanelList;

    private int _onboardingCurrentIndex = 0;

    [SerializeField] private CameraMovement _camMovement;
    [SerializeField] private TouchDetection _touchDetection;

    [SerializeField] private List<Animator> _handAnimatorList;
    [SerializeField] private List<GameObject> _handList;

    private int _previousDetectionId = -1;
    private bool _isStartingDetection = false;
    private bool _isStartingSwipe = false;

    private WorldDropHandler _dropHandler;

    private void Awake()
    {

        _camMovement.OnZoomOrDezoom += CamMovementOnZoom;
        _camMovement.OnSwipe += CamMovementOnSwipe;
        _touchDetection.OnClick += TouchDetectionOnClick;
    }

    private void OnDestroy()
    {
        _camMovement.OnZoomOrDezoom -= CamMovementOnZoom;
        _camMovement.OnSwipe -= CamMovementOnSwipe;
        _touchDetection.OnClick -= TouchDetectionOnClick;

        if (_dropHandler != null)
            _dropHandler.OnDropItem -= DropHandlerOnDropItem;
    }

    private void Start()
    {
        _dropHandler = WorldDropHandler.Instance;

        _dropHandler.OnDropItem += DropHandlerOnDropItem;

        if (GameManager.Instance.bookFinish) return;

        _onboardingPanelList[0].SetActive(true);
        _handList[0].SetActive(true);
        _handAnimatorList[0].SetBool("IsOnboardingTap", true);
    }

    private void DropHandlerOnDropItem()
    {
        CheckOnboarding(2);
    }

    private void CamMovementOnSwipe()
    {
        Debug.Log(_onboardingCurrentIndex);

        if (_onboardingCurrentIndex != 3)
            return;

        if (_isStartingSwipe == false)
        {
            CheckOnboarding(3);
            _isStartingSwipe = true;
        }
    }

    private void TouchDetectionOnClick(GameObject go)
    {
        if (_onboardingCurrentIndex != 0)
            return;

        if (go.CompareTag("Pomme"))
        {
            CheckOnboarding(0);
        }
    }

    private void CamMovementOnZoom(int positivOrNegativ)
    {
        if (_onboardingCurrentIndex != 4 && _onboardingCurrentIndex != 5)
            return;

        if (positivOrNegativ > 0)
            CheckOnboarding(4);
        else
            CheckOnboarding(5);
    }

    public void CheckOnboarding(int index)
    {
        //Debug.Log(index);
        if (index != _onboardingCurrentIndex || GameManager.Instance.bookFinish)
            return;

        SetPanelActive(_onboardingCurrentIndex, false);

        _onboardingCurrentIndex++;

        SetPanelActive(_onboardingCurrentIndex, true);
    }

    private void SetHandActive(int index, bool isActive)
    {
        if (index < 0 || index >= _handList.Count)
            return;

        _handList[index].SetActive(isActive);

        switch (index)
        {
            case 2:
                _handAnimatorList[index].SetBool("IsOnboardingTap", false);
                _handAnimatorList[index].SetBool("IsOnboardingDragDrop", true);
                break;
            case 3:
                _handAnimatorList[index].SetBool("IsOnboardingDragDrop", false);
                _handAnimatorList[index].SetBool("IsOnboardingSwipe", true);
                break;
            case 4:
                _handAnimatorList[index].SetBool("IsOnboardingSwipe", false);
                _handAnimatorList[index].SetBool("IsOnboardingZoom", true);
                break;
            case 5:
                _handAnimatorList[index].SetBool("IsOnboardingZoom", false);
                _handAnimatorList[index].SetBool("IsOnboardingDezoom", true);
                break;
            default:
                _handAnimatorList[index].SetBool("IsOnboardingDezoom", false);
                _handAnimatorList[index].SetBool("IsOnboardingTap", true);
                break;
        }
    }

    private void SetPanelActive(int index, bool active)
    {
        if (index < 0 || index >= _onboardingPanelList.Count)
            return;

        SetHandActive(index, active);

        _onboardingPanelList[index].SetActive(active);
    }
}
