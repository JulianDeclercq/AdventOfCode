#include "Day14.h"

string Day14::HexToBinary(const std::string& hex)
{
	string bin;
	for_each(hex.begin(), hex.end(), [&](const char c) {bin.append(_hexLookup.at(toupper(c))); });
	return bin;
}

void Day14::AssignGroupToCell(vector<Cell>& cells, Cell& cell, int currentGroup)
{
	// If this cell is not visible or has already been assigned a group, leave the method
	if (!cell.Visible || cell.GroupChecked)
		return;

	// If this cell is visible, mark it with the current group number
	cell.Group = currentGroup;

	// Mark the cell as checked
	cell.GroupChecked = true;

	// Check all neighbors (excluding diagonals)
	// Left neighbor
	if (cell.Index % _width != 0) // check if left neighbor exists
		AssignGroupToCell(cells, cells[cell.Index - 1], currentGroup);

	// Upper neighbor
	if (cell.Index >= _width) // check if upper neighbor exists
		AssignGroupToCell(cells, cells[cell.Index - _width], currentGroup);

	// Right neighbor
	if ((cell.Index % _width) != 1) // check if right neighbor exists
		AssignGroupToCell(cells, cells[cell.Index + 1], currentGroup);

	// Lower neighbor
	if (cell.Index < _width * (_width - 1)) // check if lower neighbor exists
		AssignGroupToCell(cells, cells[cell.Index + _width], currentGroup);
}

void Day14::Part1()
{
	//const string input = "flqrgnkx";
	const string input = "ljoxqyyw";

	int used = 0;
	for (int i = 0; i < 128; ++i)
	{
		string hash = Day10().KnotHash(input + '-' + to_string(i));
		string binary = HexToBinary(hash);
		used += count_if(binary.begin(), binary.end(), [](const char c) {return c == '1'; });
	}

	cout << "Day 14 Part 1 answer: " << used << endl;
}

void Day14::Part2()
{
	vector <Cell> cells{};
	cells.reserve(_width * _width);

	//const string input = "flqrgnkx";
	const string input = "ljoxqyyw";

	// Fill the cells
	size_t idxCtr = 0;
	for (size_t i = 0; i < _width; ++i)
	{
		string hash = Day10().KnotHash(input + '-' + to_string(i));
		string binary = HexToBinary(hash);
		for (const char c : binary)
		{
			cells.push_back(Cell(c == '1', idxCtr));
			++idxCtr;
		}
	}

	// Assign groups
	int currentGroup = 1;
	for (Cell& cell : cells)
	{
		// Ignore extra calls for cells that are not visible or already checked even if they would cancel their method immediately at start of it
		if (!cell.Visible || cell.GroupChecked)
			continue;

		AssignGroupToCell(cells, cell, currentGroup);
		++currentGroup;
	}

	cout << "Day 14 Part 2 answer: " << currentGroup << endl;
}