#include <iostream>
#include <string>
#include <vector>
#include <fstream>
#include <utility>
#include <set>

enum Direction {
	NORTH = 0,
	EAST = 1,
	SOUTH = 2,
	WEST = 3
};
Direction facing = NORTH;

int xCoord = 0, yCoord = 0;
std::set<std::pair<int, int>> visitedPos;
bool stopExec = false;
bool part1 = false;

void ParseWithDelim(const std::string input, const std::string& delim, std::vector<std::string>& parts)
{
	std::string s = input; // Make a non-const copy

	size_t pos = 0;
	std::string token;
	while ((pos = s.find(delim)) != std::string::npos) {
		token = s.substr(0, pos);
		parts.push_back(token);
		s.erase(0, pos + delim.length());
	}
	parts.push_back(s);
}

void ExecuteCommand(const std::string& cmd)
{
	if (stopExec)
	{
		return;
	}

	const int dir = (cmd.front() == 'R') ? 1 : -1;
	const int calcDir = (static_cast<int>(facing) + dir) % 4;
	facing = (calcDir == -1) ? WEST : static_cast<Direction>(calcDir);

	int steps = std::stoi(cmd.substr(1));

	if (part1)
	{
		switch (facing)
		{
		case NORTH: yCoord += steps; break;
		case EAST:  xCoord += steps; break;
		case SOUTH: yCoord -= steps; break;
		case WEST:  xCoord -= steps; break;
		default: std::cout << "Invalid facing\n"; break;
		}
	}
	else
	{
		for (int i = 0; i < steps; ++i)
		{
			switch (facing)
			{
			case NORTH: yCoord += 1; break;
			case EAST:  xCoord += 1; break;
			case SOUTH: yCoord -= 1; break;
			case WEST:  xCoord -= 1; break;
			default: std::cout << "Invalid facing\n"; break;
			}

			std::pair<int, int> newPos = std::make_pair(xCoord, yCoord);
			if (visitedPos.find(newPos) != visitedPos.end())
			{
				std::cout << "Double visited pos is " << newPos.first << ' ' << newPos.second << " -> " << abs(newPos.first) + abs(newPos.second) << " blocks away" << std::endl;
				stopExec = true;
				return;
			}
			visitedPos.insert(newPos);
		}
	}
}

int main()
{
	std::ifstream file("input.txt");
	if (file.fail())
	{
		std::cout << "Failed to open input file.\n";
		return 1;
	}

	std::string line = "";
	std::getline(file, line); // There is only 1 line in the input file
	file.close();

	std::vector<std::string> commands;
	ParseWithDelim(line, ", ", commands);

	for (const std::string& cmd : commands)
	{
		ExecuteCommand(cmd);
	}

	if (part1)
	{
		std::cout << "Blocks away: " << abs(xCoord) + abs(yCoord) << std::endl;
	}

	return 0;
}