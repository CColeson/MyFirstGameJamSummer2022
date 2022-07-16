using Godot;
using System;
using System.Collections.Generic;

public class World : Node2D
{   
	private Player Player;
	private CrewManagerBody CrewManagerBody;
	private Control CrewManager;
	public bool CrewMangerIsOpen
	{
		get
		{
			return CrewManager.Visible;
		}
	}

    public override void _Ready()
    {
        Player = Game.GetPlayerInstance(this);
        CrewManager = GetNode<Control>("GUI/CrewMemberManager");
        //                                          Fuck Godot UI Trees
        CrewManagerBody = GetNode<CrewManagerBody>("GUI/CrewMemberManager/NinePatchRect/MarginContainer/CrewManagerBody");
        Player.Connect(nameof(Player.OnCrewUpdated), CrewManagerBody, nameof(CrewManagerBody._OnPlayerCrewUpdated));
    }

	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("open_crew_member_manager"))
		{
			CrewManager.Visible = !CrewManager.Visible;
		}
	}
}
