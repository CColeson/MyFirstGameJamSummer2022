public static class SailSpeeds
{
    public static SailSpeed Full = new SailSpeed(150, "full");
    public static SailSpeed Half = new SailSpeed(110, "half");
    public static SailSpeed Low = new SailSpeed(50, "low");
}

public struct SailSpeed
{
    public float Speed;
    public string Name;
    public SailSpeed(float speed, string name)
    {
        Speed = speed;
        Name = name;
    }
}