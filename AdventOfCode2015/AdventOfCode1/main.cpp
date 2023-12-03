#include <iostream>
#include <fstream>
#include <string>

int main()
{
	int _curFloor = 0;
	size_t _firstCharBasement = 0;

	std::ifstream inputstream;
	inputstream.open("input.txt");
	if (inputstream.fail())
	{
		std::cout << "Failed to open file." << std::endl;
	}

	std::string extractedLine;
	while (!inputstream.eof())
	{
		std::getline(inputstream, extractedLine);
	}

	bool _foundBasement = false;
	for (size_t i = 0; i < extractedLine.length(); ++i)
	{
		if (extractedLine[i] == '(')
		{
			++_curFloor;
		}
		else
		{
			--_curFloor;
			if (!_foundBasement) //makes it check less
			{
				if (_curFloor == -1)
				{
					_firstCharBasement = i + 1; //answer is one-based
					_foundBasement = true;
				}
			}
		}
	}
	std::cout << "These instructions take santa to floor " << _curFloor << std::endl;
	std::cout << "Pos of the character that causes Santa to first enter the basement is: " << _firstCharBasement << std::endl;

	system("pause");
	return 0;
}
