using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_BookSaga", menuName = "Scriptable Objects/SO_BookSaga")]
public class SO_BookSaga : ScriptableObject
{
    [SerializeField] private List<SO_Book> _booksInSagaList;

    private bool _isCompleted = false;
}
