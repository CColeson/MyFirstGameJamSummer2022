using Godot;
using System;

public class PlayerCamera : Camera2D
{
	[Export]
	public float MinZoom = 0.2f;
	[Export]
	public float MaxZoom = 0.5f;
	[Export]
	public float ZoomFactor = 0.1f;
	[Export]
	public float ZoomDuration = 0.2f;
	[Export]
	public float Decay = 0.8f;
	[Export]
	public Vector2 MaxOffset = new Vector2(100, 50);
	[Export]
	public float MaxRoll = 0.1f;
	private float _ZoomLevel = 1f;
	public Tween Tween;
	private RandomNumberGenerator Rng = new RandomNumberGenerator();
	private OpenSimplexNoise Noise;
	private float Trauma = 0f;
	private float TraumaPower = 2;
	private float NoiseY = 0;
	
	public override void _Ready()
	{
		base._Ready();
		Tween = GetNode<Tween>("Tween");

		Noise = new OpenSimplexNoise();
		Rng.Randomize();
		Noise.Seed = (int) Rng.Randi();
		Noise.Period = 4;
		Noise.Octaves = 2;
	}
	
	public float ZoomLevel
	{
		set
		{
			_ZoomLevel = Godot.Mathf.Clamp(value, MinZoom, MaxZoom);
			Tween.InterpolateProperty
			(
				this,
				"zoom",
				Zoom,
				new Vector2(ZoomLevel, ZoomLevel),
				ZoomDuration,
				Tween.TransitionType.Sine,
				Tween.EaseType.Out
			);
			Tween.Start();
		}
		get
		{
			return _ZoomLevel;
		}
	}
	
	public override void _Process(float delta)
	{
		if (Input.IsActionJustReleased("zoom_in"))
		{
			ZoomLevel -= ZoomFactor;
		}
		
		if (Input.IsActionJustReleased("zoom_out"))
		{
			ZoomLevel += ZoomFactor;
		}

		if (Trauma > 0) 
		{
			Trauma = Mathf.Max(Trauma - Decay * delta, 0);
			Shake();
		}
	}

	public void Shake()
	{
		var amount = Mathf.Pow(Trauma, TraumaPower);
		NoiseY += 1;
		Rotation = MaxRoll * amount * Noise.GetNoise2d(Noise.Seed, NoiseY);
		Offset = new Vector2()
		{
			x = MaxOffset.x * amount * Noise.GetNoise2d(Noise.Seed * 2, NoiseY),
			y = MaxOffset.y * amount * Noise.GetNoise2d(Noise.Seed * 2, NoiseY)
		};
	}

	public void AddTrauma(float amount)
	{
		Trauma = Mathf.Min(Trauma + amount, 1f);
	}
}
