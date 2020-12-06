#include <iostream>
#include <chrono>
#include "Day6.h"

using namespace std;
using namespace chrono;

int main()
{
    auto day = Day6();

    auto start = high_resolution_clock::now();
    cout << "Solution to Day 6, Part 1: " << day.Part1() << endl;
    auto finish = high_resolution_clock::now();
    cout << "Solution to Day 6, Part 1 took " << duration_cast<microseconds>(finish - start).count() / 1000000.0 
         << "s (" << duration_cast<milliseconds>(finish - start).count() << "ms)" << endl;

    auto start2 = high_resolution_clock::now(); // different variable so part 2 can be tested individually if part 1 is commented
    cout << "Solution to Day 6, Part 2: " << day.Part2() << endl;
    auto finish2 = high_resolution_clock::now();
    cout << "Solution to Day 6, Part 2 took " << duration_cast<microseconds>(finish2 - start2).count() / 1000000.0
        << "s (" << duration_cast<milliseconds>(finish2 - start2).count() << "ms)" << endl;
}
