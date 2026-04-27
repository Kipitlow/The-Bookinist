using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool _isFirstCustomerEncounter;
    public bool _isFirstCustomerFinishDialog = false;
    public bool bookFinish;
    public bool bookStarted;

    [Range(0, 100)]
    [SerializeField] private int _hintsNumber = 30;

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

    public int GetHintNumber()
    { 
        return _hintsNumber;
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
        return bookStarted;
    }

    public void FinishBook()
    {
        bookStarted = false;
        bookFinish = true;
        SaveSystem.instance.progression.playerProgressionTutoComplete = bookFinish;
    }

    public bool UseHint()
    {
        if (_hintsNumber <= 0) return false;

        _hintsNumber--;
        return true;
    }

}