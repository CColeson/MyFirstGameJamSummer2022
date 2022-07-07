using Godot;
using System;
using System.Collections.Generic;

public class World : Node2D
{
    private Player Player;
    public override void _Ready()
    {
        Player = Game.GetPlayerInstance(this);
    }

    public void ConnectOverboardPersonSignals(OverboardPerson overboardMember)
    {
        var a = new Godot.Collections.Array();
        a.Add(overboardMember);
        GD.Print(a);
        overboardMember.Connect("body_entered", Player, nameof(Player.PickUpOverboardPerson), new Godot.Collections.Array {overboardMember});
    }
}
