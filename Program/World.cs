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
}
