
using java.sql;
using System.Diagnostics;

namespace AlgLab4
{
    class Program
    {
        public static void Main(string[] args)
        {
            SelectTask();
        }

        public static void SelectTask()
        {
            Console.Write("Введите 1 для первого задания, 2 для второго и 3 для третьего: ");
            string choise = Console.ReadLine();
            if (choise == "1") FirstTask();
            else if (choise == "2") SecondTask();
            else ThirdTask();
        }

        public static void FirstTask()
        {
            FirstTask firstTask = new FirstTask();
            firstTask.Menu();
        }

        public static void SecondTask()
        {
            Console.Write("Выберите тип сортировки(для работы с числами - 1, для работы со строками 2): ");
            int choise = Convert.ToInt32(Console.ReadLine());
            if (choise == 1)
            {
                /*Console.Write("Введите название выборки и через пробел название по которому делаем выбор: ");
                string selectedClass = Console.ReadLine();
                Console.Write("Введите ключ сортировки: ");
                string sortKey = Console.ReadLine();
                Console.Write("Введите название файла без расширения(файл должен быть *.txt): ");
                string fileName = Console.ReadLine();
                Console.Write("Введите задержку(1 - 0,1 сек, а 10 - 1 сек):");
                int time = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine();
                Console.WriteLine();
                ExternalMergeSort external = new ExternalMergeSort(selectedClass, sortKey, $"{fileName}.txt", time * 100);*/
                ExternalMergeSort external = new ExternalMergeSort("Континент Азия", "Население", $"people.txt", 100);
            }
            else
            {
                Console.Write("Введите название файла без расширения(файл должен быть *.txt): ");
                string fileName = Console.ReadLine();
                Console.Write("Введите задержку(1 - 0,1 сек, а 10 - 1 сек):");
                int time = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine();
                Console.WriteLine();
                ExternalMergeSort external = new ExternalMergeSort($"{fileName}.txt", time * 100);
            }
        }

        public static void ThirdTask()
        {
            Console.Write("Выберите тип сортировки(Shell Sort - 1, LSD sort 2): ");
            int choise = Convert.ToInt32(Console.ReadLine());
            if (choise == 1)
            {
                Console.Write("Введите название файла с расширения(файл должен быть *.txt): ");
                TaskThree.path = Console.ReadLine();
                Console.WriteLine();
                TaskThree.ShellSort();
            }
            else
            {
                Console.Write("Введите название файла с расширения(файл должен быть *.txt): ");
                TaskThree.path = Console.ReadLine();
                Console.WriteLine();
                TaskThree.LSDSort();
            }
        }
    }
}