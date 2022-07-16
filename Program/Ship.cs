using System;
using Godot;

public class Ship : KinematicBody2D
{
    [Export]
    public PackedScene Explosion;
    protected RandomNumberGenerator _rng = new RandomNumberGenerator();
    protected CannonController _cannonController;
    public CrewManager Crew;
    public float MoveSpeed = (float) SailSpeeds.Half.Speed;
	public float MaxSpeed { get { return CurrentSailSpeed.Speed + (Crew.SailsCount * 12);} }
	public SailSpeed CurrentSailSpeed = SailSpeeds.Half;
	public float MaxRotationSpeed { get { return 1.0f + (Crew.SailsCount * 0.05f); } }
	public float RotationSpeed = 1.0f;
	public float StoppingSpeed { get { return 40 + (Crew.AnchorsCount * 10); }}
    private World _world;
    private ExplosionSounds _explosionSounds;

    public override void _Ready()
    {
        _rng.Randomize();
        _cannonController = GetNode<CannonController>("CannonController");
        _explosionSounds = GetNode<ExplosionSounds>("ExplosionSounds");
        Crew = GetNode<CrewManager>("CrewManager");
        Crew.InitilizeCannonController(_cannonController);
        
        GetNode<Area2D>("Hitbox").Connect("area_entered", this, nameof(OnDamageTaken));
        Crew.Connect(nameof(CrewManager.OnCrewMemberDeath), this, nameof(OnCrewMemberDeath));

        _world = Game.GetWorldInstance(this);
    }

    public virtual void OnDamageTaken(Node cannonBall)
    {
        var c = cannonBall as CannonBall;
        if (c == null || c.Creator == this)
            return;
        
        var e = Explosion.Instance<Explosion>();
        e.GlobalPosition = c.GlobalPosition;
        var s = _rng.RandfRange(1, 1.4f);
        e.Scale = new Vector2(s, s);
        _world.AddChild(e);
        _explosionSounds.PlayRandom();
        Crew.DamageCrewMember();
        c.QueueFree();
    }

    protected void CreateDeathExplosion()
    {
        var e = Explosion.Instance<Explosion>();
        e.GlobalPosition = GlobalPosition;
        e.Scale = new Vector2(2, 2);
        _world.AddChild(e);
    }

    public virtual void OnCrewMemberDeath(CrewMember m)
    {

    }
}