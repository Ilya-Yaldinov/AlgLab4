namespace AlgLab4
{
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
            SortIntChoise();
        }

        public DirectMerge(string file, int time)
        {
            this.file = file;
            this.time = time;
            iterations = 1;
            SortStringChoise();
        }

        private void SortIntChoise()
        {
            Console.WriteLine("1.Простое слияние:");
            Console.WriteLine("2.Естественное слияние:");
            Console.WriteLine("3.Многопутевое слияние:");
            int num = Convert.ToInt32(Console.ReadLine());
            if(num == 1)
            {
                SortForInt();
            }
            if(num == 2)
            {
                NaturallMergeSort sort = new NaturallMergeSort(file, indexKey, time);
                sort.SortForInt();
            }
            else
            {
                MultipathMergeSort sort = new MultipathMergeSort(file, indexKey, time);
                sort.SortForInt();
            }
        }

        private void SortStringChoise()
        {
            Console.WriteLine("1.Простое слияние:");
            Console.WriteLine("2.Естественное слияние:");
            Console.WriteLine("3.Многопутевое слияние:");
            int num = Convert.ToInt32(Console.ReadLine());
            if (num == 1)
            {
                SortForString();
            }
            if (num == 2)
            {
                NaturallMergeSort sort = new NaturallMergeSort(file, indexKey, time);
                sort.SortForString();
            }
            else
            {
                MultipathMergeSort sort = new MultipathMergeSort(file, indexKey, time);
                sort.SortForString();
            }
        }

        private void SortForInt()
        {
            while (true)
            {
                SplitToFiles();
                if (segments == 1) break;
                SimpleMergeIntPairs();
            }
            Console.WriteLine("Сортировка законченна.");
        }

        private void SortForString()
        {
            while (true)
            {
                SplitToFiles();
                if (segments == 1) break;
                SimpleMergeStringPairs();
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

        private void SimpleMergeIntPairs() 
        {
            using (StreamReader readerA = new StreamReader("a.txt"))
            using (StreamReader readerB = new StreamReader("b.txt"))
            using (StreamWriter sr = new StreamWriter(file))
            {
                long counterA = iterations, counterB = iterations;
                string elementA = null, elementB = null;
                bool pickedA = false, pickedB = false;
                while (!readerA.EndOfStream || !readerB.EndOfStream || pickedA || pickedB)
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

        private void SimpleMergeStringPairs()
        {
            using (StreamReader readerA = new StreamReader("a.txt"))
            using (StreamReader readerB = new StreamReader("b.txt"))
            using (StreamWriter sr = new StreamWriter(file))
            {
                long counterA = iterations, counterB = iterations;
                string elementA = null, elementB = null;
                bool pickedA = false, pickedB = false;
                while (!readerA.EndOfStream || !readerB.EndOfStream || pickedA || pickedB)
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