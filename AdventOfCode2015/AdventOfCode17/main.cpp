#include <iostream>
#include <fstream>
#include <vector>
#include <string>
#include <algorithm>

std::vector<int> capacities;
int minimum = 0;

int explore(int i, int sum)
{
	if (i == capacities.size())
	{
		if (sum == 150)
		{
			return 1;
		}
		else
		{
			return 0;
		}
	}
	return explore(i + 1, sum) + explore(i + 1, sum + capacities[i]);
}

int main()
{
	std::ifstream input;
	input.open("input.txt");
	if (input.fail())
	{
		std::cout << "Failed to open inputfile." << std::endl;
	}

	while (!input.eof())
	{
		std::string line;
		std::getline(input, line);
		capacities.push_back(std::stoi(line));
	}

	std::cout << "The answer is: " << explore(0, 0) << std::endl;
	system("pause");
}

