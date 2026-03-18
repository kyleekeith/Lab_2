using Lab_2.Contracts;
namespace Lab_2.Services;
/// <summary>
/// handles authentiication logic (login/logout)
/// </summary>
public class AuthService : IAuthService
{
    private readonly IMemberRepository _memberRepository;
    private string _currentMemberId;

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
        if (member == null) return false;
        
        _currentMemberId = accountId;
        return true;
    }

    /// <summary>
    /// logs out member
    /// </summary>
    /// <returns></returns>
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
    public string GetCurrentMemberId()
    {
        return _currentMemberId;
    }
}
