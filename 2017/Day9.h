#pragma once
#include <vector>
#include <iostream>
#include <string>
#include <algorithm>
#include <fstream>

using namespace std;

class CharacterGroup
{
public:
	CharacterGroup(size_t startIdx, size_t endIdx) : StartIdx(startIdx), EndIdx(endIdx), Score(1), Children(vector<CharacterGroup*>())
	{
	}

	size_t StartIdx;
	size_t EndIdx;
	int Score;
	vector<CharacterGroup*> Children;

	void CalculateScores()
	{
		// Loop through the children
		for (CharacterGroup* child : Children)
		{
			// Increment the score
			child->Score = Score + 1;

			// Calculate the score for the child
			child->CalculateScores();
		}
	}
};

class Day9
{
private:
	string ParseInput();
	void RemoveIgnores(string& str);
	void RemoveJunk(string& str);
	void ParseGroups(string& input);

	vector<CharacterGroup*> _groups = vector<CharacterGroup*>();
	int _nonCanceledGarbageCharacters = 0;

public:
	void Part1();
	void Part2();
};