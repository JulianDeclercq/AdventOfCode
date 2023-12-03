#include <iostream>
#include <vector>
#include <string>
#include <fstream>
#include <map>

//forward decl
void ReadFromFile(std::string path);
void FollowPath(std::string path, std::map<std::pair<int, int>, int> &houses);

std::string SantaPath = "", RoboSantaPath = "";

int main()
{
	ReadFromFile("./input.txt");

	std::map<std::pair<int, int>, int> houses;
	houses[{0, 0}] += 2; //Start x2
	int luckyHouses = 0;

	FollowPath(SantaPath, houses);
	FollowPath(RoboSantaPath, houses);

	for (std::pair<std::pair<int, int>, int> housePos : houses)
	{
		if (housePos.second >= 1)
		{
			++luckyHouses;
		}
	}
	std::cout << luckyHouses << std::endl;
	//std::cout << houses.find({ 0, 0 })->second << std::endl;

	system("pause");
	return 0;
}

void ReadFromFile(std::string path)
{
	std::ifstream inputstream;
	inputstream.open(path);
	if (inputstream.fail())
	{
		std::cout << "Failed to open file." << std::endl;
	}

	std::string temp;
	while (!inputstream.eof()) //there should only be 1 line though as there are no enters in the input
	{
		std::getline(inputstream, temp);
	}

	for (size_t i = 0; i < temp.size(); ++i)
	{
		if (i % 2)
		{
			SantaPath += temp[i];
		}
		else
		{
			RoboSantaPath += temp[i];
		}
	}
}

void FollowPath(std::string path, std::map<std::pair<int, int>, int> &houses)
{
	int curRow = 0, curCol = 0;
	for (char direction : path)
	{
		switch (direction)
		{
		case '^':
			--curRow;
			break;
		case 'v':
			++curRow;
			break;
		case '<':
			--curCol;
			break;
		case '>':
			++curCol;
			break;
		default: std::cout << "Hit default." << std::endl;
			break;
		}
		++houses[{curRow, curCol}];
	}
}