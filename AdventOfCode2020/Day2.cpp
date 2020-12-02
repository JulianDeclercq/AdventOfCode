#include "Day2.h"

void Day2::ParseInput()
{
	if (_inputParsed)
		return;

	//ifstream input("input/day2example.txt");
	ifstream input("input/day2.txt");

	if (!input)
	{
		cout << "Failed to open input." << endl;
		return;
	}

	string line = "";
	smatch match;
	regex reg("(\\d+)-(\\d+) (\\w): (\\w+)");

	while (getline(input, line))
	{
		if (!regex_search(line, match, reg))
		{
			cout << "Didn't match regex pattern for " << line << endl;
			continue;
		}
		
		_passwords.push_back(Password(
			stoi(match[1]), 
			stoi(match[2]), 
			static_cast<string>(match[3])[0], 
			static_cast<string>(match[4])));
	}

	_inputParsed = true;
}

bool Day2::IsValid(const Password& password)
{
	int count = count_if(password.Contents.begin(), password.Contents.end(), [password](const char c) {return c == password.Requirement; });
	return count >= password.Minimum && count <= password.Maximum;
}

int Day2::Part1()
{
	ParseInput();
	return count_if(_passwords.begin(), _passwords.end(), [this](const Password& p) {return IsValid(p); });
}
