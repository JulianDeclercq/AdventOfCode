#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <algorithm>
#include "include/rapidjson/document.h"
#include "include/rapidjson/writer.h"
#include "include/rapidjson/stringbuffer.h"

int FindAndCountNrs(std::string &str);
std::string FilterGroupsToString(std::string &inputStr);
void JsonMethod();

int main()
{
	std::ifstream input("./input.txt");
	if (input.fail())
	{
		std::cout << "Failed to open the input file." << std::endl;
	}

	std::string inputStr;
	std::getline(input, inputStr);
	std::string inputStrOrig(inputStr); //copy constructor

	int result = 0;
	result += FindAndCountNrs(inputStr);
	std::cout << "Answer is: " << result << std::endl;

	JsonMethod();

	/*std::string myStr = "59str63nkjh";
	std::cout << "Find and count numbers for string :" << myStr << std::endl;
	std::cout << FindAndCountNrs(myStr) << std::endl;*/

	system("pause");
	return 0;
}

int FindAndCountNrs(std::string &str) //PART 1 logic. For part 2: this calculates the value of the string without any groups.
{
	std::vector<int> values;
	bool isNegative = false;
	int nrDigits = 1;
	size_t numberIdx = 0;

	for (;;) //watch out for the newly filtered string
	{
		numberIdx = str.find_first_of("0123456789");
		if (numberIdx != str.npos)
		{
			isNegative = false;
			nrDigits = 1;

			if (numberIdx > 0)
			{
				if (str[numberIdx - 1] == '-')
				{
					isNegative = true;
				}
			}
			if (str.substr(numberIdx + 1).find_first_of("0123456789") == 0)
			{
				++nrDigits;
				if (str.substr(numberIdx + 2).find_first_of("0123456789") == 0)
				{
					++nrDigits;
				}
			}
			if (isNegative)
			{
				values.push_back(-std::stoi(str.substr(numberIdx, nrDigits)));
			}
			else
			{
				values.push_back(std::stoi(str.substr(numberIdx, nrDigits)));
			}

			//changing the loop's "input"
			str = str.substr(numberIdx + nrDigits);
		}
		else
		{
			std::cout << "Found last nr of the file." << std::endl;
			break;
		}
	}

	int result = 0;
	for (int i : values)
	{
		result += i;
	}
	return result;
}

//Try without JSON parser
std::string FilterGroupsToString(std::string &inputStr)
{
	std::string result;
	std::vector<std::string> groups;

	std::ofstream output("./output.txt");

	//Check if it contains red
	std::string red = "red";
	size_t redIdx = 0, openBrace = 0, closedBrace = 0;

	int ctr = 0;
	for (;;)
	{
		if (inputStr.find(red) != inputStr.npos)
		{
			++ctr;
			redIdx = inputStr.find(red);
			if (inputStr.substr(0, redIdx).rfind('{') != inputStr.npos)
			{
				openBrace = inputStr.substr(0, redIdx).rfind('{');
				output << "Open brace found at index: " << openBrace << std::endl;
				closedBrace = inputStr.substr(openBrace).find('}');
				output << "Matching closed brace found at index: " << closedBrace + openBrace << " (OpenBrace + " << closedBrace << ")" << std::endl;

				if (redIdx < closedBrace + openBrace)
				{
					if (((inputStr.substr(0, redIdx).rfind('[') > openBrace) && (inputStr.substr(openBrace).find(']') < closedBrace)) == false) //if red is in an array inside an object, do not delete the object
					{
						output << "Input string before erase: \n" << inputStr << "\n\n";
						output << "Content to erase: \n" << inputStr.substr(openBrace, closedBrace + 1) << "\n\n";
						inputStr.erase(openBrace, closedBrace + 1);
						output << "Input string after erase: \n" << inputStr << "\n\n";
					}
					else
					{
						output << "\"red\" found inside an array, erased it to continue the loop. \n" << std::endl;
						inputStr.erase(redIdx, 3);
					}
				}
				else
				{
					output << "Non-surrounded \"red\" found, erased it to continue the loop. \n" << std::endl;
					inputStr.erase(redIdx, 3);
				}
			}
			else
			{
				break;
			}
		}
		else
		{
			break;
		}
	}
	std::cout << ctr << " out of 411 \"reds\" have been found." << std::endl; //411
	return result;
}

