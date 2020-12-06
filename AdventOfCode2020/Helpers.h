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
}
