#include "Day17.h"

void Day17::ParseInput()
{
	//ifstream input("input/day17example.txt");
	ifstream input("input/day17.txt");

	if (!input)
	{
		cout << "Failed to open input." << endl;
		return;
	}

	string line = "";
	int y = 0;
	while (getline(input, line))
	{
		for (size_t x = 0; x < line.size(); ++x)
		{
			_grid.insert({ Cube(x, y, 0), line[x] == '#' });
			_grid4D.insert({ Cube4D(x, y, 0, 0), line[x] == '#' });
		}

		++y;
	}
}

int Day17::ActiveNeighbours(const Cube& c, const grid& grid, set<Cube>* newlyAdded)
{
	// no bounds check since it's an infinite grid
	int active = 0;
	for (int x : {c.X - 1, c.X, c.X + 1})
	{
		for (int y : {c.Y - 1, c.Y, c.Y + 1})
		{
			for (int z : {c.Z - 1, c.Z, c.Z + 1})
			{
				// ignore self
				if (c.X == x && c.Y == y && c.Z == z)
					continue;

				// current neighbour
				const auto neighbour = Cube(x, y, z);

				// if the neighbour didn't exist, add it to the newlyAdded vector
				if (grid.find(neighbour) == grid.end())
				{
					if (newlyAdded != nullptr)
						newlyAdded->insert(neighbour);

					continue;
				}

				// if this neighbour is active, count it
				if (grid.at(neighbour))
				{
					active++;

					// it is never relevant to keep searching after this point
					if (active >= 4)
						return active;
				}
			}
		}
	}
	return active;
}

void Day17::ConwayStep()
{
	// work on a copy to avoid wrong checks midway changing
	auto originalGrid(_grid);
	set<Cube> newlyAddedNeighbours;
	
	for (const auto& cube : originalGrid)
	{
		int an = ActiveNeighbours(cube.first, originalGrid, &newlyAddedNeighbours);
		TransformCube(cube, an);
	}

	for (const auto& newlyAdded : newlyAddedNeighbours)
	{
		// add it to the grid
		_grid.insert({ newlyAdded, false });

		int an = ActiveNeighbours(newlyAdded, originalGrid, nullptr);
		TransformCube({ newlyAdded, false }, an);
	}
}

void Day17::TransformCube(const pair<Cube, bool>& cube, int activeNeighbours)
{
	if (cube.second) // active
	{
		if (activeNeighbours != 2 && activeNeighbours != 3)
			_grid[cube.first] = false;
	}
	else // inactive
	{
		if (activeNeighbours == 3)
			_grid[cube.first] = true;
	}
}

void Day17::ConwayStep4D()
{
	auto originalGrid(_grid4D);
	set<Cube4D> newlyAddedNeighbours;

	for (const auto& cube : originalGrid)
	{
		int an = ActiveNeighbours4D(cube.first, originalGrid, &newlyAddedNeighbours);
		TransformCube4D(cube, an);
	}

	for (const auto& newlyAdded : newlyAddedNeighbours)
	{
		// add it to the grid
		_grid4D.insert({ newlyAdded, false });

		int an = ActiveNeighbours4D(newlyAdded, originalGrid, nullptr);
		TransformCube4D({ newlyAdded, false }, an);
	}
}

int Day17::ActiveNeighbours4D(const Cube4D& c, const grid4D& grid, set<Cube4D>* newlyAdded)
{
	// no bounds check since it's an infinite grid
	int active = 0;
	for (int x : {c.X - 1, c.X, c.X + 1})
	{
		for (int y : {c.Y - 1, c.Y, c.Y + 1})
		{
			for (int z : {c.Z - 1, c.Z, c.Z + 1})
			{
				for (int w : {c.W - 1, c.W, c.W + 1})
				{
					// ignore self
					if (c.X == x && c.Y == y && c.Z == z && c.W == w)
						continue;

					// current neighbour
					const auto neighbour = Cube4D(x, y, z, w);

					// if the neighbour didn't exist, add it to the newlyAdded vector
					if (grid.find(neighbour) == grid.end())
					{
						if (newlyAdded != nullptr)
						{
							newlyAdded->insert(neighbour);
						}
						else _grid4D.insert({ neighbour, false }); // this was the PATCH for part2

						continue;
					}

					// if this neighbour is active, count it
					if (grid.at(neighbour))
					{
						active++;

						// it is never relevant to keep searching after this point
						if (active >= 4)
							return active;
					}
				}
			}
		}
	}
	return active;
}

void Day17::TransformCube4D(const pair<Cube4D, bool>& cube, int activeNeighbours)
{
	if (cube.second) // active
	{
		if (activeNeighbours != 2 && activeNeighbours != 3)
			_grid4D[cube.first] = false;
	}
	else // inactive
	{
		if (activeNeighbours == 3)
			_grid4D[cube.first] = true;
	}
}

int Day17::PartOne()
{
	int cycles = 6;
	for (int i = 0; i < cycles; ++i)
		ConwayStep();

	return count_if(_grid.begin(), _grid.end(), [](const auto& p) {return p.second;});
}

int Day17::PartTwo()
{
	int cycles = 6;
	for (int i = 0; i < cycles; ++i)
		ConwayStep4D();

	return count_if(_grid4D.begin(), _grid4D.end(), [](const auto& p) {return p.second; });
}
