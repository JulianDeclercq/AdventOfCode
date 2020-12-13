#pragma once

#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <algorithm>
#include <sstream>
#include <limits>
#include <map>

using namespace std;

class Day13
{
private:
	int _target = 0, _earliest = INT_MAX, _busId = 0;
	vector<int> _busses;
	map<int, int> _offsets;
public:
	void ParseInput();
	int PartOne();
	long long PartTwo();
};
