#pragma once
#include <iostream>
#include <string>
#include <fstream>
#include <vector>
#include <cmath>

using namespace std;

class Day1
{
	private:
		std::vector<int> _masses = std::vector<int>();

		void parse_input();
		int fuel_required(int mass);
		int fuel_required_part2(int massOrFuel);

	public:
		// constructor
		Day1() {};

		void part1();
		void part2();
};
