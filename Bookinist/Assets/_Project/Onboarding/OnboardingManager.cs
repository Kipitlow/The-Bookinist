using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class OnboardingManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _onboardingPanelList;

    private int _onboardingCurrentIndex = 0;

    [SerializeField] private CameraMovement _camMovement;
    [SerializeField] private TouchDetection _touchDetection;

    private int _previousDetectionId = -1;
    private bool _isStartingDetection = false;
    private bool _isStartingSwipe = false;

    private void Awake()
    {

        _camMovement.OnZoomOrDezoom += CamMovementOnZoom;
        _camMovement.OnSwipe += CamMovementOnSwipe;
        _touchDetection.OnClick += TouchDetectionOnClick;
    }

    private void CamMovementOnSwipe()
    {
        //Debug.Log("iufds");
        if (_isStartingSwipe == false)
        {
            _isStartingSwipe = true;
            CheckOnboarding(3);
        }
    }

    private void TouchDetectionOnClick(GameObject go)
    {
        //Debug.Log(go.name);
        if (go.name == "pomme 1 (1)")
        {
            CheckOnboarding(0);
        }
    }

    private void CamMovementOnZoom(int positivOrNegativ)
    {
        if (positivOrNegativ > 0)
            CheckOnboarding(4);
        else
            CheckOnboarding(5);
    }

    public void CheckOnboarding(int index)
    {
        if (index != _onboardingCurrentIndex)
            return;

        SetPanelActive(_onboardingCurrentIndex, false);

        _onboardingCurrentIndex++;

        SetPanelActive(_onboardingCurrentIndex, true);
    }

    private void SetPanelActive(int index, bool active)
    {
        if (index < 0 || index >= _onboardingPanelList.Count)
            return;

        _onboardingPanelList[index].SetActive(active);
    }
}
