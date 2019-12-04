#include "Day1.h"

void Day1::parse_input()
{
	ifstream input("Input/day1.txt");

	if (!input)
	{
		cout << "Failed to open input." << endl;
		return;
	}

	string line = "";
	while (getline(input, line))
		_masses.push_back(stoi(line));

	cout << "test " << fuel_required(100756) << endl;
}

int Day1::fuel_required(int mass)
{
	return int(floor(mass / 3.0) - 2.0);
}

int Day1::fuel_required_part2(int massOrFuel)
{
	// calculate the fuel required for this mass or fuel
	int fuelNeeded = fuel_required(massOrFuel);

	// if fuel needed for this fuel is negative, recursion stops
	if (fuelNeeded <= 0)
		return 0;

	// calculate the fuel needed for this mass/fuel and add it to the total
	//cout << "needed " << fuelNeeded << " fuel" << endl;
	return (fuelNeeded + fuel_required_part2(fuelNeeded));
}

void Day1::part1()
{
	cout << "Day 1 part 1:" << endl;

	// read the input
	parse_input();

	int answer = 0;
	for (int mass : _masses)
		answer += fuel_required(mass);

	cout << "The answer to day 1 part 1 is: " << answer << endl;
}

void Day1::part2()
{
	cout << "Day 1 part 2:" << endl;

	// read the input
	parse_input();

	int answer = 0;
	for (int mass : _masses)
		answer += fuel_required_part2(mass);

	cout << "The answer to day 1 part 2 is: " << answer << endl;
}
