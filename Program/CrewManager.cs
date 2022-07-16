using System;
using System.Collections.Generic;
using Godot;
using System.Linq;

// inheriting Node so it can use signals...
public class CrewManager : Godot.Node
{
    [Signal]
    public delegate void OnCrewMemberDeath(CrewMember m);
    [Signal]
    public delegate void OnCrewMemberTossed(CrewMember m);
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
    private RandomNumberGenerator _rng = new RandomNumberGenerator();

    public void InitilizeCannonController(CannonController cannonController)
    {
        _cannonManager = new CannonManager(cannonController);
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

    public void HealAllCrewMembers()
    {
        foreach(var c in CrewList)
        {
            c.HP = c.MaxHP;
        }
    }

    // just to be sure that enemies have at least two cannons
    public void MoveMemberOffCannonToCannon()
    {
        var c = CrewList.Where(x => !_cannonManager.HasCrewMember(x)).FirstOrDefault();
        if (c != null)
            MemberToCannon(c);
    }

    public void DamageCrewMember()
    {
        
        for (int i = 0; i < _rng.RandiRange(1, 3); i++)
        {
            if (CrewCount <= 0)
                return;

            var c = CrewList[_rng.RandiRange(0, CrewCount - 1)];
            c.HP -= 1;
            if (c.HP <= 0)
            {
                _removeMemberFromAllPositions(c);
                CrewList.Remove(c);
                EmitSignal(nameof(OnCrewMemberDeath), c);
                continue;
            }
        }
    }

    public void AssignMemberToRandomPosition(CrewMember m)
    {
        // yes, its shit
        switch (_rng.RandiRange(0, 1))
        {
            case 1:
                MemberToSails(m);
                break;
            case 0:
            default:
                MemberToCannon(m);
                break;
        }
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