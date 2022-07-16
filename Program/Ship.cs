using System;
using Godot;

public class Ship : KinematicBody2D
{
    protected RandomNumberGenerator _rng = new RandomNumberGenerator();
    protected CannonController _cannonController;
    public CrewManager Crew;
    public float MoveSpeed = (float) SailSpeeds.Half.Speed;
	public float MaxSpeed { get { return CurrentSailSpeed.Speed + (Crew.SailsCount * 12);} }
	public SailSpeed CurrentSailSpeed = SailSpeeds.Half;
	public float MaxRotationSpeed { get { return 1.0f + (Crew.SailsCount * 0.05f); } }
	public float RotationSpeed = 1.0f;
	public float StoppingSpeed { get { return 40 + (Crew.AnchorsCount * 10); }}

    public override void _Ready()
    {
        _rng.Randomize();
        _cannonController = GetNode<CannonController>("CannonController");
        Crew = GetNode<CrewManager>("CrewManager");
        Crew.InitilizeCannonController(_cannonController);
        
        GetNode<Area2D>("Hitbox").Connect("area_entered", this, nameof(OnDamageTaken));
        Crew.Connect(nameof(CrewManager.OnCrewMemberDeath), this, nameof(OnCrewMemberDeath));
    }

    public virtual void OnDamageTaken(Node cannonBall)
    {
        var c = cannonBall as CannonBall;
        if (c == null || c.Creator == this)
            return;
        
        Crew.DamageCrewMember();
        c.QueueFree();
    }

    public virtual void OnCrewMemberDeath(CrewMember m)
    {

    }
}