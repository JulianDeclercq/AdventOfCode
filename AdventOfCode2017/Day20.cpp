#include "Day20.h"

void Day20::Part1()
{
	ifstream input("Input/Day20.txt");
	//ifstream input("Example/Day20Part1.txt");

	if (input.fail())
	{
		cout << "Failed to open inputfile." << endl;
		return;
	}

	string line;
	smatch match;
	vector<Particle> particles = vector < Particle>();
	particles.reserve(1000);

	// Parse the input
	int loopCtr = 0;
	while (getline(input, line))
	{
		if (!regex_match(line, match, regex(R"(p=<(-?\d+),(-?\d+),(-?\d+)>, v=<(-?\d+),(-?\d+),(-?\d+)>. a=<(-?\d+),(-?\d+),(-?\d+)>)")))
		{
			cout << "Line " << line << " didn't match regex." << endl;
			continue;
		}

		// Parse a new particle and add it to the vector
		particles.push_back(Particle(loopCtr,									// id
			Helpers::Point3D(stoi(match[1]), stoi(match[2]), stoi(match[3])),	// position
			Helpers::Point3D(stoi(match[4]), stoi(match[5]), stoi(match[6])),	// velocity
			Helpers::Point3D(stoi(match[7]), stoi(match[8]), stoi(match[9])))); // acceleration

		// Increment the loop counter
		++loopCtr;
	}

	// Sort works for this solution because of the overloaded < operator of the Particle class
	sort(particles.begin(), particles.end());

	cout << "Day 20 Part 1 answer: " << particles[0].ID << endl;
}