#pragma once

#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <algorithm>
#include <regex>
#include <bitset>
#include <map>
#include <numeric>

using namespace std;
using ull = unsigned long long;

class Day14
{
private:
	map<int, ull> _memory;
	vector<string> _instructions;
	string _mask;
	void SetMask(string& mask);
	ull ApplyMask(ull target);
	void WriteMemory(int at, ull value);

public:
	void ParseInput();
	ull PartOne();
	ull PartTwo();
};
