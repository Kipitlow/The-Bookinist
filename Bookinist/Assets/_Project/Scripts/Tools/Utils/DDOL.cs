using UnityEngine;

/// <summary>
/// Singleton "Don't Destroy On Load" minimal.
/// </summary>
public class DDOL : MonoBehaviour
{
    #region Variables

    public static DDOL Instance;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #endregion
}
