using Godot;
using System;

public class Explosion : AnimatedSprite
{
    private RandomNumberGenerator _rng = new RandomNumberGenerator();
    public override void _Ready()
    {
        _rng.Randomize();
        Playing = true;
        //SpeedScale = _rng.RandfRange(0.8f, 2);
        RotationDegrees  = _rng.RandfRange(0, 360);
        Connect("animation_finished", this, nameof(OnAnimationFinished));
    }

    public void OnAnimationFinished()
    {
        QueueFree();
    }

}
