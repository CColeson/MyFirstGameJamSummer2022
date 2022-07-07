using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Game : Godot.Node
{
    public CrewMemberFactory CrewMemberGenerator;
    public override void _Ready()
    {
        base._Ready();
        var firstNames = new List<string>();
        var file = new Godot.File();
        file.Open("res://Assets/Etc/Names/First.pirate", File.ModeFlags.Read);
        string content;
        while(!string.IsNullOrEmpty(content = file.GetLine()))
        {
            firstNames.Add(content.StripEdges());
        }
        file.Close();

        var lastNames = new List<string>();
        file.Open("res://Assets/Etc/Names/Last.pirate", File.ModeFlags.Read);
        while(!string.IsNullOrEmpty(content = file.GetLine()))
        {
            lastNames.Add(content.StripEdges());
        }
        file.Close();
        CrewMemberGenerator = new CrewMemberFactory(firstNames, lastNames);
    }

    public static Game GetGlobalInstance(Node caller)
    {
        return caller.GetNode<Game>("/root/Game");
    }
    public static World GetWorldInstance(Node caller)
    {
        return caller.GetNode<World>("/root/World");
    }
    
    public static Player GetPlayerInstance(Node caller)
    {
        return caller.GetNode<Player>("/root/World/Player");
    }
}