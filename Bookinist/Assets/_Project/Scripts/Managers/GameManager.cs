using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool _isFirstCustomerEncounter;

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

    public void FirstCustomerEncounter()
    {
        _isFirstCustomerEncounter = true;
    }
}