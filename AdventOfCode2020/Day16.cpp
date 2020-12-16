#include "Day16.h"

void Day16::ParseInput()
{
	//ifstream input("input/day16example.txt");
	ifstream input("input/day16.txt");

	if (!input)
	{
		cout << "Failed to open input." << endl;
		return;
	}

	string line = "";
	bool parsingFields = true;
	regex fieldR("(.+): (\\d+)-(\\d+) or (\\d+)-(\\d+)");
	smatch match;
	while (getline(input, line))
	{
		if (line.empty())
		{
			getline(input, line);
			if (line.compare("your ticket:") == 0)
			{
				getline(input, line);
				_ticket = Helpers::ParseNumbersSeparatedByCommas(line);
			}
			else if (line.compare("nearby tickets:") == 0)
			{
				parsingFields = false;
			}
			continue;
		}

		if (parsingFields)
		{
			if (!regex_match(line, match, fieldR))
			{
				cout << "No match for regex" << endl;
				continue;
			}

			InclusiveRange first(stoi(match[2]), stoi(match[3]));
			InclusiveRange second(stoi(match[4]), stoi(match[5]));
			_fields[match[1]] = { first, second };

			_allRanges.push_back(first);
			_allRanges.push_back(second);
		}
		else // parsing nearby tickets
		{
			_tickets.push_back(Helpers::ParseNumbersSeparatedByCommas(line));
		}
	}
}

int Day16::PartOne()
{
	int tser = 0; // ticket scanning error rate
	for (const ticket& ticket : _tickets)
	{
		for (const int value : ticket)
		{
			bool valid = false;

			for (const InclusiveRange& range : _allRanges)
			{
				if (range.Contains(value))
				{
					valid = true;
					break;
				}
			}

			if (!valid)
				tser += value;
		}
	}
	return tser;
}

int Day16::PartTwo()
{
	return 0;
}
