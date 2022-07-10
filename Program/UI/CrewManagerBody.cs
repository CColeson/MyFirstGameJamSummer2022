using Godot;
using System;

public class CrewManagerBody : VBoxContainer
{
    [Export]
    public PackedScene CrewMemberLine;
    public override void _Ready()
    {
        _OnPlayerCrewAdded(Game.GetPlayerInstance(this));
    }

    public void _OnPlayerCrewAdded(Player player)
    {
        foreach (Node c in GetChildren())
        {
            RemoveChild(c);
            c.QueueFree();
        }

        foreach (var m in player.Crew)
        {
            var l = CrewMemberLine.Instance<CrewMemberLine>();
            l.Member = m;
            this.AddChild(l);
        }
    }
}
