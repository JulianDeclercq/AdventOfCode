#include <iostream>
#include <fstream>
#include <regex>
#include <string>
#include <vector>

std::vector<std::string> lines;

bool CheckIfTriangle(const std::vector<int>& sides)
{
	const bool a = (sides[0] + sides[1] > sides[2]);
	const bool b = (sides[0] + sides[2] > sides[1]);
	const bool c = (sides[1] + sides[2] > sides[0]);
	return (a && b && c);
}
int main()
{
	std::ifstream file("input.txt");

	std::string line;
	while (std::getline(file, line))
	{
		lines.push_back(line);
	}
	file.close();

	//std::regex re("\s+(\d*)\s+(\d*)\s+(\d*)");
	std::regex re("\\s+(\\d*)\\s+(\\d*)\\s+(\\d*)");
	std::smatch match;

	int ctr = 0;
	for (const std::string& str : lines)
	{
		if (std::regex_match(str, match, re))
		{
			if (match.size() != 4)
			{
				return -1;
			}

			std::vector<int> sides{ std::stoi(match[1].str()) , std::stoi(match[2].str()) , std::stoi(match[3].str()) };
			if (CheckIfTriangle(sides))
			{
				++ctr;
			}
		}
	}

	std::cout << "Answer is: " << ctr << std::endl;

	return 0;
}