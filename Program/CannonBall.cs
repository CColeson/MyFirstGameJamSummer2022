using Godot;
using System;

public class CannonBall : Area2D
{
    [Export]
    public float Speed = 500;
    public Vector2 Direction;

    public override void _Process(float delta)
    {
        Position += Direction * Speed * delta;
    }
}
