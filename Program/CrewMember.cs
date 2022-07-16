using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class CrewMember: Godot.Object
{
    public string FirstName;
    public string LastName;
    public int HP;
    public int MaxHP;
    public CrewMember(string firstName, string lastName, int maxHP, int HP)
    {
        FirstName = firstName;
        LastName = lastName;
        MaxHP = maxHP;
        this.HP = HP;
    }
}