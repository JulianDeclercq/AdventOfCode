#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <algorithm>

//forward decl
void ReadFromFile(std::string path);
int RibbonNeeded(int length, int width, int height);
int PaperNeeded(int length, int width, int height);
void GetDim(std::string str, int &length, int &width, int &height);

std::vector<std::string> packagesVec;

int main()
{
	ReadFromFile("input.txt");

	int totalPaperNeeded = 0, totalRibbonNeeded = 0;
	int length = 0, width = 0, height = 0;
	for (std::string s : packagesVec)
	{
		GetDim(s, length, width, height);
		totalPaperNeeded += PaperNeeded(length, width, height);
		totalRibbonNeeded += RibbonNeeded(length, width, height);
	}

	std::cout << "Total square feet of wrapping paper needed is: " << totalPaperNeeded << std::endl;
	std::cout << "Total feet of ribbon needed is: " << totalRibbonNeeded << std::endl;

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

	std::string extractedLine;
	while (!inputstream.eof())
	{
		std::getline(inputstream, extractedLine);
		packagesVec.push_back(extractedLine);
	}
}

void GetDim(std::string str, int &length, int &width, int &height)
{
	size_t firstX = str.find('x');
	size_t lastX = str.rfind('x');

	length = std::stoi(str.substr(0, firstX));
	width = std::stoi(str.substr(firstX + 1, lastX - (firstX - 1)));
	height = std::stoi(str.substr(lastX + 1));
}

int PaperNeeded(int length, int width, int height)
{
	int paperNeeded = 0;
	std::vector<int> sides = { length * width , width * height, height * length };
	std::sort(sides.begin(), sides.end());

	paperNeeded = (2 * sides[0]) + (2 * sides[1]) + (2 * sides[2]) + sides[0]; //sides [0] is the smallest side
	return paperNeeded;
}

int RibbonNeeded(int length, int width, int height)
{
	int ribbonNeeded = 0;
	std::vector<int> dim = { length, width, height };
	std::sort(dim.begin(), dim.end());

	ribbonNeeded = (2 * dim[0]) + (2 * dim[1]);
	ribbonNeeded += length * width * height; //bow
	return ribbonNeeded;
}