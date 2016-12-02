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
	//UP RIGHT DOWN LEFT
	actions.insert(std::make_pair('U', -3));
	actions.insert(std::make_pair('R', +1));
	actions.insert(std::make_pair('D', +3));
	actions.insert(std::make_pair('L', -1));
}

bool CommandValid(const char c)
{
	/*	1 2 3
		4 5 6
		7 8 9	*/
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

void ExecuteCommandLine(const std::string& cmd)
{
	for (const char& c : cmd)
	{
		if (CommandValid(c))
		{
			curNr += actions.at(c);
		}
	}
	std::cout << "Number: " << curNr << std::endl;
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

	LoadActions();

	for (const std::string& line : commands)
	{
		ExecuteCommandLine(line);
	}

	return 0;
}