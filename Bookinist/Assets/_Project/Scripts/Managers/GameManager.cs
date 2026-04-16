using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool _isFirstCustomerEncounter;
    public bool bookFinish;
    public bool bookStarted;

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
        bookFinish = false;
        bookStarted = true;
    }

    public bool IsBookfinish()
    {
        return bookFinish;
    }

    public bool IsBookStarted()
    {
        return bookFinish;
    }

    public void FinishBook()
    {
        bookStarted = false;
        bookFinish = true;
    }

}