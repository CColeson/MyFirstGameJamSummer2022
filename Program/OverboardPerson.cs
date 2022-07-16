using Godot;
using System;
using System.Linq;
public class OverboardPerson : Area2D
{
    public CrewMember CrewMember;
    private RandomNumberGenerator _rng = new RandomNumberGenerator();
    private ActionLabel _actionLabel;
    private Timer _dialogTimer;
    public override void _Ready()
    {
        _rng.Randomize();
        RotationDegrees = _rng.RandfRange(0, 360);

        var ds = GetNode<Node2D>("DingySprites").GetChildren().Cast<Sprite>().ToList();
        foreach (var s in ds)
            s.Visible = false;
        ds[_rng.RandiRange(0, ds.Count - 1)].Visible = true;

        CrewMember = Game
            .GetGlobalInstance(this)
            .CrewMemberGenerator
            .Generate();
        
        var a = GetNode<AnimationPlayer>("AnimationPlayer");
        a.Play("default");
        a.Seek(_rng.RandfRange(0, 2), true);

        _actionLabel = GetNode<ActionLabel>("ActionLabel");
        _dialogTimer = GetNode<Timer>("DialogTimer");
        _dialogTimer.Connect("timeout", this, nameof(OnDialogTimerTimeout));
        _dialogTimer.OneShot = true;

        _actionLabel.OnParentReady(this);
        

        var it = GetNode<Timer>("InitialTimer");
        it.Connect("timeout", this, nameof(OnInitialTimerTimeout));
        _dialogTimer.Start(_rng.RandfRange(2, 4));
    }

    public void OnDialogTimerTimeout()
    {
        _actionLabel.Flash("save me!");
        _dialogTimer.Start(_rng.RandfRange(2, 4));
    }

    public void OnInitialTimerTimeout()
    {
        _dialogTimer.Start(0.1f);
    }
}
