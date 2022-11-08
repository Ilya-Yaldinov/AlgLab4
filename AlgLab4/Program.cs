
using System.Diagnostics;

namespace AlgLab3
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.Write("Выберите тип сортировки(для работы с числами - 1, для работы со строками 2): ");
            int choise = Convert.ToInt32(Console.ReadLine());
            if( choise == 1)
            {
                Console.Write("Введите название выборки и через пробел название по которому делаем выбор: ");
                string selectedClass = Console.ReadLine();
                Console.Write("Введите ключ сортировки: ");
                string sortKey = Console.ReadLine();
                Console.Write("Введите название файла без расширения(файл должен быть *.txt): ");
                string fileName = Console.ReadLine();
                Console.Write("Введите задержку(1 - 0,1 сек, а 10 - 1 сек):");
                int time = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine();
                Console.WriteLine();
                ExternalMergeSort external = new ExternalMergeSort(selectedClass, sortKey, $"{fileName}.txt", time * 100);
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
    }

    public class ExternalMergeSort
    {

        public ExternalMergeSort(string selectedClass, string sortKey, string fileName, int time)
        {
            ExternalSortForInt externalSortForInt = new ExternalSortForInt(selectedClass.Split(' '), sortKey, fileName, time);
        }

        public ExternalMergeSort(string fileName, int time)
        {
            DirectMerge directMerge = new DirectMerge(fileName, time);
        }
    }

    public class ExternalSortForInt
    {
        private string[] selectedClass;
        private string sortKey;
        private string fileName;
        int time = 0;
        private int indexOfKey = 0;
        private int indexOfClass = 0;

        public ExternalSortForInt(string[] selectedClass, string sortKey, string fileName, int time)
        {
            this.selectedClass = selectedClass;
            this.sortKey = sortKey;
            this.fileName = fileName;
            this.time = time;
            Select();
        }

        private void Select()
        {
            StreamReader sr = new StreamReader(fileName);
            StreamWriter sw = new StreamWriter("sort.txt");
            var head = sr.ReadLine().Split(';');
            for (int i = 0; i < head.Length; i++)
            {
                if (head[i].ToLower() == sortKey.ToLower()) indexOfKey = i;
                if (head[i].ToLower() == selectedClass[0].ToLower()) indexOfClass = i;
            }
            while (!sr.EndOfStream)
            {
                var str = sr.ReadLine();
                var items = str.Split(';');
                if (items[indexOfClass] == selectedClass[1])
                {
                    sw.WriteLine(str);
                }
            }
            sw.Close();
            sr.Close();
            DirectMerge directMerge = new DirectMerge("sort.txt", indexOfKey, time);
        }
    }

    public class DirectMerge
    {
        private string file { get; set; }
        private long iterations, segments;
        private int time;
        private int indexKey = 0;

        public DirectMerge(string file, int indexKey, int time)
        {
            this.file = file;
            this.indexKey = indexKey;
            this.time = time;
            iterations = 1;
            SortForInt();
        }

        public DirectMerge(string file, int time)
        {
            this.file = file;
            this.time = time;
            iterations = 1;
            SortForString();
        }

        public void SortForInt()
        {
            while (true)
            {
                SplitToFiles();
                if (segments == 1) break;
                MergeIntPairs();
            }
            Console.WriteLine("Сортировка законченна.");
        }

        public void SortForString()
        {
            while (true)
            {
                SplitToFiles();
                if (segments == 1) break;
                MergeStringPairs();
            }
            Console.WriteLine("Сортировка законченна.");
        }

        private static bool needToReOrder(string s1, string s2)
        {
            for (int i = 0; i < (s1.Length > s2.Length ? s2.Length : s1.Length); i++)
            {
                if (s1.ToCharArray()[i] < s2.ToCharArray()[i]) return false;
                if (s1.ToCharArray()[i] > s2.ToCharArray()[i]) return true;
            }
            return false;
        }

        private void SplitToFiles()
        {
            segments = 1;
            using (StreamReader sr = new StreamReader(file))
            using (StreamWriter writerA = new StreamWriter("a.txt"))
            using (StreamWriter writerB = new StreamWriter("b.txt"))
            {
                long counter = 0;
                bool flag = true;

                while (!sr.EndOfStream)
                {
                    Thread.Sleep(time);
                    if (counter == iterations)
                    {
                        flag = !flag;
                        counter = 0;
                        segments++;
                    }
                    string element = sr.ReadLine();

                    if (flag)
                    {
                        writerA.WriteLine(element);
                        Console.WriteLine($"Считываем элемент {element} с файла \"{file}\" и записываем в файл a.txt.");
                    }
                    else
                    {
                        writerB.WriteLine(element);
                        Console.WriteLine($"Считываем элемент {element} с файла \"{file}\" и записываем в файл b.txt.");
                    }
                    counter++;
                }
            }
            Console.WriteLine();
        }

        private void MergeIntPairs() 
        {
            using (StreamReader readerA = new StreamReader("a.txt"))
            using (StreamReader readerB = new StreamReader("b.txt"))
            using (StreamWriter sr = new StreamWriter(file))
            {
                long counterA = iterations, counterB = iterations;
                string elementA = null, elementB = null;
                bool pickedA = false, pickedB = false;
                while (!readerA.EndOfStream || !readerB.EndOfStream)
                {
                    if (counterA == 0 && counterB != 0)
                    {
                        Console.WriteLine($"Серия a закончилась, дописываем {counterB} элементов серии b.");
                    }
                    if (counterB == 0 && counterA != 0)
                    {
                        Console.WriteLine($"Серия b закончилась, дописываем {counterB} элементов серии a.");
                    }
                    if (counterA == 0 && counterB == 0)
                    {
                        counterA = iterations;
                        counterB = iterations;
                    }

                    if (!readerA.EndOfStream)
                    {
                        if (counterA > 0 && !pickedA)
                        {
                            Thread.Sleep(time);
                            elementA = readerA.ReadLine();
                            Console.WriteLine($"Считываем элемент {elementA} с файла \"a.txt\".");
                            pickedA = true;
                        }
                    }

                    if (!readerB.EndOfStream)
                    {
                        if (counterB > 0 && !pickedB)
                        {
                            Thread.Sleep(time);
                            elementB = readerB.ReadLine();
                            Console.WriteLine($"Считываем элемент {elementB} с файла \"b.txt\".");
                            pickedB = true;
                        }
                    }

                    if (pickedA)
                    {
                        if (pickedB)
                        {
                            if (Convert.ToInt64(elementA.Split(";")[indexKey]) < Convert.ToInt64(elementB.Split(";")[indexKey]))
                            {
                                Thread.Sleep(time);
                                Console.WriteLine($"Добовляем {elementA} из файла \"a.txt\" в файл \"{file}\".");
                                sr.WriteLine(elementA);
                                counterA--;
                                pickedA = false;
                            }
                            else
                            {
                                Thread.Sleep(time);
                                Console.WriteLine($"Добовляем {elementB} из файла \"b.txt\" в файл \"{file}\".");
                                sr.WriteLine(elementB);
                                counterB--;
                                pickedB = false;
                            }
                        }
                        else
                        {
                            Thread.Sleep(time);
                            Console.WriteLine($"Добовляем {elementA} из файла \"a.txt\" в файл \"{file}\".");
                            sr.WriteLine(elementA);
                            counterA--;
                            pickedA = false;
                        }
                    }
                    else if (pickedB)
                    {
                        Thread.Sleep(time);
                        Console.WriteLine($"Добовляем {elementB} из файла \"b.txt\" в файл \"{file}\".");
                        sr.WriteLine(elementB);
                        counterB--;
                        pickedB = false;
                    }
                }
                iterations *= 2;
                Thread.Sleep(time);
                Console.WriteLine();
                Console.WriteLine($"Увеличиваем размер серии в 2 раза(теперь она равна {iterations}).");
                Console.WriteLine();
            }
        }

        private void MergeStringPairs()
        {
            using (StreamReader readerA = new StreamReader("a.txt"))
            using (StreamReader readerB = new StreamReader("b.txt"))
            using (StreamWriter sr = new StreamWriter(file))
            {
                long counterA = iterations, counterB = iterations;
                string elementA = null, elementB = null;
                bool pickedA = false, pickedB = false;
                while (!readerA.EndOfStream || !readerB.EndOfStream)
                {
                    if (counterA == 0 && counterB != 0)
                    {
                        Console.WriteLine($"Серия a закончилась, дописываем {counterB} элементов серии b.");
                    }
                    if (counterB == 0 && counterA != 0)
                    {
                        Console.WriteLine($"Серия b закончилась, дописываем {counterB} элементов серии a.");
                    }
                    if (counterA == 0 && counterB == 0)
                    {
                        counterA = iterations;
                        counterB = iterations;
                    }

                    if (!readerA.EndOfStream)
                    {
                        if (counterA > 0 && !pickedA)
                        {
                            Thread.Sleep(time);
                            elementA = readerA.ReadLine();
                            Console.WriteLine($"Считываем элемент {elementA} с файла \"a.txt\".");
                            pickedA = true;
                        }
                    }

                    if (!readerB.EndOfStream)
                    {
                        if (counterB > 0 && !pickedB)
                        {
                            Thread.Sleep(time);
                            elementB = readerB.ReadLine();
                            Console.WriteLine($"Считываем элемент {elementB} с файла \"b.txt\".");
                            pickedB = true;
                        }
                    }

                    if (pickedA)
                    {
                        if (pickedB)
                        {
                            if (needToReOrder(elementA, elementB) == false)
                            {
                                Thread.Sleep(time);
                                Console.WriteLine($"Добовляем {elementA} из файла \"a.txt\" в файл \"{file}\".");
                                sr.WriteLine(elementA);
                                counterA--;
                                pickedA = false;
                            }
                            else
                            {
                                Thread.Sleep(time);
                                Console.WriteLine($"Добовляем {elementB} из файла \"b.txt\" в файл \"{file}\".");
                                sr.WriteLine(elementB);
                                counterB--;
                                pickedB = false;
                            }
                        }
                        else
                        {
                            Thread.Sleep(time);
                            Console.WriteLine($"Добовляем {elementA} из файла \"a.txt\" в файл \"{file}\".");
                            sr.WriteLine(elementA);
                            counterA--;
                            pickedA = false;
                        }
                    }
                    else if (pickedB)
                    {
                        Thread.Sleep(time);
                        Console.WriteLine($"Добовляем {elementB} из файла \"b.txt\" в файл \"{file}\".");
                        sr.WriteLine(elementB);
                        counterB--;
                        pickedB = false;
                    }
                }
                iterations *= 2;
                Thread.Sleep(time);
                Console.WriteLine();
                Console.WriteLine($"Увеличиваем размер серии в 2 раза(теперь она равна {iterations}).");
                Console.WriteLine();
            }
        }
    }
}