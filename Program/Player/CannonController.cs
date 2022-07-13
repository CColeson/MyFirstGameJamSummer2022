using Godot;
using System;
using System.Collections.Generic;

public class CannonController : Node2D
{
    private CannonSet RightCannons;
    private CannonSet LeftCannons;
    public override void _Ready()
    {
        RightCannons = GetNode<CannonSet>("Right");
        LeftCannons = GetNode<CannonSet>("Left");
    }

    public void Fire()
    {
        var mp = GetGlobalMousePosition();
        var disToRight = RightCannons.GlobalPosition.DistanceTo(mp);
        var disToLeft = LeftCannons.GlobalPosition.DistanceTo(mp);
        (disToRight < disToLeft ? RightCannons : LeftCannons).Fire();
    }

    public void InitializeSignals(Player p)
    {
        RightCannons.InitializeSignals(p);
        LeftCannons.InitializeSignals(p);
    }

    public void AssignMemberToCannon(CrewMember m)
    {
        (RightCannons.AvailableCannons.Count > LeftCannons.AvailableCannons.Count 
            ? LeftCannons : RightCannons).AssignMemberToCannon(m);
    }

    public void RemoveCrewMember(CrewMember m)
    {
        (RightCannons.AvailableCannons.Count > LeftCannons.AvailableCannons.Count 
            ? RightCannons : LeftCannons).RemoveCrewMemberFromCannon(m);
    }
}
