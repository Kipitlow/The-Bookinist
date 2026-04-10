using UnityEngine;
using UnityEngine.Rendering;

public class ExitButton : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
