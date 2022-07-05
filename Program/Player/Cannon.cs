using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Cannon : Position2D
{
	[Export]
	public bool IsActive = true;
	[Export]
	public float FireTimeMin = 0.1f;
	[Export]
	public float FireTimeMax = 0.4f;
	[Export]
	public float PitchMin = 0.7f;
	[Export]
	public float PitchMax = 1.52f;
	[Signal]
	public delegate void OnFire();
	private Node2D World;
	public Node2D Ship;
	public Position2D DirectionPointer; // I am not strong at math, this is a shit workaround...
	public List<CPUParticles2D> ParticleEmitters;
	public AudioStreamPlayer FireSound;
	public PackedScene Projectile;
	public Timer Timer;
	private RandomNumberGenerator Rng = new RandomNumberGenerator();
	public override void _Ready()
	{
		World = GetNode<Node2D>("/root/World");
		DirectionPointer = GetNode<Position2D>("Position2D");
		Timer = GetNode<Timer>("Timer");
		FireSound = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
		
		ParticleEmitters = GetNode("Particles").GetChildren().Cast<CPUParticles2D>().ToList();
		InitializeParticles();

		Timer.Connect("timeout", this, nameof(_OnTimerTimeout));
		Rng.Randomize();
	}

	public void Fire()
	{
		Timer.Start(Rng.RandfRange(FireTimeMin, FireTimeMax));
	}

	public void _OnTimerTimeout()
	{
		var p = Projectile.Instance<CannonBall>();
		World.AddChild(p);
		p.GlobalPosition = GlobalPosition;
		p.Direction = p.GlobalPosition.DirectionTo(DirectionPointer.GlobalPosition);
		EmitSignal(nameof(OnFire));
		FireParticles();
		FireSound.PitchScale = Rng.RandfRange(PitchMin, PitchMax);
		FireSound.Play();
	}

	private void FireParticles()
	{
		foreach (var p in ParticleEmitters)
		{
			p.Emitting = true;
		}
	}

	private void InitializeParticles()
	{
		foreach (var p in ParticleEmitters)
		{
			p.Direction = p.Position.DirectionTo(DirectionPointer.Position);
			GD.Print(p.Direction);
		}
	}
}