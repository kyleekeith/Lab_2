namespace Lab_2.Domain;
/// <summary>
/// represents a member of the system
/// </summary>
public class Member
{
    /// <summary>
    /// account id
    /// </summary>
    public string AccountId { get; private set; }
    
    /// <summary>
    /// name of member
    /// </summary>
    public string Name { get; private set; }
    
    /// <summary>
    /// new instance of member
    /// </summary>
    /// <param name="acccountId"></param>
    /// <param name="name"></param>
    public Member(string acccountId, string name)
    {
        AccountId = acccountId;
        Name = name;

        Validate();
    }

    /// <summary>
    /// validates member
    /// </summary>
    /// <exception cref="Exception"></exception>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(AccountId))
            throw new Exception("Account ID is required");
        if (string.IsNullOrWhiteSpace(Name))
            throw new Exception("Name is required");
    }

    /// <summary>
    /// readable version of member
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"{Name} (ID: {AccountId})";
    }
}