using Godot;
using System;

public class EnemiesRemaining : Label
{
    private World _world;
    public override void _Ready()
    {
        _world = Game.GetWorldInstance(this);
    }

    public override void _Process(float delta)
    {
        Text = $"enemies remaining:  {_world.EnemiesRemaining}";
    }
}
