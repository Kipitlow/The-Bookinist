using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private bool _paused = false;

    public void TogglePause()
    {
        if (_paused)
            Time.timeScale = 1.0f;
        else
            Time.timeScale = 0.0f;
        _paused = !_paused;
    }

    public void SetPauseState(bool PauseState)
    {
        if (!PauseState)
        {
            Time.timeScale = 1.0f;
            _paused = true;
        }
        else
        {
            Time.timeScale = 0.0f;
            _paused = true;
        }
    }
}
