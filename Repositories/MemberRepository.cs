using System;
using System.Collections.Generic;

namespace Lab_2.Repositories;

public class MemberRepository : IMemberRepository
{
    private List<Member> members = new List<Member>();
    
    public void add(Member member)
    {
        if (member == null)
            throw new ArgumentNullException(nameof(member));
        members.add(member);
    }

    public List<Member> GetAll()
    {
        return members;
    }

    public Member GetByAccount(string accountId)
    {
        foreach (var m in members)
        {
            if (m.Account == accountId)
            {
                return m;
            }
        }

        return null; // if not found
    }
}