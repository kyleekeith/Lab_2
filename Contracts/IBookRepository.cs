using System.Collections.Generic;
using Lab_2.Domain;

namespace Lab_2.Contracts
{
    /// <summary>
    /// Defines operations for storing and retrieving books.
    /// </summary>
    public interface IBookRepository
    {
        /// <summary>
        /// Adds a new book to the repository.
        /// </summary>
        /// <param name="book">The book to add.</param>
        void AddBook(Book book);

        /// <summary>
        /// Returns all books in the repository.
        /// </summary>
        /// <returns>A list of all books.</returns>
        List<Book> GetAll();

        /// <summary>
        /// Finds a book by its ISBN.
        /// </summary>
        /// <param name="isbn">The ISBN of the book.</param>
        /// <returns>The matching book if found; otherwise null.</returns>
        Book? GetByISBN(string isbn);

        /// <summary>
        /// Returns the number of books in the repository.
        /// </summary>
        /// <returns>Total number of books.</returns>
        int Count();
    }
}