#include "Day19.h"

void Day19::ParseInput()
{
	//ifstream input("input/day19example.txt");
	//ifstream input("input/day19example2.txt");
	ifstream input("input/day19.txt");

	if (!input)
	{
		cout << "Failed to open input." << endl;
		return;
	}

	string line = "";
	regex singleRule("(\\d+): \"(\\w)\"");
	regex doubleRule("(\\d+): (\\d+) (\\d+)");
	regex alternateDoubleRule("(\\d+): (\\d+) (\\d+) . (\\d+) (\\d+)");

	regex directRule("(\\d+): (\\d+)");
	regex alwaysValidRule("(\\d+): (\\d+) . (\\d+)"); // exception for a rule that turned out to always be valid, not sure if this would work with different input
	smatch match;
	bool parsingRules = true; // if false, messages are being parsed
	while (getline(input, line))
	{
		// messages are parsed after an empty line
		if (line.empty())
		{
			parsingRules = false;
			continue;
		}

		if (!parsingRules)
		{
			_messages.push_back(line);
			continue;
		}

		if (regex_match(line, match, alternateDoubleRule))
		{
			_rules[stoi(match[1])] = (Rule(Rule::Type::AlternateDouble,
									{ stoi(match[2]), stoi(match[3]) },
									{ stoi(match[4]), stoi(match[5]) }));
		}
		else if (regex_match(line, match, doubleRule))
		{
			_rules[stoi(match[1])] = (Rule(Rule::Type::Double, { stoi(match[2]), stoi(match[3]) }));
		}
		else if (regex_match(line, match, singleRule))
		{
			_rules[stoi(match[1])] = (Rule(Rule::Type::Single, static_cast<string>(match[2])[0]));
		}
		else if (regex_match(line, match, directRule))
		{
			_rules[stoi(match[1])] = (Rule(Rule::Type::Direct, { stoi(match[2]), -1 }));
		}
		else if (regex_match(line, match, alwaysValidRule))
		{
			_rules[stoi(match[1])] = (Rule(Rule::Type::AlwaysValid));
		}
		else cout << "invalid rule: " << line << endl;
	}
}

bool Day19::CheckRule(const Rule& rule, const string& message)
{
	if (message.empty())
		return false;
	
	bool b = false;
	switch (rule.Type)
	{
	case Rule::Type::Single:
		b = message.front() == rule.Character && message.size() == 1;
		return b;
	case Rule::Type::Double:
		return CheckDoubleRule(rule.Rules, message);
	case Rule::Type::AlternateDouble:
		return CheckDoubleRule(rule.Rules, message) || CheckDoubleRule(rule.AltRules, message);
	case Rule::Type::Direct:
		return CheckRule(_rules[rule.Rules.first], message);
	case Rule::Type::AlwaysValid:
		return true;
	case Rule::Type::Invalid:
	default: 
		cout << "ERROR: invalid rule" << endl;
		return false;
	}
}

bool Day19::CheckDoubleRule(const pair<int, int>& rules, const string& message)
{
	/*
	The remaining rules list the sub-rules that must be followed; for example,
	the rule 0: 1 2 means that to match rule 0, the text being checked must match rule 1,
	and the text after the part that matched rule 1 must then match rule 2.
	*/

	// TODO: "the text after the part", currently it's just offset by 1, check if this is too naive

	auto peekType = _rules[rules.first].Type;
	string msg = peekType == Rule::Type::Single ? message.substr(0, 1) : message.substr(0, 2);
	
	peekType = _rules[rules.second].Type;
	string msg2 = peekType == Rule::Type::Single ? message.substr(msg.size(), 1) : message.substr(msg.size(), 2);

	return CheckRule(_rules[rules.first], msg) && CheckRule(_rules[rules.second], msg2);
}

int Day19::PartOne()
{
	int count = 0;
	for (const auto& msg : _messages)
	{
		if (CheckRule(_rules[0], msg))
		{
			++count;
			cout << msg << endl;
		}
	}
	return count;
}

int Day19::PartTwo()
{
	return 0;
}
