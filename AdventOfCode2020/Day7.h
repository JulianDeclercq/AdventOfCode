#pragma once

#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <algorithm>
#include <map>
#include <regex>
#include <set>

using namespace std;

class Day7
{
private:
	map<string, vector<pair<int, string>>> _bags;
	map<string, vector<string>> _parentBags;

	void ParseBagContents(const string& bagName, string contentsDescription);
	void AddToParentBags(const string& bagName, set<string>& parentBags);
	int ContentCount(const string& bagName, bool root = false);

public:
	void ParseInput();
	int PartOne();
	int PartTwo();
};