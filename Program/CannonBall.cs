using Godot;
using System;
using System.Collections.Generic;

public class CannonBall : Area2D
{
    [Export]
    public float Speed = 500;
    public Vector2 Direction;
    public Ship Creator;
    public override void _Process(float delta)
    {
        Position += Direction * Speed * delta;
    }
}
