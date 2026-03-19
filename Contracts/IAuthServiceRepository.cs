namespace Lab_2.Contracts
{
    /// <summary>
    /// Defines login and logout behavior for members.
    /// </summary>
    public interface IAuthServiceRepository
    {
        /// <summary>
        /// Attempts to log in a member by account id.
        /// </summary>
        /// <param name="accountId">The member account id.</param>
        /// <returns>True if login succeeds; otherwise false.</returns>
        bool Login(string accountId);

        /// <summary>
        /// Logs out the current member.
        /// </summary>
        void Logout();

        /// <summary>
        /// Checks whether a member is currently logged in.
        /// </summary>
        /// <returns>True if someone is logged in; otherwise false.</returns>
        bool IsLoggedIn();

        /// <summary>
        /// Returns the account id of the currently logged-in member.
        /// </summary>
        /// <returns>The current member id, or null if nobody is logged in.</returns>
        string? CurrentMemberId();
    }
}