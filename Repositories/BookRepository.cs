using System;
using System.Collections.Generic;
using Lab_2.Contracts;
using Lab_2.Domain;

namespace Lab_2.Repositories;

public class BookRepository : IBookRepository
{
    private readonly List<Book> books = new List<Book>();

    public void AddBook(Book book)
    {
        if (book == null)
            throw new ArgumentNullException(nameof(book));

        books.Add(book);
    }

    public List<Book> GetAll()
    {
        return books;
    }

    public Book? GetByISBN(string isbn)
    {
        foreach (var b in books)
        {
            if (b.ISBN == isbn)
            {
                return b;
            }
        }

        return null;
    }

    public int Count()
    {
        return books.Count;
    }
}