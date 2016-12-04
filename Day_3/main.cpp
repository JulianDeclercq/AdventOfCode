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

	int ctr = 0;
	for (int i = 0; i < lines.size(); i += 3)
	{
		std::smatch match;// , match2, match3;
		std::vector<int> tri, tri2, tri3;

		if (std::regex_match(lines[i], match, re))
		{
			tri.push_back(std::stoi(match[1].str()));
			tri2.push_back(std::stoi(match[2].str()));
			tri3.push_back(std::stoi(match[3].str()));
		}
		if (std::regex_match(lines[i + 1], match, re))
		{
			tri.push_back(std::stoi(match[1].str()));
			tri2.push_back(std::stoi(match[2].str()));
			tri3.push_back(std::stoi(match[3].str()));
		}
		if (std::regex_match(lines[i + 2], match, re))
		{
			tri.push_back(std::stoi(match[1].str()));
			tri2.push_back(std::stoi(match[2].str()));
			tri3.push_back(std::stoi(match[3].str()));
		}

		if (CheckIfTriangle(tri)) ++ctr;
		if (CheckIfTriangle(tri2)) ++ctr;
		if (CheckIfTriangle(tri3)) ++ctr;
	}

	std::cout << "Answer is: " << ctr << std::endl;

	return 0;
}