using Godot;
using System;

public class PositionButton : TextureButton
{
    [Export]
    public CrewPosition Position = CrewPosition.Idle;
    // yes its bad, yes I should use multiple classes or at least some kind of interface, yes I am too lazy for that
    [Signal]
    public delegate void MemberToCannon(CrewMember m);
    [Signal]
    public delegate void MemberToSails(CrewMember m);
    [Signal]
    public delegate void MemberToAnchor(CrewMember m);
    [Signal]
    public delegate void MemberToPlank(CrewMember m);
    public CrewMemberLine Line;
    private string SignalToCall;

    public override void _Ready()
    {
        SignalToCall = GetSignalName();
        var pCrewMan = Game.GetPlayerInstance(this).Crew;
        Connect(
            SignalToCall,
            pCrewMan,
            SignalToCall
        );
    }

    public void OnParentReady(CrewMemberLine p)
    {
        Line = p;
        var pCrewMan = Game.GetPlayerInstance(this).Crew;
        var pos = pCrewMan.GetCrewMemberPosition(p.Member);
        Disabled = pos == Position; 
    }

    private string GetSignalName()
    {
        switch (Position)
        {
            case CrewPosition.Cannon:
               return nameof(MemberToCannon);
            case CrewPosition.Anchor:
                return nameof(MemberToAnchor);
            case CrewPosition.Sail:
                return nameof(MemberToSails);
            case CrewPosition.Plank:
                return nameof(MemberToPlank);
        }
        return "Idle";
    }

    public override void _Pressed()
    {
        base._Pressed();
        Line.OnButtonPressed(this);
        EmitSignal(SignalToCall, Line.Member);
    }
}
