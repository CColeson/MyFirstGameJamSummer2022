using Godot;
using System;

public class PositionButton : TextureButton
{
    [Export]
    public CrewPosition Represents;
    public CrewMemberLine Line;

    public override void _Pressed()
    {
        base._Pressed();
        Line.OnButtonPressed(this);
    }
}
