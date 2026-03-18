using Lab_2.Contracts;
using Lab_2.Domain;
namespace Lab_2.Services;
/// <summary>
/// book recommendations based on average ratings
/// </summary>
public class AverageRatingRecommender
{
    private readonly IRatingRepository _ratingRepository;
    private readonly IBookRepository _bookRepository;

    /// <summary>
    /// initializes the recommender
    /// </summary>
    /// <param name="ratingRepository"></param>
    /// <param name="bookRepository"></param>
    public AverageRatingRecommender(IRatingRepository ratingRepository, IBookRepository bookRepository)
    {
        _ratingRepository = ratingRepository;
        _bookRepository = bookRepository;
    }

    /// <summary>
    /// top rated books not rated by member
    /// </summary>
    /// <param name="memberId"></param>
    /// <param name="topN"></param>
    /// <returns></returns>
    public List<Book> Reccommend(string memberId, int topN)
    {
        var allBooks = _bookRepository.GetAll();
        var myRatings = _ratingRepository.GetByMember(memberId)
            .Select(r => r.ISBN)
            .ToHashSet();

        var scored = new List<Recommendation>();

        foreach (var book in allBooks)
        {
            if (myRatings.Contains(book.ISBN)) continue;

            double avg = AverageForBook(book.ISBN);
            scored.Add(new Recommendation(book, avg));
        }

        return scored
            .OrderByDescending(r => r.Score)
            .Take(topN)
            .Select(r => r.Book)
            .ToList();
    }

    /// <summary>
    /// calculates average rating for book
    /// </summary>
    /// <param name="isbn"></param>
    /// <returns></returns>
    private double AverageForBook(string isbn)
    {
        var ratings = _ratingRepository.GetAll()
            .Where(r =. r.ISBN == isbn)
            .ToList();

        if (ratings.Count == 0) return 0;

        return ratings.Average(r => (int)r.Value);
    }
}