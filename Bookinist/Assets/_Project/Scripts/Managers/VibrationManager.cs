using UnityEngine;
using UnityEngine.InputSystem;

public class VibrationManager : MonoBehaviour
{
    public static VibrationManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void Vibrate(float intensity, float duration) //intensité entre 0 et 1
    {
        // Vérifie si l'appareil supporte les vibrations via l'Input System
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(intensity, intensity);

            Invoke(nameof(StopVibration), duration);
        }
        else
        {
            Handheld.Vibrate();
        }
    }

    private void StopVibration()
    {
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0f, 0f);
        }
    }

    private void OnDisable()
    {
        StopVibration();
    }
}