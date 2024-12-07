using System.Text.RegularExpressions;

namespace AdventOfCode2024.helpers;

public class RegexHelper(Regex r, params string[] groupNames)
{
    public bool Match(string line)
    {
        if (!r.IsMatch(line))
            return false;
     
        _groups.Clear();
        
        var match = r.Match(line);
        for (var i = 0; i < groupNames.Length; ++i)
            _groups.Add(groupNames[i], match.Groups[i + 1].ToString());

        return true;
    }

    public string Get(string groupName) => _groups[groupName];
    public int GetInt(string groupName) => int.Parse(_groups[groupName]);
    public long GetLong(string groupName) => long.Parse(_groups[groupName]);
    public bool IsEmpty(string groupName) => string.IsNullOrEmpty(_groups[groupName]);
    public bool TryGetInt(string groupName, out int value) => int.TryParse(_groups[groupName], out value);
    
    public string this[string groupName] => _groups[groupName];

    private readonly Dictionary<string, string> _groups = new();
}