using Godot;
using System;
using System.Collections.Generic;
using GArray = Godot.Collections.Array;
public class CrewMemberLine : HBoxContainer
{
    [Signal]
    public delegate void MemberToCannon(CrewMember m);
    [Signal]
    public delegate void MemberToSails(CrewMember m);
    [Signal]
    public delegate void MemberToAnchor(CrewMember m);

    public CrewMember Member; // to be assigned by parent after instantation
    public Label NameLabel;
    public Label HPLabel;
    private List<PositionButton> PosButtons = new List<PositionButton>();
    public override void _Ready()
    {
        NameLabel = GetNode<Label>("Name");
        HPLabel = GetNode<Label>("HP");

        NameLabel.Text = $"{Member.FirstName} {Member.LastName}";
        HPLabel.Text = $"{Member.HP}/{Member.MaxHP}";

        foreach (CenterContainer c in GetNode<HBoxContainer>("PositionButtons").GetChildren())
        {
            var b = c.GetNode<PositionButton>("Button");
            b.Disabled = Member.CurrentPosition == b.Represents;
            b.Line = this;
            PosButtons.Add(b);
        }

        // I know this is shite
        var p = Game.GetPlayerInstance(this);
        Connect(
            nameof(CrewMemberLine.MemberToCannon), 
            p, 
            nameof(Player._OnCrewMemberPositionChanged)
        );
    }

    public void OnButtonPressed(PositionButton b)
    {
        Member.CurrentPosition = b.Represents;
        foreach (var bb in PosButtons)
        {
            bb.Disabled = Member.CurrentPosition == bb.Represents;
        }
        // i hate it too...
        // switch (Member.CurrentPosition)
        // {
        //     case CrewPosition.Cannon:
        //         break;
        // }
        EmitSignal(nameof(MemberToCannon), Member);
    }

}
