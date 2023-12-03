#include <iostream>
#include <string>
#include <algorithm>

bool ContainsStraightThree(const std::string &str);
bool ContainsIOL(const std::string &str);
bool ContainsTwoPairs(const std::string &str);
bool IsValidPassword(const std::string &str);
void GenerateNextPassword(std::string &current);

int main()
{
	std::string result = "hxbxwxba";
	while (!IsValidPassword(result))
	{
		GenerateNextPassword(result);
	}

	std::cout << "Found solution part 1! " << result << std::endl;

	//2nd part
	GenerateNextPassword(result);
	while (!IsValidPassword(result))
	{
		GenerateNextPassword(result);
	}
	std::cout << "Found solution part 2! " << result << std::endl;

	
	system("pause");
	return 0;
}

bool ContainsStraightThree(const std::string &str)
{
	for (size_t i = 0; i < str.size() - 2; ++i)
	{
		if (str[i + 1] - str[i] == 1)
		{
			if (str[i + 2] - str[i + 1] == 1)
			{
				return true;
			}
		}
	}
	return false;
}

bool ContainsIOL(const std::string &str)
{
	for (char c : str)
	{
		if (c == 'i' || c == 'o' || c == 'l')
		{
			return true;
		}
	}
	return false;
}

bool ContainsTwoPairs(const std::string &str)
{
	bool foundOne = false;

	std::string::const_iterator strIt, strIt2;
	strIt = std::adjacent_find(str.begin(), str.end());
	if (strIt != str.end())
	{
		foundOne = true;
	}

	if (foundOne)
	{
		const std::string substring = str.substr(strIt - str.begin() + 2);
		strIt2 = std::adjacent_find(substring.begin(), substring.end());
		if (strIt2 != substring.end() && *strIt2 != *strIt)
		{
			return true;
		}
	}
	return false;
}

bool IsValidPassword(const std::string &str)
{
	if (ContainsStraightThree(str) && !ContainsIOL(str) && ContainsTwoPairs(str))
	{
		return true;
	}
	else
	{
		return false;
	}
}

void GenerateNextPassword(std::string &current)
{
	if (current[current.size() - 1] < 'z')
	{
		++current[current.size() - 1];
	}
	else
	{
		current[current.size() - 1] = 'a';
		if (current[current.size() - 2] < 'z')
		{
			++current[current.size() - 2];
		}
		else
		{
			current[current.size() - 2] = 'a';
			if (current[current.size() - 3] < 'z')
			{
				++current[current.size() - 3];
			}
			else
			{
				current[current.size() - 3] = 'a';
				if (current[current.size() - 4] < 'z')
				{
					++current[current.size() - 4];
				}
				else
				{
					current[current.size() - 4] = 'a';
					if (current[current.size() - 5] < 'z')
					{
						++current[current.size() - 5];
					}
					else
					{
						current[current.size() - 5] = 'a';
						if (current[current.size() - 6] < 'z')
						{
							++current[current.size() - 6];
						}
						else
						{
							current[current.size() - 6] = 'a';
							if (current[current.size() - 7] < 'z')
							{
								++current[current.size() - 7];
							}
							else
							{
								current[current.size() - 7] = 'a';
								if (current[current.size() - 8] < 'z')
								{
									++current[current.size() - 8];
								}
							}
						}
					}
				}
			}
		}
	}
}  //definitely not the best way to do it, but it works and it is 1 am (did this right before bedtime to relax).

