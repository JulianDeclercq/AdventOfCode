#include "Day16.h"

void Day16::ParseInput()
{
	//ifstream input("input/day16example.txt");
	//ifstream input("input/day16example2.txt");
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
			const ticket& t = Helpers::ParseNumbersSeparatedByCommas(line);
			_tickets.push_back(t);
		}
	}
}

bool Day16::FitInFieldRanges(const vector<int>& values, const vector<InclusiveRange>& ranges)
{
	for (int value : values)
	{
		bool fits = false;

		for (const auto& range : ranges)
		{
			if (range.Contains(value))
			{
				fits = true;
				break;
			}
		}

		if (!fits)
			return false;
	}
	return true;
}

void Day16::RemoveFromPossibilities(const string& field, size_t ignoreIdx)
{
	for (size_t i = 0; i < _possibleFieldsPerColumn.size(); ++i)
	{
		if (i == ignoreIdx)
			continue;

		auto& possibilities = _possibleFieldsPerColumn[i];
		const auto& it = find(possibilities.begin(), possibilities.end(), field);
		if (it != possibilities.end())
			possibilities.erase(it);
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

ull Day16::PartTwo()
{
	// discard the invalid tickets entirely
	vector<ticket> validTickets;
	for (const auto& ticket : _tickets)
	{
		bool validTicket = true;
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
			{
				validTicket = false;
				break;
			}
		}
		if (validTicket)
			validTickets.push_back(ticket);
	}

	for (const auto& ticket : validTickets)
	{
		if (_valuesPerColumn.size() == 0)
			_valuesPerColumn.resize(ticket.size());

		for (size_t i = 0; i < ticket.size(); ++i)
			_valuesPerColumn[i].push_back(ticket[i]);
	}

	_possibleFieldsPerColumn.resize(_valuesPerColumn.size());
	for (size_t col = 0; col < _valuesPerColumn.size(); ++col)
	{
		for (const auto& pair : _fields)
		{
			if (FitInFieldRanges(_valuesPerColumn[col], pair.second))
				_possibleFieldsPerColumn[col].push_back(pair.first);
		}
	}

	while (_fieldIndices.size() < _fields.size())
	{
		for (size_t i = 0; i < _possibleFieldsPerColumn.size(); ++i)
		{
			const auto& possibilities = _possibleFieldsPerColumn[i];
			if (possibilities.size() != 1)
				continue;

			_fieldIndices[possibilities.front()] = i;

			// check if this was the last field that needed to be mapped to avoid unnecessary work
			if (_fieldIndices.size() == _fields.size())
				break;

			RemoveFromPossibilities(possibilities.front(), i);
		}
	}

	const string startsWith = "departure";
	ull answer = 1;

	for (const auto& field : _fieldIndices)
	{
		if (field.first.substr(0, startsWith.size()).compare(startsWith) == 0)
			answer *= _ticket[field.second];

		//cout << field.first << ": " << _ticket[field.second] << endl;
	}

	return answer;
}
