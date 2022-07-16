using Godot;
using System;
using System.Collections.Generic;

public class CannonController : Node2D
{
    public Ship Creator;
    private CannonSet RightCannons;
    private CannonSet LeftCannons;
    public override void _Ready()
    {
        RightCannons = GetNode<CannonSet>("Right");
        LeftCannons = GetNode<CannonSet>("Left");

        RightCannons.Ship = GetParent<Ship>();
        LeftCannons.Ship = RightCannons.Ship;
    }

    public void Fire(Vector2 position)
    {
        var disToRight = RightCannons.GlobalPosition.DistanceTo(position);
        var disToLeft = LeftCannons.GlobalPosition.DistanceTo(position);
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
