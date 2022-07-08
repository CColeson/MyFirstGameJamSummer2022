using Godot;
using System;

public class CrewMemberCountLabel : Label
{

    private Player Player;

    public override void _Ready()
    {
        Player = Game.GetPlayerInstance(this);
    }

    public override void _Process(float delta)
    {
        this.Text = $"crew members:   {Player.Crew.Count} / 3";
    }
}
