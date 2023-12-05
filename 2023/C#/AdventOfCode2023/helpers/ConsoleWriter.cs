namespace AdventOfCode2023.helpers;

public class ConsoleWriter
{
    public void OverWrite(string newMessage)
    {
        var eraseOld = string.Concat(Enumerable.Repeat('\b', _lastMessage.Length));
        Console.Write($"{eraseOld}{newMessage}");
        _lastMessage = newMessage;
    }
    
    private string _lastMessage = "";
}