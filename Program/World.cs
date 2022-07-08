using Godot;
using System;
using System.Collections.Generic;

public class World : Node2D
{   
    private Player Player;
    private CrewManagerBody CrewManagerBody;

    public override void _Ready()
    {
        Player = Game.GetPlayerInstance(this);
        //                                          Fuck Godot UI Trees
        CrewManagerBody = GetNode<CrewManagerBody>("GUI/CrewMemberManager/NinePatchRect/MarginContainer/CrewManagerBody");
        Player.Connect(nameof(Player.OnPlayerCrewMemberAdded), CrewManagerBody, nameof(CrewManagerBody._OnPlayerCrewAdded));
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("open_crew_member_manager"))
        {
            var m = GetNode<Control>("GUI/CrewMemberManager");
            m.Visible = !m.Visible;
        }
    }
}
