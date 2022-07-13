using System;
using System.Collections.Generic;
using Godot;

// inheriting Node so it can use signals...
public class CrewManager : Godot.Node
{
    public int MaxCrewCount = 10;
    public int CrewCount { get { return CrewList.Count; } }
    public bool CanAddCrewMember { get { return CrewCount < MaxCrewCount; } }
    public List<CrewMember> CrewList = new List<CrewMember>();
    public CannonController _CannonController; // assigned by parent
    private CannonManager _cannonManager; // instantiated on ready provided player has a cannon controller
    private Manager _sailManager = new Manager();
    private Manager _anchorManager = new Manager();

    public void OnParentReady(Player parent)
    {
        _cannonManager = new CannonManager(parent.CannonController);
    }

    public void Add(CrewMember m)
    {
        if (!CanAddCrewMember)
            throw new Exception("Crew member capacity reached");

        CrewList.Add(m);
    }

    private void _removeMemberFromAllPositions(CrewMember m)
    {
        _cannonManager.RemoveCrewMember(m);
        _sailManager.RemoveCrewMember(m);
        _anchorManager.RemoveCrewMember(m);
    }

    public void MemberToCannon(CrewMember m)
    {
        _removeMemberFromAllPositions(m);
        _cannonManager.AddCrewMember(m);
    }

    public void MemberToSails(CrewMember m)
    {
        _removeMemberFromAllPositions(m);
        _sailManager.AddCrewMember(m);
    }

    public void MemberToAnchor(CrewMember m)
    {
        _removeMemberFromAllPositions(m);
        _anchorManager.AddCrewMember(m);
    }

    public void MemberToPlank(CrewMember m)
    {
        //todo
    }
}