using Godot;
using System;

public class ActionLabelNode : Label
{
    public Tween ColorTween;
    public Timer Timer;
    public override void _Ready()
    {
        ColorTween = GetNode<Tween>("ColorTween");
        Timer = GetNode<Timer>("Timer");
        Timer.Connect("timeout", this, nameof(_OnTimerTimeout));
        Modulate = new Godot.Color(Modulate.r, Modulate.g, Modulate.b, 0);

        ColorTween.InterpolateProperty(this, "modulate", new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 0.4f);
        ColorTween.Connect("tween_completed", this, nameof(_OnVisible));
        ColorTween.Start();
    }

    
    public void _OnVisible(Godot.Object obj, string key)
    {
        Timer.Start(1f);
    }

    public void _OnInvisible(Godot.Object obj, string key)
    {
        QueueFree();
    }

    public void _OnTimerTimeout()
    {
        ColorTween.InterpolateProperty(this, "modulate", new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 0.4f);
        ColorTween.Disconnect("tween_completed", this, nameof(_OnVisible));
        ColorTween.Connect("tween_completed", this, nameof(_OnInvisible));
        ColorTween.Start();
    }
}
