using UnityEngine;

public class TransitionCaller : MonoBehaviour
{
    public void CallNuages()
    {
        CloudTransitionController.Instance.PlayTransition();
    }
}
