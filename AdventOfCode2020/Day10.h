#pragma once

#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <algorithm>

using namespace std;

class Day10
{
private:
	vector<int> _adaptors;
public:
	void ParseInput();
	int PartOne();
	int PartTwo();
};