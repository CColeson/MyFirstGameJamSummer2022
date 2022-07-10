using Godot;
using System;

public class PlayerMaxSpeedLabel : Label
{
    private Player Player;
    public override void _Ready()
    {
        Player = Game.GetPlayerInstance(this);
    }

    public override void _Process(float delta)
    {
        Text = $"max speed:  {Player.CurrentSailSpeed.Speed}";
    }
}
