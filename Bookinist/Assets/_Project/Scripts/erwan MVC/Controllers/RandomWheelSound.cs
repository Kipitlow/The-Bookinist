using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomWheelSound : MonoBehaviour
{
    [Header("Time")]
    [Tooltip("Use it for sound spawn custom randomization between min and max")]
    [SerializeField] private int minTime = 10;
    [SerializeField] private int maxTime = 30;

    [Header("Reference")]
    [SerializeField] AudioSource soundToPlay;
    void Start()
    {
        StartCoroutine(SpawnSound());
    }

    IEnumerator SpawnSound()
    {
        while (true)
        {
            int newDelay = UnityEngine.Random.Range(minTime, maxTime + 1);
            yield return new WaitForSeconds(maxTime);
            soundToPlay.Play();
        }
    }
}
