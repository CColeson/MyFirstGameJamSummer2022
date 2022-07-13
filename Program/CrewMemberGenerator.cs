using System;
using System.Collections.Generic;
using Godot;

public partial class Game : Node
{
    public class CrewMemberFactory
    {
        public List<string> FirstNames;
        public List<string> LastNames;
        private RandomNumberGenerator Rng = new RandomNumberGenerator();
        public CrewMemberFactory(List<string> firstNames, List<string> lastNames)
        {
            Rng.Randomize();
            FirstNames = firstNames;
            LastNames = lastNames;
        }

        public CrewMember Generate()
        {
            var n = GetRandomName();
            var h = GetRandomHPValue();
            return new CrewMember(n.Item1, n.Item2, h, h);
        }

        private int GetRandomHPValue()
        {
            return Rng.RandiRange(2, 5);
        }

        private (string, string) GetRandomName()
        {
            return
            (
                FirstNames[Rng.RandiRange(0, FirstNames.Count - 1)].ToLower(),
                LastNames[Rng.RandiRange(0, LastNames.Count - 1)].ToLower()
            );
        }
    }
}
