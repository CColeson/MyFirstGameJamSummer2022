using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using GArray = Godot.Collections.Array;

public class Player : Ship
{
	[Signal]
	public delegate void OnCrewUpdated();
	[Signal]
	public delegate void OnPlayerDeath();
	public delegate void State(float delta);
	public CPUParticles2D SternParticles;
	public Vector2? PositionToMoveTo = null;
	public Vector2? DirectionToMoveTo = null;
	public Vector2 PositionBeforeMove = Vector2.Zero;
	public PlayerCamera Camera;
	public ActionLabel ActionLabel;
	public State Anchoring;
	public State RaisingAnchor;
	public State Anchored;
	public State Default;
	public State Death;
	public State CurrentState;
	private World World;

	public override void _Ready()
	{
		base._Ready();

		World = Game.GetWorldInstance(this);
		Camera = GetNode<PlayerCamera>("Camera2D");
		SternParticles = GetNode<CPUParticles2D>("SternParticles");
		ActionLabel = GetNode<ActionLabel>("ActionLabel");

		Crew.Add(Game.GetGlobalInstance(this).CrewMemberGenerator.Generate());
		ActionLabel.OnParentReady(this);
		_cannonController.InitializeSignals(this);

		SternParticles.Emitting = false;
		PositionBeforeMove = GlobalPosition;
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
			

		if (Input.IsActionJustPressed("attack") && !World.CrewMangerIsOpen)
		{
			_cannonController.Fire(GetGlobalMousePosition());
		}

		if (Input.IsActionJustPressed("anchor"))
		{
			if (CurrentState == Anchoring || CurrentState == Anchored)
				CurrentState = RaisingAnchor;
			else 
				CurrentState = Anchoring;
		}

		if (Input.IsActionJustPressed("full_sail"))
			CurrentSailSpeed = SailSpeeds.Full;
		if (Input.IsActionJustPressed("half_sail"))
			CurrentSailSpeed = SailSpeeds.Half;
		if (Input.IsActionJustPressed("low_sail"))
			CurrentSailSpeed = SailSpeeds.Low;
	}

	public override void OnDamageTaken(Node cannonBall)
	{
		base.OnDamageTaken(cannonBall);
		var c = cannonBall as CannonBall;
		if (c == null || c.Creator == this)
			return;

		Camera.AddTrauma(0.6f);
		// TODO play sound, spawn explosion
		EmitSignal(nameof(OnCrewUpdated), this);
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

	public void _OnCannonFire()
	{
		Camera.AddTrauma(0.15f);
	}

	public void _OnNoCannonsAvailable()
	{
		ActionLabel.Flash("assign a crew member to cannons to shoot on this side");
	}

	public void OnOverBoardPersonPickedUp(Node person)
	{
		var p = person as OverboardPerson;
		if (p == null)
			return;
		
		if (!Crew.CanAddCrewMember)
		{
			ActionLabel.Flash("crew is at max capactiy");
			return;
		}

		var member = p.CrewMember;
		Crew.Add(member);
		ActionLabel.Flash($"picked up {member.FirstName} {member.LastName}");
		EmitSignal(nameof(OnCrewUpdated), this);
		p.QueueFree();
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

	public override void OnCrewMemberDeath(CrewMember m)
	{
		ActionLabel.Flash($"{m.FirstName} {m.LastName} has died");
		if (Crew.CrewCount <= 0)
		{
			CurrentSailSpeed = SailSpeeds.Zero;
			CurrentState = Death;
			Camera.PlayerIsDead = true;
			EmitSignal(nameof(OnPlayerDeath));
		}
		EmitSignal(nameof(OnCrewUpdated), this);
	}

	private void InitializeStates()
	{
		Default = (float delta) => 
		{
			MoveSpeed = MaxSpeed;
			if (RotationSpeed < MaxRotationSpeed)
			{
				RotationSpeed += delta;
			}
			else
			{
				RotationSpeed = MaxRotationSpeed;
			}
				
			RotateAndMove(delta);
		};

		Anchoring = (float delta) => 
		{
			RotateAndMove(delta);
			MoveSpeed = Mathf.Clamp(MoveSpeed - StoppingSpeed * delta, 0, MaxSpeed);
			RotationSpeed = Mathf.Clamp(RotationSpeed - 0.05f * delta, 0, MaxRotationSpeed);
			if (MoveSpeed <= 0)
			{
				CurrentState = Anchored;
				MoveSpeed = 0;
			}
		};

		RaisingAnchor = (float delta) => {
			RotateAndMove(delta);
			MoveSpeed = Mathf.Clamp(MoveSpeed + 60 * delta, 0, MaxSpeed);
			RotationSpeed = Mathf.Clamp(RotationSpeed + 0.05f * delta, 0, MaxRotationSpeed);
			if (MoveSpeed >= MaxSpeed)
			{
				CurrentState = Default;
				MoveSpeed = MaxSpeed;
				RotationSpeed = MaxRotationSpeed;
			}	
		};

		Anchored = (float delta) => {
			RotationSpeed = 0.4f;
			RotateToTarget(PositionBeforeMove, (Vector2) PositionToMoveTo, delta);
		};

		Death = (float delta) => {

		};



		CurrentState = Default;
	}
}
