namespace AdventOfCode2023.helpers;

public static class Qonsole
{
    public static void OverWrite(string newMessage)
    {
        var eraseOld = string.Concat(Enumerable.Repeat("\b \b", _lastMessage.Length));
        Console.Write(eraseOld);
        Console.Write(newMessage);
        _lastMessage = newMessage;
    }

    public static void WriteLine(string message)
    {
        Console.WriteLine(message);
        _lastMessage = message;
    }
    
    private static string _lastMessage = "";
}