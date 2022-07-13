using System;
using Godot;
using System.Collections.Generic;
using System.Linq;

public class Manager : Godot.Object
{
    protected List<CrewMember> _crew = new List<CrewMember>();
    public int CrewCount { get { return _crew.Count; } }

    public Manager()
    {

    }

    public virtual bool HasCrewMember(CrewMember m)
    {
        return _crew.Contains(m);
    }

    public virtual void RemoveCrewMember(CrewMember m)
    {
        if (HasCrewMember(m))
            _crew.Remove(m);
    }

    public virtual void AddCrewMember(CrewMember m)
    {
        if (!HasCrewMember(m))
            _crew.Add(m);
    }
}