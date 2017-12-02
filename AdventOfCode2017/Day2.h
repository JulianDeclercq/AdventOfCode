#include <string>
#include <fstream>
#include <iostream>
#include <vector>
#include <algorithm>
#include <iterator>
#include <sstream>

using namespace std;

namespace Day2
{
	template<typename T>
	void split(const string &s, char delim, T result)
	{
		stringstream ss(s);
		string item;
		while (getline(ss, item, delim))
			*(result++) = stoi(item);
	}

	vector<int> split(const string &s, char delim)
	{
		vector<int> elems;
		split(s, delim, std::back_inserter(elems));
		return elems;
	}

#pragma region Part1
	vector<string> Rows = vector<string>();

	void ParseInput()
	{
		std::ifstream input("Input/Day2.txt");
		//ifstream input("Example/Day2.txt");
		if (input.fail())
		{
			std::cout << "Failed to open input file.\n";
			return;
		}

		string line;
		while (getline(input, line))
			Rows.push_back(line);
	}

	int RowDifference(const string& row)
	{
		// Parse the row string to a vector of integers
		vector<int> numbers = split(row, ' ');
		int maxElement = *max_element(numbers.begin(), numbers.end());
		int minElement = *min_element(numbers.begin(), numbers.end());
		return maxElement - minElement;
	}

	void Part1()
	{
		ParseInput();

		int checksum = 0;
		for (const std::string& row : Rows)
			checksum += RowDifference(row);

		std::cout << "Day 2 Part 1 answer is: " << checksum << std::endl;
	}
#pragma endregion

#pragma region Part2
	void Part2()
	{
		std::cout << "Day 2 Part 2 answer is: " << std::endl;
	}
#pragma endregion
}