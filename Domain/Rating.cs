namespace Lab_2.Domain;
/// <summary>
/// rating given by member to a book
/// </summary>
public class Rating
{
    /// <summary>
    /// id of member who rated book
    /// </summary>
    public string MemberId { get; private set; }
    
    /// <summary>
    /// isbn of rated book
    /// </summary>
    public string ISBN { get; private set; }
    
    /// <summary>
    /// rating value that was given to book
    /// </summary>
    public RatingValue Value { get; private set; }
    
    /// <summary>
    /// new instance of rating
    /// </summary>
    /// <param name="memberId"></param>
    /// <param name="isbn"></param>
    /// <param name="value"></param>
    public Rating(string memberId, string isbn, RatingValue value)
    {
        MemberId = memberId;
        ISBN = isbn;
        Value = value; 
        
        Validate();
    }

    /// <summary>
    /// validates rating data
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(MemberId))
            throw new ArgumentException("Member id cannot be null or empty");
        if (string.IsNullOrWhiteSpace(ISBN))
            throw new ArgumentException("ISBN cannot be null or empty");
    }
}