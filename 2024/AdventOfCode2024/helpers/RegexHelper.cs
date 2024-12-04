using System.Text.RegularExpressions;

namespace AdventOfCode2024.helpers;

public class RegexHelper
{
    public RegexHelper(Regex r, params string[] groupNames)
    {
        _regex = r;
        _groupNames = groupNames;
    }

    public bool Match(string line)
    {
        if (!_regex.IsMatch(line))
            return false;
     
        _groups.Clear();
        
        var match = _regex.Match(line);
        for (var i = 0; i < _groupNames.Length; ++i)
            _groups.Add(_groupNames[i], match.Groups[i + 1].ToString());

        return true;
    }

    public string Get(string groupName) => _groups[groupName];
    public int GetInt(string groupName) => int.Parse(_groups[groupName]);
    public bool IsEmpty(string groupName) => string.IsNullOrEmpty(_groups[groupName]);
    public bool TryGetInt(string groupName, out int value) => int.TryParse(_groups[groupName], out value);
    
    public string this[string groupName] => _groups[groupName];

    private Regex _regex;
    private Dictionary<string, string> _groups = new();
    private string[] _groupNames;
}