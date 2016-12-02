#include <iostream>
#include <string>
#include <fstream>
#include <vector>
#include <map>

std::vector<std::string> commands;
std::map<char, int> actions;
int curNr = 5;

void LoadActions()
{
	/*
		1 2 3
		4 5 6
		7 8 9
	*/
	actions.insert(std::make_pair('U', -3));
	actions.insert(std::make_pair('R', +1));
	actions.insert(std::make_pair('D', +3));
	actions.insert(std::make_pair('L', -1));
}

void LoadActions2()
{
	/*
		1
	  2 3 4
	5 6 7 8 9
	  A B C
		D
	*/
	actions.insert(std::make_pair('U', -4)); // Regular UP
	actions.insert(std::make_pair('R', +1));
	actions.insert(std::make_pair('D', +4)); // Regular DOWN
	actions.insert(std::make_pair('L', -1));
}

bool CommandValid(const char c)
{
	switch (c)
	{
	case 'U': return (curNr > 3);
	case 'R': return (curNr % 3 != 0);
	case 'D': return (curNr < 7);
	case 'L': return (curNr % 3 != 1);
	}

	std::cout << "Something went wrong in CommandValid.\n" << std::endl;
	return false;
}

bool CommandValid2(const char c, bool& specialCmd)
{
	// Have to be declared before switch statement
	std::vector<int> v = std::vector<int>();
	std::vector<int>::iterator it = std::vector<int>::iterator();

	switch (c)
	{
	case 'U': v = { 1, 2, 4, 5, 9 };
			  if (curNr == 13 || curNr == 3) { specialCmd = true; };
			  break;
	case 'R': v = { 1, 4, 9, 12, 13 };
			  break;
	case 'D': v = { 5, 9, 10, 12, 13 };
			  if (curNr == 11 || curNr == 1) { specialCmd = true; };
			  break;
	case 'L': v = { 1, 2, 5, 10, 13 };
			  break;
	}

	it = std::find(v.begin(), v.end(), curNr);
	return (it == v.end());
}

void ExecuteCommandLine(const std::string& cmd)
{
	for (const char& c : cmd)
	{
		//if (CommandValid(c))
		bool specialCmd = false;
		if (CommandValid2(c, specialCmd))
		{
			curNr += (specialCmd) ? actions.at(c) / 2 : actions.at(c);
		}
	}

	// Translating numbers higher than 10 to A B C D
	char answer = (curNr > 10) ? static_cast<char>(curNr % 10 + 'A') : static_cast<char>(curNr + '0');
	std::cout << "Number: " << answer << std::endl;
}

int main()
{
	std::ifstream file("input.txt");
	if (file.fail())
	{
		std::cout << "Failed to open inputfile\n";
		return -1;
	}

	std::string line;
	while (std::getline(file, line))
	{
		commands.push_back(line);
	}
	file.close();

	// Part 1
	//LoadActions();
	LoadActions2();
	for (const std::string& line : commands)
	{
		ExecuteCommandLine(line);
	}

	return 0;
}