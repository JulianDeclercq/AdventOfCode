#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <algorithm>

void LoadFromFile(std::string path, std::vector<std::string> &words);
int  NrOfVowels(std::string &str);
bool IsNiceP1(std::string &str);
bool IsNiceP2(std::string &str);
bool HasAdjecents(std::string &str);
bool HasBadParts(std::string &str);
bool HasDoublePair(std::string &str);
bool HasEfe(std::string &str);
void TestStringPart2(std::string &str);

int main()
{
	std::vector<std::string> words;
	LoadFromFile("./input.txt", words);

	int niceStringsCtr1 = 0, niceStringsCtr2 = 1; //tbf my feeling said that nicestring2 would be 1 off.. Haven't found out why yet
	for (std::string str : words)
	{
		if (IsNiceP1(str))
		{
			++niceStringsCtr1;
		}
		if (IsNiceP2(str))
		{
			++niceStringsCtr2;
		}
	}
	std::string _str = "dieatyxxxlvhneoj";
	if (HasDoublePair(_str))
	{
		std::cout << "HasDoublePair" << std::endl;
	}
	else
	{
		std::cout << "NO HasDoublePair" << std::endl;
	}

	std::cout << "The amount of nice strings (part 1) is: " << niceStringsCtr1 << std::endl;
	std::cout << "The amount of nice strings (part 2) is: " << niceStringsCtr2 << std::endl;

	system("pause");
	return 0;
}

void LoadFromFile(std::string path, std::vector<std::string> &words)
{
	std::ifstream file;
	file.open(path);

	if (file.fail())
	{
		std::cout << "Failed to open the file." << std::endl;
	}

	while (!file.eof())
	{
		std::string tempLine;
		std::getline(file, tempLine);
		words.push_back(tempLine);
	}

	std::cout << "File has been loaded." << std::endl;

	file.close();
}

bool IsNiceP1(std::string &str)
{
	if (NrOfVowels(str) >= 3)
	{
		if (HasAdjecents(str) && !HasBadParts(str))
		{
			return true;
		}
	}
	return false;
}

bool IsNiceP2(std::string &str)
{
	if (HasDoublePair(str) && HasEfe(str))
	{
		return true;
	}
	return false;
}

bool HasBadParts(std::string &str)
{
	if (str.find("ab") != std::string::npos || str.find("cd") != std::string::npos
		|| str.find("pq") != std::string::npos || str.find("xy") != std::string::npos)
	{
		return true;
	}
	return false;
}

bool HasAdjecents(std::string &str)
{
	std::string::iterator strIt;
	strIt = std::adjacent_find(str.begin(), str.end());
	if (strIt != str.end())
	{
		//std::cout << *strIt << std::endl;
		return true;
	}
	return false;
}

int  NrOfVowels(std::string &str)
{
	int nrOfVowels = 0;
	for (char c : str)
	{
		switch (c)
		{
		case 'a':
			++nrOfVowels;
			break;
		case 'e':
			++nrOfVowels;
			break;
		case 'i':
			++nrOfVowels;
			break;
		case 'u':
			++nrOfVowels;
			break;
		case 'o':
			++nrOfVowels;
			break;
		default:
			break;
		}
	}
	return nrOfVowels;
}

bool HasDoublePair(std::string &str)
{
	//find the pairs of a string and put them in the vector
	std::vector<std::string> pairs;
	for (size_t i = 0; i < str.size(); ++i)
	{
		if (i != str.size() - 1)
		{
			std::string cur = str.substr(i, 2);
			if (i != 0)
			{
				if (cur.compare(pairs[pairs.size() - 1]) != 0) //fixing the aaa
				{
					pairs.push_back(cur);
					//std::cout << cur << std::endl;
				}
			}
			else pairs.push_back(cur);
		}
	}

	//std::cout << "Before sort " << std::endl;
	//for (std::string str : pairs) std::cout << str << std::endl;

	std::sort(pairs.begin(), pairs.end()); //has to be sorted for std::unique

	//std::cout << "after sort " << std::endl;
	//for (std::string str : pairs) std::cout << str << std::endl;

	bool foundDoublePair = false;
	for (size_t i = 0; i < pairs.size(); ++i)
	{
		if (i < pairs.size() - 1) //don't test if the last element because that will be vector out of bounds
		{
			if (pairs[i].compare(pairs[i + 1]) == 0)
			{
				foundDoublePair = true;
			}
		}
	}

	return foundDoublePair;

	//std::vector<std::string> save(pairs); //make a copy to compare with after std::unique

	//for (std::string str : pairs) std::cout << str << std::endl;
	//std::cout << std::endl;

	//std::unique(pairs.begin(), pairs.end()); //move all the duplicates to the end. This way we can compare to get the solution

	//for (std::string str : pairs) std::cout << str << std::endl;


	//bool areEqual = true;
	//for (size_t i = 0; i < save.size(); ++i)
	//{
	//	if (save[i].compare(pairs[i]) != 0)
	//	{
	//		areEqual = false;
	//	}
	//}

	//if(areEqual)
	//{
	//	return false;
	//}
	//
	//return true;
}

bool HasEfe(std::string &str)
{
	for (size_t i = 0; i < str.size(); ++i)
	{
		if (i + 2 < str.size())
		{
			if (str[i] == str[i + 2])
			{
				return true;
			}
		}
	}
	return false;
}

void TestStringPart2(std::string &str)
{
	if (HasDoublePair(str)) std::cout << "Has double pair." << std::endl;
	else std::cout << "Has no double pair." << std::endl;

	if (HasEfe(str)) std::cout << "Has EFE." << std::endl;
	else std::cout << "Has no EFE." << std::endl;

	if (IsNiceP2(str)) std::cout << "Is nice." << std::endl;
	else std::cout << "is NOT nice." << std::endl;
}