//Try with JSON parser
void JsonMethod()
{
	//Simple JSON Example
	/*// 1. Parse a JSON string into DOM.
	const char* _json = "{\"project\":\"rapidjson\",\"stars\":10}";
	rapidjson::Document _doc;
	_doc.Parse(_json);

	// 2. Modify it by DOM.
	rapidjson::Value& s = _doc["stars"];
	s.SetInt(s.GetInt() + 1);

	// 3. Stringify the DOM
	rapidjson::StringBuffer buffer;
	rapidjson::Writer<rapidjson::StringBuffer> _writer(buffer);
	_doc.Accept(_writer);

	// Output {"project":"rapidjson","stars":11}
	std::cout << buffer.GetString() << std::endl;*/

	//Implementation
	// 1. Parse a JSON string into DOM. These are test strings that all work
	//const char* _json = inputStr.c_str();
	//const char* _json = "{\"project\":\"rapidjson\",\"stars\":10,\"tetn\":15 }";
	//const char* _json = "{\"hello\": \"world\",\"t\": true ,\"f\": false,\"n\": null,\"i\": 123,\"pi\": 3.1416,\"a\": [1, 2, 3, 4]}";
	std::string myStr;
	std::ifstream input("./input.txt");
	if (input.fail()) std::cout << "Failed to open the input file." << std::endl;
	std::getline(input, myStr);
	const char* _json = myStr.c_str();
	rapidjson::Document _doc;
	_doc.Parse(_json);

	int _result = 0;
	static const char* kTypeNames[] = { "Null", "False", "True", "Object", "Array", "String", "Number" };

	if (_doc.IsObject())
	{
		std::cout << "Document is an object.\n";
		for (rapidjson::Value::ConstMemberIterator itr = _doc.MemberBegin(); itr != _doc.MemberEnd(); ++itr)
		{
			printf("Type of member %s is %s\n", itr->name.GetString(), kTypeNames[itr->value.GetType()]);

			if (std::string(kTypeNames[itr->value.GetType()]).compare("Object") == 0) //using std::string to avoid warning
			{
				for (rapidjson::Value::ConstMemberIterator it = itr->value.MemberBegin(); it != itr->value.MemberEnd(); ++it)
				{
					printf("\tType of member %s is %s\n", it->name.GetString(), kTypeNames[it->value.GetType()]);
				}
			}
			else if (std::string(kTypeNames[itr->value.GetType()]).compare("Number") == 0) //using std::string to avoid warning
			{
				if (itr->value.IsNumber())
				{
					if (!itr->value.IsDouble())
					{
						_result += itr->value.GetInt();
					}
				}
				else std::cout << "It is not a number.\n";
			}
		}
	}
	else if (_doc.IsArray())
	{
		std::cout << "Document is an array.\n";
		for (rapidjson::Value::ConstValueIterator _mainIt = _doc.Begin(); _mainIt != _doc.End(); ++_mainIt)
		{
			if (_mainIt->IsObject())
			{
				std::cout << "%---------------------------------------------%\n";
				for (rapidjson::Value::ConstMemberIterator itr = _mainIt->MemberBegin(); itr != _mainIt->MemberEnd(); ++itr)
				{
					printf("Type of member %s is %s\n", itr->name.GetString(), kTypeNames[itr->value.GetType()]);
				}
				std::cout << "%---------------------------------------------%\n";
			}
			else if (_mainIt->IsArray())
			{
				for (rapidjson::Value::ConstValueIterator _localIt = _mainIt->Begin(); _localIt != _mainIt->End(); ++_localIt)
				{
				}
				std::cout << "Found a new array.\n";
			}
			else
			{
				std::cout << "Found currently unknown type.\n";
			}
		}
	}
	else
	{
		std::cout << "Document is not an array nor an object.\n";
	}

	std::cout << "The result is: " << _result << std::endl;
}

