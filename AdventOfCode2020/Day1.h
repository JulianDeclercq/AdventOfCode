#pragma once

#include <string>
#include <iostream>
#include <fstream>
#include <vector>

using namespace std;

class Day1
{
private:
	vector<int> _expenses = vector<int>();

public:
	void ParseInput();
	int PartOne();
	int PartTwo();
};
