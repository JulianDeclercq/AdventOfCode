#pragma once

#include <iostream>
#include <regex>
#include <fstream>
#include <string>
#include <vector>
#include <algorithm>
#include <map>

using namespace std;
using range = pair<int, int>;

class Day5
{
private:
	vector<string> _boardingPasses;
	bool _inputParsed = false;
	void ParseInput();
	int Partitioning(const range& range, const string& operation, const char lower, const char upper);
public:
	Day5() {};
	int Part1();
	int Part2();
};