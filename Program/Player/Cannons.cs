using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Cannons : Node2D
{
    [Export]
    public PackedScene Projectile;
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
            return _Cannons.Where(x => x.IsActive).ToList();
        }
    }
    public override void _Ready()
    {
       _Cannons =  GetChildren().Cast<Cannon>().ToList();
       foreach (var c in _Cannons)
       {
            c.Projectile = Projectile;
       }
    }

    public void Fire()
    {
        foreach (var c in AvailableCannons)
        {
            c.Fire();
        }
    }
}
