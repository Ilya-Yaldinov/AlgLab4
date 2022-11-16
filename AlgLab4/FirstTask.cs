using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgLab4
{
    public class ConsoleHelper
    {
        int delay;
        public ConsoleHelper(int delay) => this.delay = delay;
        public void WriteColorLine(string s, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(s);
            Console.ForegroundColor = ConsoleColor.White;
            Pause();
        }
        private void Pause()
        {
            Thread.Sleep(delay);
        }
    }

    public class FirstTask
    {
        public void Menu()
        {
            int[] arr = new int[] { 1, 8, 35, 34, 96, 32, 4, 68, 184, 7 };
            bool isCorrectKey = false;
            while (!isCorrectKey)
            {
                Console.WriteLine("Enter 1 for CombSort array or 2 for QuickSort array");
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.D1)
                {
                    CombSort(arr);
                    isCorrectKey = true;
                }
                else if (key.Key == ConsoleKey.D2)
                {
                    QuickSort(arr);
                    isCorrectKey = true;
                }
                else Console.WriteLine("Incorrect key. Please try again");
            }
        }

        public static int[] CombSort(int[] data)
        {

            Console.WriteLine("Enter delay between commands (in ms)");
            ConsoleHelper helper = new ConsoleHelper(int.Parse(Console.ReadLine()));
            double gap = data.Length;
            helper.WriteColorLine($"Setting the initial gap equal to the length of the array, gap = {gap}", ConsoleColor.White);
            bool swaps = true;

            while (gap > 1 || swaps)
            {
                gap /= 1.247330950103979;
                helper.WriteColorLine($"Divide the gap by the reduction factor, gap = {gap}", ConsoleColor.Magenta);

                if (gap < 1)
                {

                    gap = 1;
                    helper.WriteColorLine($"The gap can't be less than 1. Set gap = 1", ConsoleColor.White);
                }

                int i = 0;
                swaps = false;

                while (i + gap < data.Length)
                {
                    helper.WriteColorLine($"Start comparing array[{i}] with other elements", ConsoleColor.White);
                    int igap = i + (int)gap;
                    helper.WriteColorLine($"Checking array[{i}] = {data[i]} and array[{igap}] = {data[igap]} element", ConsoleColor.Yellow);
                    if (data[i] > data[igap])
                    {
                        helper.WriteColorLine($"{data[i]} more than {data[igap]}. Swap array[{i}] and array[{igap}]", ConsoleColor.Green);
                        int temp = data[i];
                        data[i] = data[igap];
                        data[igap] = temp;
                        swaps = true;
                    }
                    else helper.WriteColorLine($"{data[i]} less than {data[igap]}", ConsoleColor.Red);

                    ++i;

                }
            }
            helper.WriteColorLine($"Return sorted array", ConsoleColor.Cyan);
            return data;
        }

        public int[] QuickSort(int[] array)
        {
            Console.WriteLine("Enter delay between commands (in ms)");
            ConsoleHelper helper = new ConsoleHelper(int.Parse(Console.ReadLine()));
            helper.WriteColorLine($"Return sorted array", ConsoleColor.Cyan);
            return QuickSort(array, 0, array.Length - 1, helper);
        }
        public int[] QuickSort(int[] array, int leftIndex, int rightIndex, ConsoleHelper helper)
        {
            var i = leftIndex;
            var j = rightIndex;
            var pivot = array[leftIndex];
            helper.WriteColorLine($" i = {i} - left index, j = {j} - right index, array[{leftIndex}] = {pivot} - pivot element", ConsoleColor.Yellow);

            while (i <= j)
            {
                while (array[i] < pivot)
                {
                    helper.WriteColorLine($"array[{i}] < pivot= {pivot}. Increasing i by 1", ConsoleColor.Blue);
                    i++;
                }

                while (array[j] > pivot)
                {
                    helper.WriteColorLine($"array[{j}] > pivot = {pivot}. Decreasing j by 1", ConsoleColor.Blue);
                    j--;
                }
                if (i <= j)
                {
                    helper.WriteColorLine($"array[{i}] = {array[i]} <= array[{j}] = {array[j]}. Swapping elements.", ConsoleColor.Green);
                    int temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                    i++;
                    j--;
                }
            }

            if (leftIndex < j)
            {
                helper.WriteColorLine($"Start sorting subarray from {leftIndex} by {j}", ConsoleColor.Magenta);
                QuickSort(array, leftIndex, j, helper);
            }
            if (i < rightIndex)
            {
                helper.WriteColorLine($"Start sorting subarray from {i} by {rightIndex}", ConsoleColor.Magenta);
                QuickSort(array, i, rightIndex, helper);
            }
            return array;
        }
    }
}
