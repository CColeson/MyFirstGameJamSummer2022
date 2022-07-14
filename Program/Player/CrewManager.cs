using System;
using System.Collections.Generic;
using Godot;

// inheriting Node so it can use signals...
public class CrewManager : Godot.Node
{
    public int MaxCrewCount = 10;
    public int CrewCount { get { return CrewList.Count; } }
    public int SailsCount { get { return _sailManager.CrewCount; }}
    public int CannonsCount { get { return _cannonManager.CrewCount; }}
    public int AnchorsCount { get { return _anchorManager.CrewCount; }}
    public bool CanAddCrewMember { get { return CrewCount < MaxCrewCount; } }
    public List<CrewMember> CrewList = new List<CrewMember>();
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
        GD.Print(m.LastName);
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

    public CrewPosition GetCrewMemberPosition(CrewMember m)
    {
        if (_cannonManager.HasCrewMember(m))
            return CrewPosition.Cannon;
        if (_sailManager.HasCrewMember(m))
            return CrewPosition.Sail;
        if (_anchorManager.HasCrewMember(m))
            return CrewPosition.Anchor;
        return CrewPosition.Idle;
    }
}