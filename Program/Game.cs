using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Game : Godot.Node
{
    public List<string> FirstNames = new List<string>();
    public List<string> LastNames = new List<string>();
    private RandomNumberGenerator Rng = new RandomNumberGenerator();
    public override void _Ready()
    {
        base._Ready();
        Rng.Randomize();
        var file = new Godot.File();
        file.Open("res://Assets/Etc/Names/First.pirate", File.ModeFlags.Read);
        string content;
        while(!string.IsNullOrEmpty(content = file.GetLine()))
        {
            FirstNames.Add(content.StripEdges());
        }
        file.Close();

        file.Open("res://Assets/Etc/Names/Last.pirate", File.ModeFlags.Read);
        while(!string.IsNullOrEmpty(content = file.GetLine()))
        {
            LastNames.Add(content.StripEdges());
        }
        file.Close();
        GD.Print(GetRandomName().Item1, GetRandomName().Item2);
    }

    public (string, string) GetRandomName()
    {
        return
        (
            FirstNames[Rng.RandiRange(0, FirstNames.Count - 1)],
            LastNames[Rng.RandiRange(0, LastNames.Count - 1)]
        );
    }

    public static Game GetGlobalInstance(Node caller)
    {
        return caller.GetNode<Game>("/root/Game");
    }
    public static Node2D GetWorldInstance(Node caller)
    {
        return caller.GetNode<Node2D>("/root/World");
    }

    public static Player GetPlayerInstance(Node caller)
    {
        return caller.GetNode<Player>("/root/World/Player");
    }
}