using Godot;

public class CannonManager : Manager
{
    private CannonController _controller;
    public CannonManager(CannonController cannonController) : base()
    {
        _controller = cannonController;
    }
    public override void AddCrewMember(CrewMember m)
    {
        base.AddCrewMember(m);
        _controller.AssignMemberToCannon(m);
    }

    public override void RemoveCrewMember(CrewMember m)
    {
        base.RemoveCrewMember(m);
        _controller.RemoveCrewMember(m);
    }
}