#pragma once
#include <string>
#include <vector>
#include <algorithm>
#include <iostream>
#include <iomanip>
#include "Helpers.h"

using namespace std;

class Day10
{
private:
	void ReverseLength(vector<int>& sequence, int length);
	void KnotHashRound(vector<int>& sequence, const string& input);

	int _currentPosition = 0;
	int _skipSize = 0;
	const int _listSize = 256;

public:
	void Part1();
	void Part2();
	string KnotHash(const string& input);
};