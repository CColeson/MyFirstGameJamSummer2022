using Godot;
using System;

public class CannonManager : Node2D
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
}
