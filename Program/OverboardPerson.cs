using Godot;
using System;

public class OverboardPerson : Area2D
{
    public CrewMember CrewMember;
    public override void _Ready()
    {
        CrewMember = Game
            .GetGlobalInstance(this)
            .CrewMemberGenerator
            .Generate();
        
        Game
            .GetWorldInstance(this)
            .ConnectOverboardPersonSignals(this);
    }

}
