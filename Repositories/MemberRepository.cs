using System;
using System.Collections.Generic;
using Lab_2.Contracts;
using Lab_2.Domain;

namespace Lab_2.Repositories;

public class MemberRepository : IMemberRepository
{
    private readonly List<Member> members = new List<Member>();

    public void Add(Member member)
    {
        if (member == null)
            throw new ArgumentNullException(nameof(member));

        members.Add(member);
    }

    public List<Member> GetAll()
    {
        return members;
    }

    public Member? GetByAccount(string accountId)
    {
        foreach (var m in members)
        {
            if (m.AccountId == accountId)
            {
                return m;
            }
        }

        return null;
    }

    public int Count()
    {
        return members.Count;
    }
}