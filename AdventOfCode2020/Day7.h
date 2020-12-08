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
	bool _inputParsed = false;
	map<string, vector<pair<int, string>>> _bags;
	map<string, vector<string>> _parentBags;
	void ParseInput();
	void ParseBagContents(const string& bagName, string& contentsDescription);
	void AddToParentBags(const string& bagName, set<string>& parentBags);
public:
	int PartOne();
	int PartTwo();
};