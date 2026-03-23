using System;
using System.Collections.Generic;

namespace Lab_2.Repositories;

public class RatingRepository : IRatingRepository
{
    private List<Rating> ratings = new List<Rating>();

    public void Add(Rating rating)
    {
        if(rating == null)
            throw new ArgumentNullException(nameof(rating));
        ratings.Add(rating);
    }

    public List<Rating> GetAll()
    {
        return ratings;
    }

	public List<Rating> GetByMember(string memberId)
    {
        List<Rating> memberRatings = new List<Rating>();

        foreach (var rating in ratings)
        {
            if (rating.MemberId == memberId)
            {
                memberRatings.Add(rating);
            }
        }

        return memberRatings;
    }

    public RatingValue GetValue(string memberId, string isbn)
    {
        foreach (var rating in ratings)
        {
            if (rating.MemberId == memberId && rating.BookIsbn == isbn)
            {
                return rating.Value;
            }
        }

        throw new KeyNotFoundException("Rating not found.");
    }

    public bool HasRating(string memberId, string isbn)
    {
        foreach (var rating in ratings)
        {
            if (rating.MemberId == memberId && rating.BookIsbn == isbn)
            {
                return true;
            }
        }

        return false;
    }
}