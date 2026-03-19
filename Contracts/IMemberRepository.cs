using System.Collections.Generic;
using Lab_2.Domain;

namespace Lab_2.Contracts
{
    /// <summary>
    /// Defines operations for storing and retrieving members.
    /// </summary>
    public interface IMemberRepository
    {
        /// <summary>
        /// Adds a new member to the repository.
        /// </summary>
        /// <param name="member">The member to add.</param>
        void Add(Member member);

        /// <summary>
        /// Returns all members in the repository.
        /// </summary>
        /// <returns>A list of all members.</returns>
        List<Member> GetAll();

        /// <summary>
        /// Finds a member by account id.
        /// </summary>
        /// <param name="accountId">The account id of the member.</param>
        /// <returns>The matching member if found; otherwise null.</returns>
        Member? GetByAccount(string accountId);

        /// <summary>
        /// Returns the number of members in the repository.
        /// </summary>
        /// <returns>Total number of members.</returns>
        int Count();
    }
}