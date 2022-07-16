using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class ExplosionSounds : Node2D
{
    private List<AudioStreamPlayer2D> _sounds = new List<AudioStreamPlayer2D>();
    private RandomNumberGenerator _rng = new RandomNumberGenerator();
    public override void _Ready()
    {
        _rng.Randomize();
        _sounds = GetChildren().Cast<AudioStreamPlayer2D>().ToList();
    }

    public void PlayRandom()
    {
        _sounds[_rng.RandiRange(0, _sounds.Count - 1)].Play();
    }
}
