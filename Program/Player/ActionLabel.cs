using Godot;
using System;

public class ActionLabel : Node2D
{
    [Export]
    public Vector2 MarginFromPlayer = new Vector2(0, -60);
    [Export]
    public PackedScene LabelNode;
    public VBoxContainer Container;
    public Player Player;

    public override void _Ready()
    {
        Player = Game.GetPlayerInstance(this);
        Container = GetNode<VBoxContainer>("Container");
    }

    public override void _Process(float delta)
    {
        GlobalRotation = 0;
        GlobalPosition = Player.GlobalPosition + MarginFromPlayer;
    }
    
    public void Flash(string message)
    {
        var n = LabelNode.Instance<ActionLabelNode>();
        Container.AddChild(n);
        n.Text = message;
    }
}
