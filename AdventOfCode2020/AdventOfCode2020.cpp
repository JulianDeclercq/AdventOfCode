#include <iostream>
#include <chrono>
#include "Day1.h"

using namespace std;
using namespace chrono;

int main()
{
    auto day = Day1();

    auto start = high_resolution_clock::now();
    cout << "Solution to day 1, part one: " << day.PartOne();
    auto finish = high_resolution_clock::now();
    cout << "\t| " << duration_cast<microseconds>(finish - start).count() / 1000000.0 
         << "s (" << duration_cast<milliseconds>(finish - start).count() << "ms) |" << endl;

    auto start1 = high_resolution_clock::now(); // different variable so part 1 can be tested individually if part 1 is commented
    cout << "Solution to day 1, part two: " << day.PartTwo();
    auto finish1 = high_resolution_clock::now();
    cout << "\t| " << duration_cast<microseconds>(finish1 - start1).count() / 1000000.0
         << "s (" << duration_cast<milliseconds>(finish1 - start1).count() << "ms) |" << endl;
}
