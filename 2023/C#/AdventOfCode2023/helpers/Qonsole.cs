namespace AdventOfCode2023.helpers;

public static class Qonsole
{
    public static void OverWrite(string newMessage)
    {
        var eraseOld = string.Concat(Enumerable.Repeat('\b', _lastMessage.Length));
        Console.Write($"{eraseOld}{newMessage}");
        _lastMessage = newMessage;
    }

    public static void WriteLine(string message)
    {
        // If there was a previous message, take a new line
        if (!string.IsNullOrWhiteSpace(_lastMessage))
            Console.WriteLine();
        
        Console.WriteLine(message);
    }
    
    private static string _lastMessage = "";
}