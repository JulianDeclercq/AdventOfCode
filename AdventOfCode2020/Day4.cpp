#include "Day4.h"

void Day4::ParseInput()
{
	if (_inputParsed)
		return;

	//ifstream input("input/day4example.txt");
	//ifstream input("input/day4example2.txt");
	//ifstream input("input/day4example3.txt");
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
		// save the key from current iterator
		string key = *it;

		// move iterator to next submatch, the value
		it++;

		// add passport entry
		p.insert(make_pair(key, *it));

		// move iterator to next submatch, the new key if it exists
		it++;
	}
	_passports.push_back(p);
}

bool Day4::RequiredKeysPresent(const passport& p)
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

bool Day4::RequiredKeysValid(const passport& p)
{
	/*	byr (Birth Year) - four digits; at least 1920 and at most 2002.
		iyr (Issue Year) - four digits; at least 2010 and at most 2020.
		eyr (Expiration Year) - four digits; at least 2020 and at most 2030.
		hgt (Height) - a number followed by either cm or in:
		If cm, the number must be at least 150 and at most 193.
		If in, the number must be at least 59 and at most 76.
		hcl (Hair Color) - a # followed by exactly six characters 0-9 or a-f.
		ecl (Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
		pid (Passport ID) - a nine-digit number, including leading zeroes.
		cid (Country ID) - ignored, missing or not.	*/

	int byr = stoi(p.at("byr"));
	if (byr < 1920 || byr > 2002)
		return false;

	int iyr = stoi(p.at("iyr"));
	if (iyr < 2010 || iyr > 2020)
		return false;

	int eyr = stoi(p.at("eyr"));
	if (eyr < 2020 || eyr > 2030)
		return false;

	regex hgtRegex("(\\d+)(\\w+)");
	auto match = smatch();
	if (!regex_search(p.at("hgt"), match, hgtRegex))
		return false;

	int height = stoi(match[1]);
	if (match[2].compare("cm") == 0)
	{
		if (height < 150 || height > 193)
			return false;
	}
	else if (match[2].compare("in") == 0)
	{
		if (height < 59 || height > 76)
			return false;
	}
	else return false;

	regex hclRegex("#(?:[0-9]|[a-f]){6}");
	if (!regex_search(p.at("hcl"), match, hclRegex))
		return false;

	const vector<string> colours{ "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
	if (find(colours.begin(), colours.end(), p.at("ecl")) == colours.end())
		return false;

	if (p.at("pid").size() != 9)
		return false;

	for (char c : p.at("pid"))
	{
		if (!isdigit(c))
			return false;
	}

	// all requirements have been met
	return true;
}

int Day4::PartOne()
{
	ParseInput();
	return count_if(_passports.begin(), _passports.end(), [this](const auto& p) {return RequiredKeysPresent(p); });
}

int Day4::PartTwo()
{
	ParseInput();
	return count_if(_passports.begin(), _passports.end(), [this](const auto& p) {return RequiredKeysPresent(p) && RequiredKeysValid(p); });
}
