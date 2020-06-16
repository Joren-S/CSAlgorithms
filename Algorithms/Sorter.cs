using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithms
{
    class Sorter
    {
        private int[] array;
        
        public Sorter(int[] values)
        {
            array = new int[values.Length];
            Array.Copy(values, array, values.Length);
        }

        public bool? IsArraySorted()
        {
            if (array.Length < 1)
                return null;
            return IsArraySorted(0);
        }

        private bool IsArraySorted(int n)
        {
            if (n + 1 >= array.Length)
                return true;
            if (array[n] > array[n + 1])
                return false;
            return IsArraySorted(n + 1);
        }
    }
}
