using Lab_2.Contracts;
using Lab_2.Domain;
using System.Linq;
namespace Lab_2.Services;
/// <summary>
/// personalized book recommendations using similar scoring
/// </summary>
public class RecommendationService
{
    private readonly IRatingRepository _ratingRepository;
    private readonly IBookRepository _bookRepository;

    /// <summary>
    /// initializes recommendation service
    /// </summary>
    /// <param name="ratingRepository"></param>
    /// <param name="bookRepository"></param>
    public RecommendationService(IRatingRepository ratingRepository, IBookRepository bookRepository)
    {
        _ratingRepository = ratingRepository;
        _bookRepository = bookRepository;
    }

    /// <summary>
    /// generates recommendation for member
    /// </summary>
    /// <param name="memberId"></param>
    /// <returns></returns>
    public List<Book> GetRecommendations(string memberId)
    {
        var allMembers = _ratingRepository.GetAll()
            .Select(r => r.MemberId)
            .Distinct()
            .Where(id => id != memberId);
        string bestMatch = null;
        int bestScore = int.MinValue;

        foreach (var other in allMembers)
        {
            int score = CalculateSimilarity(memberId, other);
            if (score > bestScore)
            {
                bestScore = score;
                bestMatch = other;
            }
        }

        if (bestMatch == null)
            return new List<Book>();
        var otherRatings = _ratingRepository.GetByMember(bestMatch);
        var myRatings = _ratingRepository.GetByMember(memberId);
        var myRatedBooks = myRatings.Select(r => r.ISBN).ToHashSet();

        return otherRatings
            .Where(r => !myRatedBooks.Contains(r.ISBN) && r.Value > 0)
            .OrderByDescending(r => r.Value)
            .Select(r => _bookRepository.GetByISBN(r.ISBN))
            .ToList();
    }

    /// <summary>
    /// calculates similarity between two members using dot product
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public int CalculateSimilarity(string a, string b)
    {
        var ratingsA = _ratingRepository.GetByMember(a);
        var ratingsB = _ratingRepository.GetByMember(b);

        int similarity = 0;

        foreach (var ratingA in ratingsA)
        {
            var ratingB = ratingsB.FirstOrDefault(r => r.ISBN == ratingA.ISBN);
            if (ratingB != null)
            {
                similarity += (int)ratingA.Value * (int)ratingB.Value;
            }
        }
        return similarity;
    }
}