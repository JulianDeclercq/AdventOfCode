#pragma once
#include <iostream>
#include <string>
#include <vector>
#include <fstream>
#include <regex>
#include <algorithm>
#include <map>
#include <functional>
#include <numeric>
#include "Helpers.h"

using namespace std;

struct Program
{
public:
	// Member variables
	string Name;
	vector<Program*> Children;
	int Weight;
	int TowerWeight;

	// Constructors
	Program(string name, int weight, vector<Program*> children) : Name(name), Weight(weight), Children(children), TowerWeight(-1)
	{
	}

	// Methods
	int CalculateTowerWeight()
	{
		// Check if this weight has been calculated before, if so return it
		if (TowerWeight != -1)
			return TowerWeight;

		// If this is a leaf node, return the weight
		if (Children.empty())
			return Weight;

		// Calculate the weight using recursion
		int sum = 0;
		for (Program* child : Children)
			sum += child->CalculateTowerWeight();

		// Add the own weight to it
		sum += Weight;

		// Cache the calculated tower weight
		TowerWeight = sum;

		// Return it
		return TowerWeight;
	}

	bool IsBalanced()
	{
		// Create an empty int vector
		vector<int> towerWeights = vector<int>();

		// All the children's sums must match
		for (Program* child : Children)
			towerWeights.push_back(child->CalculateTowerWeight());

		// Return if all elements are equal to each other or not
		return (std::adjacent_find(towerWeights.begin(), towerWeights.end(), std::not_equal_to<int>()) == towerWeights.end());
	}
};

class Day7
{
private:
	map<string, Program*> _tree = map<string, Program*>();

	void ParseTree();
	Program* FindRoot();

	int FindWeightCorrection(Program* wrongWeightProgram);

public:
	void Part1();
	void Part2();
};
