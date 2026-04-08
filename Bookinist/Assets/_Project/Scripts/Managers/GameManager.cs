using UnityEngine;

/// <summary>
/// Singleton global du jeu.
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Variables

    public static GameManager Instance;
    public bool isFirstCustomerEncounter;

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

    #region Methods

    public void FirstCustomerEncounter()
    {
        isFirstCustomerEncounter = true;
    }

    #endregion
}