using Godot;
using System;
using System.Linq;

public class Enemy : Ship
{
    private Timer AttackTimer;
    private Player _attacking;
    private Position2D _hullPos;

    [Export]
    public int MaxCrewCount = 10;
    public int MinCrewCount = 4;
    public override void _Ready()
    {
        base._Ready();

        var g = Game.GetGlobalInstance(this);
        for (int i=0; i < _rng.RandiRange(MinCrewCount, MaxCrewCount); i++)
        {
            var c = g.CrewMemberGenerator.Generate();
            Crew.Add(g.CrewMemberGenerator.Generate());
            Crew.AssignMemberToRandomPosition(c);
        }

        while (Crew.CannonsCount < 2)
            Crew.MoveMemberOffCannonToCannon();

        ShowRandomSprite();
        AttackTimer = GetNode<Timer>("AttackTimer");
        _hullPos = GetNode<Position2D>("HullPos");
        GetNode<Area2D>("VisionArea").Connect("body_entered", this, nameof(OnPlayerEnterVisionArea));
        AttackTimer.Connect("timeout", this, nameof(OnAttackTimerTimeout));
    }

    public override void _PhysicsProcess(float delta)
    {
        if (_attacking != null)
            RotateAndMove(delta);
    }

    public void _OnHitboxAreaEntered(Node cannonBall)
    {
        var c = cannonBall as CannonBall;
        if (c == null || c.Creator == this)
            return;

        Crew.DamageCrewMember();
    }

    public override void OnCrewMemberDeath(CrewMember m)
    {
        if (Crew.CrewCount <= 0)
        {
            QueueFree();
        }
    }
    
    public void OnPlayerEnterVisionArea(Node player)
    {
        var p = player as Player;
        if (p == null)
            return;

        _attacking = p;
        AttackTimer.Start(_rng.RandfRange(1f, 3f));
    }

    private void RotateAndMove(float delta)
    {
        var moveDir = GlobalPosition.DirectionTo(_hullPos.GlobalPosition);
        RotateToTarget(delta);
        MoveAndSlide(MoveSpeed * moveDir);
    }

    private void RotateToTarget(float delta)
    {
        var direction = GlobalPosition.DirectionTo(_attacking.GlobalPosition);
		var angleTo = Transform.x.AngleTo(direction);
		Rotate(Mathf.Sign(angleTo) * Mathf.Min(delta * RotationSpeed, Mathf.Abs(angleTo)));
    }

    public void OnAttackTimerTimeout()
    {
        _cannonController.Fire(_attacking.GlobalPosition);
    }

    private void ShowRandomSprite()
    {
        var sSprites = GetNode<Node2D>("ShipSprites").GetChildren().Cast<Sprite>().ToList();
        foreach (var s in sSprites)
            s.Visible = false;
        sSprites[_rng.RandiRange(0, sSprites.Count - 1)].Visible = true;
    }
}
