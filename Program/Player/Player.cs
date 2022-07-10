using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using GArray = Godot.Collections.Array;

public class Player : KinematicBody2D
{
	public float MoveSpeed = (float) SailSpeeds.Half.Speed;
	private SailSpeed _currentSailSpeed = SailSpeeds.Half;
	public SailSpeed CurrentSailSpeed
	{
		get
		{
			return _currentSailSpeed;
		}

		set
		{
			_currentSailSpeed = value;
		}
	} 
	[Export]
	public float MaxRotationSpeed = 1.3f;
	public float RotationSpeed = 1.3f;
	[Signal]
	public delegate void OnPlayerCrewMemberAdded();
	public CPUParticles2D SternParticles;
	public Vector2? PositionToMoveTo = null;
	public Vector2? DirectionToMoveTo = null;
	public Vector2 PositionBeforeMove = Vector2.Zero;
	public Cannons LeftCannons;
	public Cannons RightCannons;
	public PlayerCamera Camera;
	public ActionLabel ActionLabel;
	public List<CrewMember> Crew;
	public int MaxCrewCount = 10;
	private RandomNumberGenerator Rng = new RandomNumberGenerator();
	public delegate void State(float delta);
	public State Anchoring;
	public State RaisingAnchor;
	public State Anchored;
	public State Default;
	public State CurrentState;

	public override void _Ready()
	{
		Rng.Randomize();
		var g = Game.GetGlobalInstance(this);
		LeftCannons = GetNode<Cannons>("Cannons/Left");
		RightCannons = GetNode<Cannons>("Cannons/Right");
		Camera = GetNode<PlayerCamera>("Camera2D");
		SternParticles = GetNode<CPUParticles2D>("SternParticles");
		ActionLabel = GetNode<ActionLabel>("ActionLabel");


		LeftCannons.Ship = this;
		RightCannons.Ship = this;

		SternParticles.Emitting = false;
		PositionBeforeMove = GlobalPosition;
		
		Crew = new List<CrewMember>()
		{
			g.CrewMemberGenerator.Generate(),
			g.CrewMemberGenerator.Generate()
		};

		CurrentSailSpeed = SailSpeeds.Half;
		ActionLabel.Player = this;
		InitializeCannonSignals();
		InitializeStates();
		GetNode("PickupArea").Connect("area_entered", this, nameof(OnOverBoardPersonPickedUp));
	}
	public override void _Process(float delta)
	{
		if (Input.IsActionPressed("move")) 
		{
			PositionToMoveTo = GetGlobalMousePosition();
			DirectionToMoveTo = GlobalPosition.DirectionTo((Vector2) PositionToMoveTo);
			PositionBeforeMove = GlobalPosition;
		}
			

		if (Input.IsActionJustPressed("attack")) 
		{
			var mp = GetGlobalMousePosition();
			var disToRight = RightCannons.GlobalPosition.DistanceTo(mp);
			var disToLeft = LeftCannons.GlobalPosition.DistanceTo(mp);
			(disToRight < disToLeft ? RightCannons : LeftCannons).Fire();
		}

		if (Input.IsActionJustPressed("anchor"))
		{
			if (CurrentState == Anchoring || CurrentState == Anchored)
			{
				CurrentState = RaisingAnchor;
			}
			else {
				CurrentState = Anchoring;
			}
		}

		if (Input.IsActionJustPressed("full_sail"))
		{
			CurrentSailSpeed = SailSpeeds.Full;
		}
		if (Input.IsActionJustPressed("half_sail"))
		{
			CurrentSailSpeed = SailSpeeds.Half;
		}
		if (Input.IsActionJustPressed("low_sail"))
		{
			CurrentSailSpeed = SailSpeeds.Low;
		}
	}
	
	public override void _PhysicsProcess(float delta)
	{
		CurrentState(delta);
	}

	private void RotateToTarget(Vector2 orignalPos, Vector2 targetPos, float delta)
	{
		var direction = orignalPos.DirectionTo(targetPos);
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
		Camera.AddTrauma(0.15f);
	}

	public void OnOverBoardPersonPickedUp(Node person)
	{
		if (Crew.Count >= MaxCrewCount)
			return;

		var p = person as OverboardPerson;
		if (p != null)
		{
			Crew.Add(p.CrewMember);
			ActionLabel.CrewMemberAdded(p.CrewMember);
			EmitSignal(nameof(OnPlayerCrewMemberAdded), this);
			p.QueueFree();
		}
			
	}

	private void InitializeStates()
	{
		Default = (float delta) => 
		{
			MoveSpeed = CurrentSailSpeed.Speed;
			RotateAndMove(delta);
		};

		Anchoring = (float delta) => 
		{
			RotateAndMove(delta);
			MoveSpeed = Mathf.Clamp(MoveSpeed - 60 * delta, 0, CurrentSailSpeed.Speed);
			RotationSpeed = Mathf.Clamp(RotationSpeed - 0.05f * delta, 0, MaxRotationSpeed);
			if (MoveSpeed <= 0)
			{
				CurrentState = Anchored;
				MoveSpeed = 0;
			}
				
		};

		RaisingAnchor = (float delta) => {
			RotateAndMove(delta);
			MoveSpeed = Mathf.Clamp(MoveSpeed + 60 * delta, 0, CurrentSailSpeed.Speed);
			RotationSpeed = Mathf.Clamp(RotationSpeed + 0.05f * delta, 0, MaxRotationSpeed);
			if (MoveSpeed >= CurrentSailSpeed.Speed)
			{
				CurrentState = Default;
				MoveSpeed = CurrentSailSpeed.Speed;
				RotationSpeed = MaxRotationSpeed;
			}	
		};

		Anchored = (float delta) => {
			RotationSpeed = 0.4f;
			RotateToTarget(PositionBeforeMove, (Vector2) PositionToMoveTo, delta);
		};

		CurrentState = Default;
	}

	private void RotateAndMove(float delta)
	{
		if (PositionToMoveTo == null || DirectionToMoveTo == null) return;
		var p = (Vector2) PositionToMoveTo;
		var moveDir = Transform.x;
		SternParticles.Emitting = Math.Floor(MoveSpeed) >= 40;
		RotateToTarget(PositionBeforeMove, p, delta);
		MoveAndSlide(MoveSpeed * moveDir);
	}
}
