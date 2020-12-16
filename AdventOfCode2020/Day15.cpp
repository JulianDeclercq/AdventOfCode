#include "Day15.h"

void Day15::ParseInput()
{
	//string input = "0,3,6";
	string input = "8,0,17,4,1,12";

    stringstream ss(input);

    for (int i; ss >> i;) 
    {
        _numbers.push_back(i);
        if (ss.peek() == ',')
            ss.ignore();
    }
}

void Day15::InitialSpeak(int number)
{
    _current = number;

    auto n = SpokenNumber();
    n.SpokenAt.push_back(_idx);
    n.TimesSpoken++;

    _lastSpoken[_current] = n;
    //cout << "Turn " << _idx << ": " << _current << endl;
    ++_idx;
}

void Day15::Next()
{
    if (_lastSpoken.find(_current) == _lastSpoken.end())
    {
        cout << "couldn't find " << _current << endl;
    }
    else
    {
        if (_lastSpoken[_current].TimesSpoken == 1) // first time it was spoken
        {
            _current = 0; // say "0"
        }
        else
        {
            // how many turns apart the number is from when it was previously spoken
            //_current = _idx - _lastSpoken[_current].first;
            _current = _lastSpoken[_current].TurnsApart();
        }
        
        // !! _current has changed at this stage
        _lastSpoken[_current].TimesSpoken++;
        _lastSpoken[_current].SpokenAt.push_back(_idx);
    }
    //cout << "Turn " << _idx << ": " << _current << endl;
    ++_idx;
}

int Day15::PartOne()
{
    int targetTurn = 2020;
    for (size_t i = 0; i < _numbers.size(); ++i)
        InitialSpeak(_numbers[i]);

    while (_idx < targetTurn + 1) // + 1 since turns start counting from 1, not 0
        Next();

	return _current;
}

int Day15::PartTwo()
{
	return 0;
}
