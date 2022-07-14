using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class CannonSet : Node2D
{
    [Signal]
    public delegate void NoCannonsAvailable();
    private List<Cannon> _Cannons = new List<Cannon>();
    private Node2D _Ship;
    public Node2D Ship
    {
        get 
        {
            return _Ship;
        }
        set
        {
            _Ship = value;
            foreach (var c in _Cannons)
            {
                c.Ship = value;
            }
        }
    }
    public List<Cannon> AvailableCannons
    {
        get
        {
            return _Cannons.Where(x => x.CrewMember != null).ToList();
        }
    }

    public override void _Ready()
    {
       _Cannons =  GetChildren().Cast<Cannon>().ToList();
    }

    public void InitializeSignals(Player p)
    {
        foreach (var c in _Cannons)
		{
			c.Connect(nameof(Cannon.OnFire), p, nameof(Player._OnCannonFire));
		}

		Connect(nameof(NoCannonsAvailable), p, nameof(Player._OnNoCannonsAvailable));
    }

    public void Fire()
    {
        if (AvailableCannons.Count == 0)
        {
            EmitSignal(nameof(NoCannonsAvailable));
            return;
        }
        foreach (var c in AvailableCannons)
        {
            c.Fire();
        }
    }

    public void AssignMemberToCannon(CrewMember m)
    {
        var c = _Cannons.Where(x => x.CrewMember == null).FirstOrDefault();
        if (c != null)
            c.CrewMember = m;
    }

    public void RemoveCrewMemberFromCannon(CrewMember m)
    {
        var c = AvailableCannons.Where(x => x.CrewMember == m).FirstOrDefault();
        if (c != null)
            c.CrewMember = null;
    }
}
