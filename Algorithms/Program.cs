using System;
using System.Threading;
using System.Collections.Generic;

namespace Algorithms
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread thr = new Thread(new ThreadStart(ThreadEntry), 100000000);
            thr.Start();
            while (thr.IsAlive) { };
        }


        static void ThreadEntry()
        {
            const int
                GEN_COUNT = 1000000,
                GEN_MIN = -100000,
                GEN_MAX = 100000,

                SEARCH_COUNT = 150;
            const bool
                GEN_SORT = true;
            

            #region Generator

            int[] arr = new int[GEN_COUNT];
            Random random = new Random();
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = random.Next(GEN_MIN, GEN_MAX);
            }

            if (GEN_SORT) Array.Sort(arr);

            #endregion

            #region Benchmark every algorithm

            Dictionary<Searcher.SearchAlgorithm, double> StoredTime = new Dictionary<Searcher.SearchAlgorithm, double>();
            foreach (Searcher.SearchAlgorithm algorithm in Enum.GetValues(typeof(Searcher.SearchAlgorithm)))
            {

                Searcher srch = new Searcher(arr, algorithm);
                Benchmark benchmark = new Benchmark($"\nTask: Searching {SEARCH_COUNT} values\n" +
                    $"Array length: {GEN_COUNT}\n" +
                    $"Algorithm: {algorithm.ToString()}");

                int foundValues = 0;
                benchmark.Start();
                for (int i = 0; i < SEARCH_COUNT; i++)
                {
                    if (srch.search(i))
                        foundValues++;
                }
                StoredTime[algorithm] = benchmark.Stop();
                Console.WriteLine($"{algorithm.ToString()}: Found {foundValues} {(foundValues == 1 ? "value" : "values")}");
            }


            Console.WriteLine("\n\n------------------------\n\n");
            foreach (Searcher.SearchAlgorithm algorithm in Enum.GetValues(typeof(Searcher.SearchAlgorithm)))
            {
                Console.WriteLine($"{algorithm.ToString()} - {StoredTime[algorithm]} ms");
            }

            #endregion

            Console.ReadLine();
        }

    }
}
