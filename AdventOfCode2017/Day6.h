#pragma once
#include <string>
#include <vector>
#include <fstream>
#include <iostream>
#include <algorithm>
#include "Helpers.h"

using namespace std;

class Day6
{
private:
	string State(const vector<int>& memoryBanks);
public:
	void Part1();
	void Part2();
};
