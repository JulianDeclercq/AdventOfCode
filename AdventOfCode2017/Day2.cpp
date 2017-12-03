#include "Day2.h"

std::vector<int> Day2::split(const string &s, char delim)
{
	vector<int> elems;
	split(s, delim, std::back_inserter(elems));
	return elems;
}

void Day2::ParseInput()
{
	//ifstream input("Example/Day2Part1.txt");
	//ifstream input("Example/Day2Part2.txt");
	ifstream input("Input/Day2.txt");
	if (input.fail())
	{
		std::cout << "Failed to open input file.\n";
		return;
	}

	string line;
	while (getline(input, line))
		Rows.push_back(line);
}

int Day2::RowDifference(const string& row)
{
	// Parse the row string to a vector of integers
	vector<int> numbers = split(row, ' ');
	int maxElement = *max_element(numbers.begin(), numbers.end());
	int minElement = *min_element(numbers.begin(), numbers.end());
	return maxElement - minElement;
}

void Day2::Part1()
{
	ParseInput();

	int checksum = 0;
	for (const std::string& row : Rows)
		checksum += RowDifference(row);

	std::cout << "Day 2 Part 1 answer is: " << checksum << std::endl;
}

int Day2::EvenlyDivided(const string& row)
{
	/*		Find the only two numbers in each row where one evenly divides the other
	so where the result of the division operation is a whole number.		*/

	// Parse the row string to a vector of integers
	vector<int> numbers = split(row, ' ');
	for (int number : numbers)
	{
		for (int number2 : numbers)
		{
			// Ignore checking with the same numbers because of nested loop
			// Ignore checking when first number is smaller than second number
			if (number == number2 || number < number2)
				continue;

			float division = static_cast<float>(number) / static_cast<float>(number2);

			// Check if it is a whole number and return it if it is
			if (floor(division) == division)
				return static_cast<int>(division);
		}
	}

	cout << "Error, no numbers where division is a whole number exist.\n";
	return -1;
}

void Day2::Part2()
{
	ParseInput();

	int checksum = 0;
	for (const std::string& row : Rows)
		checksum += EvenlyDivided(row);

	std::cout << "Day 2 Part 2 answer is: " << checksum << std::endl;
}