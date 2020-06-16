using System;
using System.Diagnostics;

class Benchmark
{
    public bool running { get; set; } = false;
    public double runtime { get; set; } = 0;

    private readonly Stopwatch sw = new Stopwatch();
    private readonly string task;

    public Benchmark(string task, bool autostart = false)
    {
        this.task = task;
        if (autostart)
        {
            sw.Start();
            running = true;
        }
    }

    public bool Start()
    {
        if (!running)
        {
            runtime = 0;
            sw.Start();
            running = true;
        }
        return running;
    }

    public double Stop(bool print = true)
    {
        if (running)
        {
            sw.Stop();
            running = false;
            if (print)
                Console.WriteLine($"{this.task} took {sw.Elapsed.TotalMilliseconds} ms");
            runtime = sw.Elapsed.TotalMilliseconds;
            return runtime;
        }
        return -1;
    }
}
