using System;
using System.Linq;

namespace SharpAudioTests
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] array = Enumerable.Range(1, 100).ToArray();

            int k = 1;
            int k2 = 1;

            int i = 0;

            while (k2 <= array[^1] && i < array.Length)
            {
                if (k2 == array[i])
                {
                    i++;
                }
                else
                {
                    if (k2 > array[i])
                    {
                        Console.WriteLine(array[i]);
                        i++;
                    }
                    else
                    {
                        k++;
                        k2 = k * k;
                    }
                }
            }
        }
    }
}