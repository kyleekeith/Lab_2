using System;
using System.Collections.Generic;
using Lab_2.Contracts;
using Lab_2.Domain;

namespace Lab_2.Repositories;

public class RatingRepository : IRatingRepository
{
    private readonly List<Rating> ratings = new List<Rating>();

    public void Add(Rating rating)
    {
        if (rating == null)
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

    public Rating? GetByMemberAndBook(string memberId, string isbn)
    {
        foreach (var rating in ratings)
        {
            if (rating.MemberId == memberId && rating.ISBN == isbn)
            {
                return rating;
            }
        }

        return null;
    }

    public RatingValue GetValue(string memberId, string isbn)
    {
        foreach (var rating in ratings)
        {
            if (rating.MemberId == memberId && rating.ISBN == isbn)
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
            if (rating.MemberId == memberId && rating.ISBN == isbn)
            {
                return true;
            }
        }

        return false;
    }

    public void Update(string memberId, string isbn, RatingValue value)
    {
        for (int i = 0; i < ratings.Count; i++)
        {
            if (ratings[i].MemberId == memberId && ratings[i].ISBN == isbn)
            {
                ratings[i] = new Rating(memberId, isbn, value);
                return;
            }
        }

        throw new KeyNotFoundException("Rating not found.");
    }

    public int Count()
    {
        return ratings.Count;
    }
}