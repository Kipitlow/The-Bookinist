using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LibraryManager : MonoBehaviour
{
    [SerializeField] private List<Button> _bookSlotList;

    [SerializeField] private List<GameObject> _bookList;

    [SerializeField] private GameObject _bookSpawnPoint;

    [SerializeField] private List<bool> _isBookUnlockedList;

    [SerializeField] private Quaternion _rotation;
    private void Start()
    {
        CheckUnlockBook();
    }

    public void CheckUnlockBook()
    {
        for (int i = 0; i < _bookSlotList.Count; i++)
        {
            if (i >= _bookList.Count || i >= _isBookUnlockedList.Count) break;

            if (_isBookUnlockedList[i] == false)
            {
                _bookSlotList[i].interactable = false;
            }
            else
                _bookSlotList[i].interactable = true;

        }
    }
    public void PlaceBookOnLibrary(int index)
    {
        if (GameManager.Instance == null) return;

        if (GameManager.Instance.bookFinish)
        {
            _isBookUnlockedList[index] = true;
        }
    }

    public void SpawnBook(int index)
    {
        GameObject newBook = Instantiate(_bookList[index], _bookSpawnPoint.transform.position, _rotation);
    }
}
