using Godot;
using System;

public class ActionLabel : Node2D
{
    [Export]
    public Vector2 MarginFromParent = new Vector2(0, -60);
    [Export]
    public PackedScene LabelNode;
    [Export]
    public Color TextColor = new Color("#ffffff");
    public VBoxContainer Container;
    public Node2D Parent;

    public override void _Ready()
    {
        Container = GetNode<VBoxContainer>("Container");
    }

    public void OnParentReady(Node2D p)
    {
        Parent = p;
    }

    public override void _Process(float delta)
    {
        GlobalRotation = 0;
        GlobalPosition = Parent.GlobalPosition + MarginFromParent;
    }
    
    public void Flash(string message)
    {
        var n = LabelNode.Instance<ActionLabelNode>();
        n.AddColorOverride("font_color", TextColor);
        Container.AddChild(n);
        n.Text = message;
    }
}
