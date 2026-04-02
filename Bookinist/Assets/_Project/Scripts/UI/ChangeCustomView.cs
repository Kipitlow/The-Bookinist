using System;
using UnityEngine;

public class ChangeCustomView : MonoBehaviour
{
    [SerializeField] int _startViewIndex;
    [SerializeField] Camera[] _cameras;

    private int _currentIndexView;
    private Camera _originalCamActive;
    private Camera _switchedCamActive;

    private bool _isInitiated = false;

    public Action<int, int> OnViewChanged;


    private void Start()
    {
        _currentIndexView = _startViewIndex;

        foreach (Camera cam in _cameras)
        {
            if (cam.gameObject.activeSelf)
                _originalCamActive = cam;

            cam.gameObject.SetActive(false);
        }

        EnableCamera(_currentIndexView);
        _isInitiated = true;
    }

    private void OnDisable()
    {
        _switchedCamActive.gameObject.SetActive(false);
        _originalCamActive.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        if (_isInitiated)
            EnableCamera(_currentIndexView);
    }

    public void ChangeCurrentCameraIndex(bool isButtonRight)
    {
        DisableCamera(_currentIndexView);

        if (isButtonRight)
        {
            if (_currentIndexView >= _cameras.Length - 1)
                _currentIndexView = 0;
            else
                _currentIndexView++;
            
            OnViewChanged?.Invoke(_currentIndexView, 1);
        }
        else
        {
            if (_currentIndexView <= 0)
                _currentIndexView = _cameras.Length - 1;
            else
                _currentIndexView--;

            OnViewChanged?.Invoke(_currentIndexView, -1);
        }

        EnableCamera(_currentIndexView);

    }

    private void DisableCamera(int index)
    {
        _cameras[_currentIndexView].gameObject.SetActive(false);

    }

    private void EnableCamera(int index)
    {
        _cameras[_currentIndexView].gameObject.SetActive(true);
        _switchedCamActive = _cameras[_currentIndexView];
    }

    public int GetCurrentIndexView()
    {
        return _currentIndexView;
    }
}
