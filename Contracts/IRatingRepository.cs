using System.Collections.Generic;
using Lab_2.Domain;

namespace Lab_2.Contracts
{
    /// <summary>
    /// Defines operations for storing and retrieving ratings.
    /// </summary>
    public interface IRatingRepository
    {
        /// <summary>
        /// Adds a rating or updates an existing one for the same member and book.
        /// </summary>
        /// <param name="rating">The rating to add.</param>
        void Add(Rating rating);

        /// <summary>
        /// Updates the rating value for a specific member and book.
        /// </summary>
        /// <param name="memberId">The member account id.</param>
        /// <param name="isbn">The ISBN of the book.</param>
        /// <param name="value">The new rating value.</param>
        void Update(string memberId, string isbn, RatingValue value);

        /// <summary>
        /// Returns all ratings in the repository.
        /// </summary>
        /// <returns>A list of all ratings.</returns>
        List<Rating> GetAll();

        /// <summary>
        /// Returns all ratings for a specific member.
        /// </summary>
        /// <param name="memberId">The member account id.</param>
        /// <returns>A list of ratings for the member.</returns>
        List<Rating> GetByMember(string memberId);

        /// <summary>
        /// Gets a specific rating for a member and a book.
        /// </summary>
        /// <param name="memberId">The member account id.</param>
        /// <param name="isbn">The ISBN of the book.</param>
        /// <returns>The matching rating if found; otherwise null.</returns>
        Rating? GetByMemberAndBook(string memberId, string isbn);

        /// <summary>
        /// Returns the rating value for a member and book.
        /// </summary>
        /// <param name="memberId">The member account id.</param>
        /// <param name="isbn">The ISBN of the book.</param>
        /// <returns>The stored rating value, or NotRead if not found.</returns>
        RatingValue GetValue(string memberId, string isbn);

        /// <summary>
        /// Checks whether a member has a rating entry for a book.
        /// </summary>
        /// <param name="memberId">The member account id.</param>
        /// <param name="isbn">The ISBN of the book.</param>
        /// <returns>True if the rating exists; otherwise false.</returns>
        bool HasRating(string memberId, string isbn);

        /// <summary>
        /// Returns the total number of ratings in the repository.
        /// </summary>
        /// <returns>Total number of ratings.</returns>
        int Count();
    }
}