#pragma once
#include <iostream>
#include <string>
#include <fstream>
#include <vector>
#include <regex>
#include <algorithm>
#include "Helpers.h"

using namespace std;

class Day16
{
private:
	void ExecuteMove(string& programOrder, const string& move);
public:
	void Part1();
};