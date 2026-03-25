using Lab_2.Contracts;
using Lab_2.Domain;

namespace Lab_2.Services;

/// <summary>
/// handles authentication logic (login/logout)
/// </summary>
public class AuthService : IAuthServiceRepository
{
    private readonly IMemberRepository _memberRepository;
    private string? _currentMemberId;

    /// <summary>
    /// initializes auth service
    /// </summary>
    /// <param name="memberRepository"></param>
    public AuthService(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    /// <summary>
    /// logs in member (uses account id)
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns></returns>
    public bool Login(string accountId)
    {
        var member = _memberRepository.GetByAccount(accountId);
        if (member == null)
            return false;

        _currentMemberId = member.AccountId;
        return true;
    }

    /// <summary>
    /// logs out member
    /// </summary>
    public void Logout()
    {
        _currentMemberId = null;
    }

    /// <summary>
    /// checks if member is logged in
    /// </summary>
    /// <returns></returns>
    public bool IsLoggedIn()
    {
        return _currentMemberId != null;
    }

    /// <summary>
    /// gets id of member logged in
    /// </summary>
    /// <returns></returns>
    public string? CurrentMemberId()
    {
        return _currentMemberId;
    }
}