namespace SpeedokuRoyaleServer.Utility;

public static class IdGenerator
{
    public static string NewId() => Guid.NewGuid().ToString();
}
