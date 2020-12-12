#pragma once

#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <algorithm>
#include <map>

using namespace std;

class Day10
{
private:
	vector<int> _adaptors;
	long long _count = 0;
	void CountArrangementsOld(int last, int offset);

	map<int, long long> _memo;
	long long CountArrangements(int step);
public:
	void ParseInput();
	int PartOne();
	long long PartTwo();
};