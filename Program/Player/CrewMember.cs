using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public struct CrewMember
{
    public string FirstName;
    public string LastName;
    public int HP;

    public CrewMember(string firstName, string lastName, int hitPoints)
    {
        FirstName = firstName;
        LastName = lastName;
        HP = hitPoints;
    }
}