#pragma once

#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <algorithm>
#include <map>
#include <regex>
#include "Helpers.h"

using namespace std;
using ticket = vector<int>;

struct InclusiveRange
{
	InclusiveRange(int min, int max) : Min(min), Max(max)
	{
	}

	int Min = 0, Max = 1;

	bool Contains(int n) const
	{
		return n >= Min && n <= Max;
	}
};

class Day16
{
private:
	map<string, vector<InclusiveRange>> _fields;
	vector<ticket> _tickets;
	ticket _ticket; // "your ticket"

	vector<InclusiveRange> _allRanges; // shortcut for part 1 solution

public:
	void ParseInput();
	int PartOne();
	int PartTwo();
};