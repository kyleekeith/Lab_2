namespace Lab_2.Domain;
/// <summary>
/// represents a book in the system
/// </summary>
public class Book
{
    /// <summary>
    /// isbn for book
    /// </summary>
    public string ISBN { get; private set; }
    
    /// <summary>
    /// author of book
    /// </summary>
    public string Author { get; private set; }
    
    /// <summary>
    /// title of book
    /// </summary>
    public string Title { get; private set; }
    
    /// <summary>
    /// year the book was published
    /// </summary>
    public int Year {  get; private set; }

    /// <summary>
    /// book instance
    /// </summary>
    /// <param name="isbn"></param>
    /// <param name="author"></param>
    /// <param name="title"></param>
    /// <param name="year"></param>
    public Book(string isbn, string author, string title, int year)
    {
        ISBN = isbn;
        Author = author;
        Title = title;
        Year = year;

        Validate();
    }
    
    /// <summary>
    /// validates the data of the book
    /// </summary>
    /// <exception cref="Exception"></exception>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(ISBN))
            throw new Exception("ISBN is required");
        if (string.IsNullOrWhiteSpace(Author)) 
            throw new Exception("Author is required");
        if (string.IsNullOrWhiteSpace(Title))
            throw new Exception("Title is required");
        if (Year <= 0)
            throw new Exception("Year is required");
    }

    /// <summary>
    /// a readable version of book
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"{Title} by {Author} ({Year})";
    }
}