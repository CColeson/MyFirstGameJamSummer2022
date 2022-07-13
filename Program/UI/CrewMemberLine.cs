using Godot;
using System;
using System.Collections.Generic;
using GArray = Godot.Collections.Array;
public class CrewMemberLine : HBoxContainer
{

    public CrewMember Member; // to be assigned by parent after instantation
    public Label NameLabel;
    public Label HPLabel;
    private List<PositionButton> _posButts = new List<PositionButton>();
    public override void _Ready()
    {
        NameLabel = GetNode<Label>("Name");
        HPLabel = GetNode<Label>("HP");

        NameLabel.Text = $"{Member.FirstName} {Member.LastName}";
        HPLabel.Text = $"{Member.HP}/{Member.MaxHP}";

        foreach (CenterContainer c in GetNode<HBoxContainer>("PositionButtons").GetChildren())
        {
            var b = c.GetNode<PositionButton>("Button");
            b.Line = this;
            _posButts.Add(b);
        }
    }

    public void OnButtonPressed(PositionButton b)
    {
        var p = Game.GetPlayerInstance(this);
        foreach (var butt in _posButts)
        {
            butt.Disabled = butt.Position == b.Position;
        }
    }
}
