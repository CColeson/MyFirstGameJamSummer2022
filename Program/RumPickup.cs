using Godot;
using System;

public class RumPickup : Area2D
{
    private RandomNumberGenerator _rng = new RandomNumberGenerator();
    public override void _Ready()
    {
        RotationDegrees = _rng.RandfRange(0, 360);
        var a = GetNode<AnimationPlayer>("AnimationPlayer");
        a.Play("default");
    }
}
