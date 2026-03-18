namespace Lab_2.Domain;
/// <summary>
/// recommended book with score
/// </summary>
public class Recommendation
{
    /// <summary>
    /// recommended book
    /// </summary>
    public Book Book { get; private set; }
    
    /// <summary>
    /// score indicating recommendation of book
    /// </summary>
    public double Score { get; private set; }

    /// <summary>
    /// new instance of recommendation
    /// </summary>
    /// <param name="book"></param>
    /// <param name="score"></param>
    public Recommendation(Book book, double score)
    {
        Book = book;
        Score = score;
    }

    /// <summary>
    /// readable version of recommendation
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"{Book.Title} (Score: {Score:F2})";
    }
}