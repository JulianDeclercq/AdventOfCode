#pragma once

#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <algorithm>
#include <sstream>
#include <map>
#include "Helpers.h"

using namespace std;

struct SpokenNumber
{
	int TimesSpoken = 0;

	int TurnsApart()
	{
		if (_spokenAt == -1 || _spokenAtBefore == -1)
		{
			cout << "Can't calculate TurnsApart" << endl;
			return -1;
		}
		return _spokenAt - _spokenAtBefore;
	}

	void SpeakAt(int idx)
	{
		_spokenAtBefore = _spokenAt;
		_spokenAt = idx;
	}

private:
	int _spokenAt = -1, _spokenAtBefore = -1;
};

class Day15
{
private:
	vector<int> _numbers;
	int _current = 0, _idx = 1;
	map<int, SpokenNumber> _lastSpoken;
	
	void InitialSpeak(int number);
	void Next();
	int SpeakAtRound(int round);
public:
	void ParseInput();
	int PartOne();
	int PartTwo();
};
#pragma once
