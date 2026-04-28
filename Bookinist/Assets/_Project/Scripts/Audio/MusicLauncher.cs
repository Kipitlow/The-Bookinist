using UnityEngine;

public class MusicLauncher : MonoBehaviour
{
    private void Start()
    {
        SoundManager.Instance.PlaySound("MainMusic", 1, true);
    }
}
