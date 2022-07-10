using Godot;
using System;

public class PositionButton : TextureButton
{
    [Export]
    public CrewPosition Represents;
    private CrewMember _Member;
    public CrewMember Member
    {
        get
        {
            return _Member;
        }
        set
        {
            _Member = value;
            this.Disabled = _Member.CurrentPosition == Represents;
        }
    }
    public override void _Ready()
    {
        
    }
}
