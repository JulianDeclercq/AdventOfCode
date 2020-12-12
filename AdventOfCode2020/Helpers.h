#pragma once
#include <chrono>
#include <string>
#include <iostream>

namespace Helpers
{
    class StopWatch
    {
    private:
        bool _running = false;
        std::chrono::steady_clock::time_point _start;
        std::chrono::duration<long long, std::nano> _elapsed = std::chrono::duration<long long, std::nano>::zero();
    public:
        void Start();
        void Stop();
        void Reset();
        std::string Formatted();
    };

	struct Point
	{
		Point(int x, int y) : X(x), Y(y) {}
		int X = 0;
		int Y = 0;

		std::string ToString()
		{
			return std::to_string(X) + ", " + std::to_string(Y);
		}

		Point& operator+=(const Point& rhs)
		{
			X += rhs.X;
			Y += rhs.Y;
			return *this; // return the result by reference
		}
	};
}
