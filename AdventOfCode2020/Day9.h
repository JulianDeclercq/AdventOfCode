#pragma once

#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <algorithm>

using namespace std;

class Day9
{
private:
	vector<long long> _numbers;
	int _preamble = 25;
public:
	void ParseInput();
	long long PartOne();
	int PartTwo();
};