#pragma once
#include <string>
#include <vector>
#include <algorithm>
#include <iostream>
#include "Helpers.h"

using namespace std;

class Day10
{
private:
	void ReverseLength(vector<int>& sequence, int length);

	int _currentPosition = 0;
	int _skipSize = 0;

public:
	void Part1();
};