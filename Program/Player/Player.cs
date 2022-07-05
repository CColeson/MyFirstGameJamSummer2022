using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using GArray = Godot.Collections.Array;

public class Player : KinematicBody2D
{
	[Export]
	public float MoveSpeed = 30;
	[Export]
	public float MoveSpeedWhileRotating = 10;
	[Export]
	public float RotationSpeed = 0.5f;
	public Vector2? PositionToMoveTo = null;

	public Cannons LeftCannons;
	public Cannons RightCannons;
	public PlayerCamera Camera;
	public List<CrewMember> Crew;
	private RandomNumberGenerator Rng = new RandomNumberGenerator();
	
	public override void _Ready()
	{
		Rng.Randomize();
		LeftCannons = GetNode<Cannons>("Cannons/Left");
		RightCannons = GetNode<Cannons>("Cannons/Right");
		LeftCannons.Ship = this;
		RightCannons.Ship = this;
		InitializeCannonSignals();
		Camera = GetNode<PlayerCamera>("Camera2D");
		var g = Game.GetGlobalInstance(this);
		Crew = new List<CrewMember>()
		{
			new CrewMember() { MemberName = g.GetRandomName(), HP = Rng.RandiRange(2, 5) },
			new CrewMember() { MemberName = g.GetRandomName(), HP = Rng.RandiRange(2, 5) },
			new CrewMember() { MemberName = g.GetRandomName(), HP = Rng.RandiRange(2, 5) },
		};
	}
	
	public override void _Process(float delta)
	{
		if (Input.IsActionPressed("move")) 
		{
			PositionToMoveTo = GetGlobalMousePosition();
		}

		if (Input.IsActionJustPressed("attack")) 
		{
			var mp = GetGlobalMousePosition();
			var disToRight = RightCannons.GlobalPosition.DistanceTo(mp);
			var disToLeft = LeftCannons.GlobalPosition.DistanceTo(mp);
			(disToRight < disToLeft ? RightCannons : LeftCannons).Fire();
		}
	}
	
	public override void _PhysicsProcess(float delta)
	{
		if (PositionToMoveTo != null && Position.DistanceTo((Vector2) PositionToMoveTo) > 20)
		{
			var p = (Vector2) PositionToMoveTo;
			var moveDir = Position.DirectionTo(p).Normalized();
			var speed = MoveSpeed;

			GD.Print(Rotation + " - " + Transform.x.AngleTo(p - GlobalPosition) );
			if (Rotation != GlobalPosition.AngleTo(p - GlobalPosition))
				speed = MoveSpeedWhileRotating;
			
			MoveAndSlide(speed * moveDir);
			RotateToTarget(p, delta);
		}
	}

	private void RotateToTarget(Vector2 targetPos, float delta)
	{
		var direction = GlobalPosition.DirectionTo(targetPos);
		var angleTo = Transform.x.AngleTo(direction);
		Rotate(Mathf.Sign(angleTo) * Mathf.Min(delta * RotationSpeed, Mathf.Abs(angleTo)));
	}

	private void InitializeCannonSignals()
	{
		foreach (var c in LeftCannons.AvailableCannons)
		{
			c.Connect(nameof(Cannon.OnFire), this, nameof(_OnCannonFire));
		}
		foreach (var c in RightCannons.AvailableCannons)
		{
			c.Connect(nameof(Cannon.OnFire), this, nameof(_OnCannonFire));
		}
	}

	public void _OnCannonFire()
	{
		Camera.AddTrauma(0.2f);
	}
}
