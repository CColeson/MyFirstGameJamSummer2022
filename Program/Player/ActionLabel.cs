using Godot;
using System;

public class ActionLabel : Node2D
{
    [Export]
    public Vector2 MarginFromPlayer = new Vector2(0, -60);
    private Player Player;
    public override void _Ready()
    {
        Player = Game.GetPlayerInstance(this);
    }

    public override void _Process(float delta)
    {
        GlobalRotation = 0;
        GlobalPosition = Player.GlobalPosition + MarginFromPlayer;
    }

    public void CrewMemberAdded(CrewMember member)
    {
        //TODO
    }
}
