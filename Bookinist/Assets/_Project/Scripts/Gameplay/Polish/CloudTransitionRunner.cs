using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CloudTransitionController : MonoBehaviour
{
    public static CloudTransitionController Instance { get; private set; }

    [SerializeField] private GameObject missionCanvasRoot;
    [SerializeField] private TMP_Text missionText;

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
    [SerializeField] private RectTransform[] leftClouds;
    [SerializeField] private RectTransform[] rightClouds;

    [Header("Events")]
    [SerializeField] private UnityEvent onScreenCovered;
    [SerializeField] private UnityEvent onTransitionFinished;

    [Header("Settings")]
    [SerializeField] private float duration = 0.6f;
    [SerializeField] private float coveredDelay = 0.3f;

    [SerializeField] private Vector2[] leftHiddenPosition;
    [SerializeField] private Vector2[] leftVisiblePosition;

    [SerializeField] private Vector2[] rightHiddenPosition;
    [SerializeField] private Vector2[] rightVisiblePosition;

    private bool isTransitionPlaying;

    public void PlayTransition()
    {
        if (isTransitionPlaying)
            return;

        StartCoroutine(TransitionRoutine());
    }

    private IEnumerator TransitionRoutine()
    {
        isTransitionPlaying = true;

        yield return MoveCloudsIn();

        onScreenCovered?.Invoke();

        yield return new WaitForSeconds(coveredDelay);

        yield return MoveCloudsOut();

        onTransitionFinished?.Invoke();

        isTransitionPlaying = false;
    }

    private IEnumerator MoveCloudsIn()
    {
        float timer = 0f;

        while (timer < duration)
        {
            float t = timer / duration;
            t = EaseOut(t);

            MoveCloudGroup(leftClouds, leftHiddenPosition, leftVisiblePosition, t);
            MoveCloudGroup(rightClouds, rightHiddenPosition, rightVisiblePosition, t);

            timer += Time.deltaTime;
            yield return null;
        }

        SetCloudGroupPosition(leftClouds, leftVisiblePosition);
        SetCloudGroupPosition(rightClouds, rightVisiblePosition);
    }

    private IEnumerator MoveCloudsOut()
    {
        float timer = 0f;

        while (timer < duration)
        {
            float t = timer / duration;
            t = EaseIn(t);

            MoveCloudGroup(leftClouds, leftVisiblePosition, leftHiddenPosition, t);
            MoveCloudGroup(rightClouds, rightVisiblePosition, rightHiddenPosition, t);

            timer += Time.deltaTime;
            yield return null;
        }

        SetCloudGroupPosition(leftClouds, leftHiddenPosition);
        SetCloudGroupPosition(rightClouds, rightHiddenPosition);
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