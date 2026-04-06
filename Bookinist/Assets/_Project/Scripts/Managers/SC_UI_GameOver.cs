using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_UI_GameOver : MonoBehaviour
{
    [SerializeField]private GameObject Panel_Menue;
    void Start()
    {
        Panel_Menue = GameObject.Find("Panel_Menue_GameOver").gameObject;
        if(Panel_Menue != null) Panel_Menue.SetActive(false);
    }
    public void Event(string eventName)
    {
        switch (eventName)
        {
            case "Switch_Panel":
                Panel_Menue.SetActive(true);
                break;
            case "Restart_Level":
                //Permet de restart le level.
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case "QuitScene":
                SceneManager.LoadScene("BookShopUpdated");
                break;
        }
    }
    public void OpenScene(string NameLevel)
    {
        if (SceneManager.GetActiveScene().name == NameLevel)
        {
            SceneManager.LoadScene(NameLevel);
        }
        else { Debug.LogError("La scene on question n'existe pas ou tu écrit mal son nom"); }
    }
}
