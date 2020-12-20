#pragma once

#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <algorithm>
#include <regex>
#include <map>

using namespace std;

class Day19
{
private:

	struct Rule
	{
		enum class Type
		{
			Invalid,
			Single,
			Double,
			AlternateDouble,
			Direct,
			AlwaysValid
		};

		Rule(){}

		Rule(Type t) : Type(t)
		{

		}

		Rule(Type t, char c) : Type(t), Character(c)
		{

		}

		Rule(Type t, pair<int, int> rules) : Type(t), Rules(rules)
		{

		}

		Rule(Type t, pair<int, int> rules, pair<int, int> altRules) : Type(t), Rules(rules), AltRules(altRules)
		{

		}

		char Character = ' ';
		pair<int, int> Rules = {-1, -1};
		pair<int, int> AltRules = {-1, -1};
		Type Type = Type::Invalid;
	};

	map<int, Rule> _rules;
	vector<string> _messages;
	bool CheckRule(const Rule& rule, const string& message);
	bool CheckDoubleRule(const pair<int, int>& rules, const string& message);
public:
	void ParseInput();
	int PartOne();
	int PartTwo();
};