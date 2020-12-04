#include "Day4.h"

void Day4::ParseInput()
{
	if (_inputParsed)
		return;

	//ifstream input("input/day4example.txt");
	ifstream input("input/day4.txt");

	if (!input)
	{
		cout << "Failed to open input." << endl;
		return;
	}

	string line = "", current = "";
	while (getline(input, line))
	{
		if (line.empty()) // Passports are separated by blank lines
		{
			ParsePassport(current);
			current = "";
			continue;
		}

		if (!current.empty())
			current += ' '; // add a space so all pairs are separated by space instead of newline

		current += line;
	}

	// make sure last passport gets parsed as well (without manipulating the input)
	if (!current.empty())
		ParsePassport(current);

	_passports[0];
	_inputParsed = true;
}

void Day4::ParsePassport(string& s)
{
	regex reg("(\\w+):([^\\s]+)");
	auto match = smatch();
	auto p = passport();

	// default constructor = end of sequence
	const auto rend = regex_token_iterator<string::iterator>();
	const int submatches[] = { 1, 2 };
	regex_token_iterator<string::iterator> it(s.begin(), s.end(), reg, submatches);
	while (it != rend)
	{
		// save the key from current iterator value
		string key = *it;

		// move iterator to next submatch, the value
		it++;

		// add passport entry
		p.insert(make_pair(key, *it)); // add passport entry

		// move iterator to next submatch, the new key if it exists
		it++;
	}
	_passports.push_back(p);
}

bool Day4::IsValid(const passport& p)
{
	// create a list of keys that the passport contains
	auto keys = vector<string>();
	keys.reserve(p.size());
	for (const auto& pair : p)
		keys.push_back(pair.first);
	
	// check if all required keys are present
	if (keys.size() < _requiredKeys.size())
		return false;

	for (string key : _requiredKeys)
	{
		if (find(keys.begin(), keys.end(), key) == keys.end())
			return false;
	}

	// all requirements have been met
	return true;
}

int Day4::Part1()
{
	ParseInput();
	return count_if(_passports.begin(), _passports.end(), [this](const auto& p) {return IsValid(p); });
}

int Day4::Part2()
{
	ParseInput();
	return 0;
}
