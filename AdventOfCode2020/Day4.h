#pragma once

#include <iostream>
#include <regex>
#include <fstream>
#include <string>
#include <vector>
#include <algorithm>
#include <map>

using namespace std;
using passport = map<string, string>;

class Day4
{
private:
	const vector<string> _requiredKeys = { "byr","iyr","eyr","hgt","hcl","ecl","pid" };// , "cid"
	vector<passport> _passports;
	bool _inputParsed = false;
	void ParseInput();
	void ParsePassport(string& s);
	bool RequiredKeysPresent(const passport& p);
	bool RequiredKeysValid(const passport& p);
public:
	Day4() {};
	int PartOne();
	int PartTwo();
};
