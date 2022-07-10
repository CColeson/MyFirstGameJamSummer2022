using Godot;
using System;
using System.Collections.Generic;
public class CrewMemberLine : HBoxContainer
{
    public CrewMember Member; // to be assigned by parent after instantation
    public Label NameLabel;
    public Label HPLabel;
    public override void _Ready()
    {
        NameLabel = GetNode<Label>("Name");
        HPLabel = GetNode<Label>("HP");

        NameLabel.Text = $"{Member.FirstName} {Member.LastName}";
        HPLabel.Text = $"{Member.HP}/{Member.MaxHP}";

        foreach (CenterContainer c in GetNode<HBoxContainer>("PositionButtons").GetChildren())
        {
            c.GetNode<PositionButton>("Button").Member = Member;
        }
    }
    

}
