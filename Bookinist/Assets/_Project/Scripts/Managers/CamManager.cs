using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;

public class CamManager : MonoBehaviour
{
    [SerializeField] private List<CinemachineCamera> _allCams;

    [SerializeField] private CinemachineCamera _mainCam;

    [SerializeField] private CinemachineCamera _activeCam;

    public Action<int, int> OnViewChanged;

    private void Awake()
    {
        if (_allCams == null || _allCams.Count == 0)
        {
            var allCams = FindObjectsByType<CinemachineCamera>(FindObjectsSortMode.None);
            foreach (var cam in allCams) 
            {
                allCams.Append(cam);
            }
        }
        _activeCam = _allCams[0];
    }

    public void SwitchToCam(int cam, bool debug)
    {
        if (cam < 0 || cam >= _allCams.Count)
        {
            if (debug) Debug.LogWarning("Invalid camera index.");
            return;
        }

        if (_allCams[cam] == _activeCam)
        {
            if (debug) Debug.Log("New Cam is Active Cam!");
            return;
        }

        _activeCam.Priority = 0;
        _allCams[cam].Priority = 1;
        _activeCam = _allCams[cam];

        

        if (debug) Debug.Log("Swapped to " + _activeCam);
    }


    public void SwitchToCam(CinemachineCamera targetCam)
    {
        int index = _allCams.IndexOf(targetCam);
        if (index == -1)
        {
            Debug.LogWarning("Camera not found.");
            return;
        }

        SwitchToCam(index, false);
    }

    public void ReturnToCam(CinemachineCamera targetCam)
    {
        _activeCam.Priority = 0;
        targetCam.Priority = 1;
        _activeCam = targetCam;
    }

    /*
    public void SetFollowTarget(Transform newTarget, bool debug = false)
    {
        if (followCam == null)
        {
            if (debug) Debug.LogError("Follow Camera reference is missing!");
            return;
        }

        followCam.Follow = newTarget;
        followCam.LookAt = newTarget;

        if (debug)
            Debug.Log("Follow Camera now tracks: " + newTarget.name);
    }
    */

    public void NextCamera()
    {
        int next = (_allCams.IndexOf(_activeCam) + 1) % _allCams.Count;
        SwitchToCam(next, false);

        OnViewChanged?.Invoke(_allCams.IndexOf(_activeCam), 1);
    }

    public void PreviousCamera()
    {
        int prev = (_allCams.IndexOf(_activeCam) - 1 + _allCams.Count) % _allCams.Count;
        SwitchToCam(prev, false);

        OnViewChanged?.Invoke(_allCams.IndexOf(_activeCam), -1);
    }

    public int GetCurrentIndexView() => _allCams.IndexOf(_activeCam) + 1;

    private void OnDrawGizmos()
    {
        if (_activeCam != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_activeCam.transform.position, 0.5f);
        }
    }
}
