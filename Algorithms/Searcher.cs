using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithms
{
    class Searcher
    {
        // Fields
        private int[] array;
        public SearchAlgorithm? algorithm { get; set; } = null;
        public enum SearchAlgorithm
        {
            Linear, RecursionLinear, Binary, Jump, Fibonacci
        }

        // Constructor

        public Searcher(int[] values, SearchAlgorithm? algorithm = SearchAlgorithm.Linear)
        {
            array = new int[values.Length];
            Array.Copy(values, array, values.Length);
            this.algorithm = algorithm;
        }

        // Public functions

        #region Main functions
        public bool? search(int n, bool overloadSortCheck = false)
        {
            if (!overloadSortCheck)
            {
                bool? sorted = new Sorter(array).IsArraySorted();
                if (sorted == null || sorted == false)
                    return null;
            }

            bool found = false;
            switch (algorithm)
            {
                default: break;

                case SearchAlgorithm.Linear:
                    found = LinearSearch(n);
                    break;

                case SearchAlgorithm.RecursionLinear:
                    found = RecursionLinear(n);
                    break;

                case SearchAlgorithm.Binary:
                    found = BinarySearch(n);     // has to be sorted
                    break;

                case SearchAlgorithm.Jump:
                    found = JumpSearch(n);       // has to be sorted
                    break;

                case SearchAlgorithm.Fibonacci:  // has to be sorted
                    found = FibonacciSearch(n);
                    break;
            }
            return found;
        }
        #endregion  

        // Private functions


        #region Linear
        private bool LinearSearch(int n)
        {
            foreach (int value in array)
            {
                if (value == n)
                    return true;
            }
            return false;
        }
        #endregion

        #region Recursion Linear

        private bool RecursionLinear(int n, int i = 0)
        {
            if (array.Length < 1 || i >= array.Length)
                return false;
            if (array[i] == n)
                return true;
            return RecursionLinear(n, ++i);
        }

        #endregion

        #region Binary
        private bool BinarySearch(int n, int? sliceL = null, int? sliceR = null)
        {
            // Initialize if first recursive call
            // Slices will only be null at the very beginning
            sliceL = sliceL ?? 0;
            sliceR = sliceR ?? array.Length - 1;

            // If there are still elements to check:
            if (sliceR >= sliceL)
            {
                // Determine midpoint of array slice
                int mid = (int)(sliceL + (sliceR - sliceL) / 2);

                // If found, return true
                if (array[mid] == n)
                    return true;

                // If n is to the left of the midpoint, adjust slice to the left
                if (n < array[mid])
                    return BinarySearch(n, sliceL, mid - 1);

                // n is to the right of the midpoint, adjust slice to the right
                return BinarySearch(n, mid + 1, sliceR);
            }
            return false;
        }
        #endregion

        #region Jump

        private bool JumpSearch(int n)
        {
            // Determine size of block to jump 
            int blockStep = (int)Math.Floor(Math.Sqrt(array.Length));
            int blockSize = blockStep;

            // Find the block where n is located, if it exists
            int linIndex = 0;
            while (array[Math.Min(blockStep, array.Length) - 1] < n)
            {
                linIndex = blockStep;
                blockStep += blockSize;
                if (linIndex >= array.Length)
                    return false;
            }

            // Linear seach starting from linIndex
            while (array[linIndex] < n)
            {
                // If element is not found in this block, it doesn't exist
                if ((linIndex + 1) == Math.Min(blockStep, array.Length))
                    return false;
                linIndex++;
            }

            // Final check
            return array[linIndex] == n;
        }

        #endregion

        #region Fibonacci

        private bool FibonacciSearch(int n)
        {
            // Setup the first fibonacci numbers
            int fib;
            int prevprevFib = 0, prevFib = 1;

            // Find the first Fibonacci number that is >= length of the array
            // Keep track of the the 2 Fibonacci numbers before
            while ((fib = prevFib + prevprevFib) < array.Length)
            {
                prevprevFib = prevFib;
                prevFib = fib;
            }

            // We use (current-2)'th fib + [(current-2)'th fib after shift left] as our index to check [fibSum].
            //      -> fibSum += prevprevFib
            // This results in an escalating value for our index, comparable to binary
            // but instead of 2 equal parts being used, 2 parts with the size of fibonacci numbers are used.
            int fibSum = -1;
            while (fib > 1)
            {
                // check if the (current-2)'th fibonnaci number is a valid index and check it if so
                // if not, the index of the last element is used.
                int index = Math.Min(fibSum + prevprevFib, array.Length - 1);

                // If the the element at the current index < n
                //      1. We set our search offset to the index we checked.
                //      2. We take a step "back" with our fibonacci numbers
                if (array[index] < n)
                {
                    fibSum = index;

                    fib = prevFib;
                    prevFib = prevprevFib;
                    prevprevFib = fib - prevFib;
                }

                // If the the element at the current index > n
                // If n is right after our fibonacci number, it won't be found here, so we have to check that seperately at the very end
                //      1. We take two steps back to narrow down towards our index
                else if (array[index] > n)
                {
                    fib = prevprevFib;
                    prevFib -= prevprevFib;
                    prevprevFib = fib - prevFib;
                }

                // Element is found
                else return true;
            }

            // Make sure we are all the way narrowed down
            // And check the very last element we missed before
            return (prevFib == 1 && array[fibSum + 1] == n);
        }

        #endregion

    }
}
