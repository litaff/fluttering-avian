namespace fluttering_avian.logger;

using Godot;
using Logger;

public class GodotLogger : ILogger
{
    public void Log(string message)
    {
        GD.Print(message);
    }

    public void LogWarning(string message)
    {
        GD.PushWarning(message);
    }

    public void LogError(string message)
    {
        GD.PushError(message);
    }
}