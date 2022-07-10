using System;
using System.Collections.Generic;

public class CrewManager
{
    public int MaxCrewCount = 10;
    public int CrewCount
    {
        get 
        {
            return CrewList.Count;
        }
    }
    public bool CanAddCrewMember
    {
        get
        {
            return CrewCount < MaxCrewCount;
        }
    }
    public List<CrewMember> CrewList = new List<CrewMember>();

    public void Add(CrewMember m)
    {
        if (!CanAddCrewMember)
            throw new Exception("Crew member capacity reached");
        CrewList.Add(m);
    }
}