using Godot;
using System;

public class PlayerSpeedLabel : Label
{
    private Player Player;
    public override void _Ready()
    {
        Player = Game.GetPlayerInstance(this);
    }

    public override void _Process(float delta)
    {
        Text = $"speed:  {Player.MoveSpeed}";
    }
}