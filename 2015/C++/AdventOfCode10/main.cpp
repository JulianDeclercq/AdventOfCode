#include <iostream>
#include <string>
#include <vector>
#include <fstream>

void GenerateNextSequence(std::string &str, std::ofstream &output);

int main()
{
	std::ofstream output("./output.txt");
	if (output.fail())
	{
		std::cout << "Failed to open outputfile." << std::endl;
	}

	std::string input = "1113222113";

	for (int i = 0; i < 50; ++i)
	{
		GenerateNextSequence(input, output);
		std::cout << "Iteration: " << i << std::endl;
	}
	std::cout << "The solution is: " << input.length() << std::endl;

	system("pause");
	return 0;
}

void GenerateNextSequence(std::string &str, std::ofstream &output)//For each step, take the previous value, and replace each run of digits (like 111) with the number of digits (3) followed by the digit itself (1).
{
	char start = str[0];
	std::vector<std::string> sequences;

	size_t newSeq = str.find_first_not_of(start);
	if (newSeq != str.npos)
	{
		start = str[newSeq];
		sequences.push_back(str.substr(0, newSeq));
	}
	else
	{
		sequences.push_back(str);
	}

	while (newSeq < str.size())
	{
		str = str.substr(newSeq);
		newSeq = str.find_first_not_of(start);
		if (newSeq == str.npos)
		{
			sequences.push_back(str);
			break;
		}
		start = str[newSeq];

		sequences.push_back(str.substr(0, newSeq));
	}

	//replace each run of digits (like 111) with the number of digits (3) followed by the digit itself (1)
	for (size_t i = 0; i < sequences.size(); ++i)
	{
		sequences[i] = std::to_string(sequences[i].size()) + sequences[i][0];
	}

	str.clear();
	for (std::string seq : sequences)
	{
		str += seq;
	}
	output << "Result: " << str << std::endl;
}
