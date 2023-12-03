#include "Day20.h"

void Day20::ParseInput(vector<Particle>& particles)
{
	ifstream input("Input/Day20.txt");
	//ifstream input("Example/Day20Part2.txt");

	if (input.fail())
	{
		cout << "Failed to open inputfile." << endl;
		return;
	}

	string line;
	smatch match;

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
}

void Day20::Part1()
{
	// Create and reserve space a vector of particles
	vector<Particle> particles = vector <Particle>();
	particles.reserve(1000);

	// Parse the input
	ParseInput(particles);

	// Sort works for this solution because of the overloaded < operator of the Particle class
	sort(particles.begin(), particles.end());

	cout << "Day 20 Part 1 answer: " << particles[0].ID << endl;
}

void Day20::Part2()
{
	// Create and reserve space a vector of particles
	vector<Particle> particles = vector <Particle>();
	particles.reserve(1000);

	// Parse the input
	ParseInput(particles);

	// Use an arbitrary number of iterations that deems high enough
	// Ideally this should be calculated rather than trial and error
	for (int i = 0; i < 100; ++i)
	{
		// Update the particles
		for (Particle& particle : particles)
			particle.Update();

		// Create a map that groups particles by their position
		map <Helpers::Point3D, vector<Particle>> particlePositions = map <Helpers::Point3D, vector<Particle>>();
		for (const Particle& particle : particles)
			particlePositions[particle.Position].push_back(particle);

		// Clear all existing particles
		particles.clear();

		// Re-add the non colliding particles
		for (const auto& particleGroup : particlePositions)
		{
			if (particleGroup.second.size() <= 1)
				particles.insert(particles.end(), particleGroup.second.begin(), particleGroup.second.end());
		}
	}

	cout << "Day 20 Part 2 answer: " << particles.size() << endl;
}