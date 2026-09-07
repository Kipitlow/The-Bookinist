using System.Collections.Generic;
using UnityEngine;

public class BookManager : MonoBehaviour
{
    [SerializeField] private List<SO_Book> _allBooks;

    private List<SO_Book> _books;

    public void AddBook(SO_Book newBook)
    {
        foreach (SO_Book book in _allBooks)
        {
            if (newBook != book)
            {
                _books.Add(book);
                return;
            }
        }
    }
}
