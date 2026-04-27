using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CloudTransitionController : MonoBehaviour
{
    public static CloudTransitionController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    [Header("Clouds")]
    [SerializeField] private RectTransform[] _leftClouds;
    [SerializeField] private RectTransform[] _rightClouds;

    [Header("Events")]
    [SerializeField] private SO_SceneManager _sceneManager;

    [Header("Settings")]
    [SerializeField] private float _duration = 0.6f;
    [SerializeField] private float _coveredDelay = 0.3f;

    [SerializeField] private Vector2[] _leftHiddenPosition;
    [SerializeField] private Vector2[] _leftVisiblePosition;

    [SerializeField] private Vector2[] _rightHiddenPosition;
    [SerializeField] private Vector2[] _rightVisiblePosition;

    private bool _isTransitionPlaying;

    public void PlayTransition(string sceneToLoad, string sceneToUnload)
    {
        if (_isTransitionPlaying)
            return;

        StartCoroutine(TransitionRoutine(sceneToLoad, sceneToUnload));
    }

    private IEnumerator TransitionRoutine(string sceneToLoad, string sceneToUnload)
    {
        _isTransitionPlaying = true;

        yield return MoveCloudsIn();

        _sceneManager.LoadScene(sceneToLoad);

        yield return new WaitForSeconds(_coveredDelay);

        yield return MoveCloudsOut();

        _sceneManager.UnloadScene(sceneToLoad);

        _isTransitionPlaying = false;
    }

    private IEnumerator MoveCloudsIn()
    {
        float timer = 0f;

        while (timer < _duration)
        {
            float t = timer / _duration;
            t = EaseOut(t);

            MoveCloudGroup(_leftClouds, _leftHiddenPosition, _leftVisiblePosition, t);
            MoveCloudGroup(_rightClouds, _rightHiddenPosition, _rightVisiblePosition, t);

            timer += Time.deltaTime;
            yield return null;
        }

        SetCloudGroupPosition(_leftClouds, _leftVisiblePosition);
        SetCloudGroupPosition(_rightClouds, _rightVisiblePosition);
    }

    private IEnumerator MoveCloudsOut()
    {
        float timer = 0f;

        while (timer < _duration)
        {
            float t = timer / _duration;
            t = EaseIn(t);

            MoveCloudGroup(_leftClouds, _leftHiddenPosition, _leftVisiblePosition, t);
            MoveCloudGroup(_rightClouds, _leftHiddenPosition, _leftVisiblePosition, t);

            timer += Time.deltaTime;
            yield return null;
        }

        SetCloudGroupPosition(_leftClouds, _leftHiddenPosition);
        SetCloudGroupPosition(_rightClouds, _rightHiddenPosition);
    }

    private void MoveCloudGroup(
        RectTransform[] clouds,
        Vector2[] startPositions,
        Vector2[] targetPositions,
        float t)
    {
        int count = Mathf.Min(clouds.Length, startPositions.Length, targetPositions.Length);

        for (int i = 0; i < count; i++)
        {
            if (clouds[i] == null)
                continue;

            clouds[i].anchoredPosition = Vector2.Lerp(startPositions[i], targetPositions[i], t);
        }
    }

    private void SetCloudGroupPosition(RectTransform[] clouds, Vector2[] positions)
    {
        int count = Mathf.Min(clouds.Length, positions.Length);

        for (int i = 0; i < count; i++)
        {
            if (clouds[i] == null)
                continue;

            clouds[i].anchoredPosition = positions[i];
        }
    }

    private float EaseOut(float t)
    {
        return 1f - Mathf.Pow(1f - t, 3f);
    }

    private float EaseIn(float t)
    {
        return t * t * t;
    }
}