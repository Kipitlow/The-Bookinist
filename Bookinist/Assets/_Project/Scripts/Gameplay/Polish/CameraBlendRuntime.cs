using Unity.Cinemachine;
using UnityEngine;

public class CameraBlendRuntime : MonoBehaviour
{
    [SerializeField] private CinemachineBrain cinemachineBrain;

    public void SetEaseInBlend(float duration)
    {
        cinemachineBrain.DefaultBlend = new CinemachineBlendDefinition(
            CinemachineBlendDefinition.Styles.EaseIn,
            duration
        );
    }
}