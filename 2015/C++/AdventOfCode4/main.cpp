#include <iostream>
#include <string>
#include <fstream>
#include <vector>

std::vector<std::string> strings;

//I used this to output text files. I would then use cmd with File Checksum Integrity Verifier to test the MD5 hashes of these files. 
//I did it this way because I didn't find a way to generate the hash of a string, but I did find a way for files.

int main()
{
	std::string key = "ckczppom";
	size_t startAt = 3000000;
	size_t stopAt = 4000000;

	std::ofstream outputFile;
	for (size_t i = startAt; i < stopAt; ++i)
	{
		outputFile.open("C:/hashes3/" + std::to_string(i) + ".txt");
		if (outputFile.fail())
		{
			std::cout << "Failed to open the outputfile nr " << i << std::endl;
			break;
		}
		outputFile << key.c_str() << std::to_string(i).c_str();
		outputFile.close();
		if (i % 10000 == 0) std::cout << "File nr " << i << " out of " << stopAt << " has been written." << std::endl;
	}

	system("pause");
	return 0;
}
