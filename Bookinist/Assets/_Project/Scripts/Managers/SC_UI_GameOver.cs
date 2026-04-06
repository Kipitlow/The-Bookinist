using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// UI Game Over : gestion du panel et des événements de l'UI GameOver.
/// </summary>
public class SC_UI_GameOver : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject _panelMenue;

    #endregion

    #region Unity Methods

    private void Start()
    {
        _panelMenue = GameObject.Find("Panel_Menue_GameOver")?.gameObject;
        if (_panelMenue != null) _panelMenue.SetActive(false);
    }

    #endregion

    #region Methods

    public void Event(string eventName)
    {
        switch (eventName)
        {
            case "Switch_Panel":
                _panelMenue.SetActive(true);
                break;
            case "Restart_Level":
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case "QuitScene":
                SceneManager.LoadScene("BookShopUpdated");
                break;
        }
    }

    public void OpenScene(string nameLevel)
    {
        if (SceneManager.GetActiveScene().name == nameLevel)
        {
            SceneManager.LoadScene(nameLevel);
        }
        else
        {
            Debug.LogError("La scene en question n'existe pas ou son nom est mal écrit");
        }
    }

    #endregion
}
