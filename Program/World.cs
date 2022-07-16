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
	public int EnemiesRemaining
	{
		get
		{
			return _enemiesNode.GetChildCount();
		}
	}
	private Node _enemiesNode;
	private bool _gameWon = false;
	private AudioStreamPlayer _defaultMusic;
	public override void _Ready()
	{
		Player = Game.GetPlayerInstance(this);
		CrewManager = GetNode<Control>("GUI/CrewMemberManager");
		//                                          Fuck Godot UI Trees
		CrewManagerBody = GetNode<CrewManagerBody>("GUI/CrewMemberManager/NinePatchRect/MarginContainer/CrewManagerBody");
		Player.Connect(nameof(Player.OnCrewUpdated), CrewManagerBody, nameof(CrewManagerBody._OnPlayerCrewUpdated));
		Player.Connect(nameof(Player.OnPlayerDeath), this, nameof(OnPlayerDeath));
		_enemiesNode = GetNode<Node>("Enemies");
		_defaultMusic = GetNode<AudioStreamPlayer>("Music");
		_defaultMusic.Play();
	}

	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("open_crew_member_manager"))
		{
			CrewManager.Visible = !CrewManager.Visible;
		}

		if (Input.IsActionJustPressed("reset"))
		{
			GetTree().ReloadCurrentScene();
		}

		if ((EnemiesRemaining <= 0 && !_gameWon))// || Input.IsActionJustPressed("autowin")
		{
			_gameWon = true;
			OnWin();
		}
	}

	public void OnPlayerDeath()
	{
		var d = GetNode<Control>("GUI/DeathNotifier");
		var dt = d.GetNode<Tween>("Tween");
		dt.InterpolateProperty(d, "modulate", new Color(1,1,1,0), new Color(1,1,1,1), 0.5f);
		dt.Start();
		_defaultMusic.Stop();
		GetNode<AudioStreamPlayer>("LoseMusic").Play();
	}
	
	private void OnWin()
	{
		var n = GetNode<Control>("GUI/WinNotifier");
		var nt = n.GetNode<Tween>("Tween");
		nt.InterpolateProperty(n, "modulate", new Color(1,1,1,0), new Color(1,1,1,1), 0.5f);
		nt.Start();
		_defaultMusic.Stop();
		GetNode<AudioStreamPlayer>("WinMusic").Play();
	}
}
