using com.sun.xml.@internal.xsom.impl.scd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgLab4
{
    public class NaturallMergeSort
    {
        private string file { get; set; }
        private long segments;
        private int time;
        private int indexKey = 0;

        public NaturallMergeSort(string file, int indexKey, int time)
        {
            this.file = file;
            this.indexKey = indexKey;
            this.time = time;
        }

        public NaturallMergeSort(string file, int time)
        {
            this.file = file;
            this.time = time;
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

        public void SortForInt()
        {
            while (true)
            {
                SplitToFilesInt();
                if (segments == 1) break;
                MergeIntPairs();
            }
            Console.WriteLine("Сортировка законченна.");
        }

        public void SortForString()
        {
            while (true)
            {
                SplitToFilesString();
                if (segments == 1) break;
                MergeStringPairs();
            }
            Console.WriteLine("Сортировка законченна.");
        }

        private void SplitToFilesInt()
        {
            segments = 1;
            using (StreamReader sr = new StreamReader(file))
            using (StreamWriter writerA = new StreamWriter("a.txt"))
            using (StreamWriter writerB = new StreamWriter("b.txt"))
            {
                bool flag = true;
                string prev = sr.ReadLine();
                writerA.WriteLine(prev);
                Console.WriteLine($"Считываем элемент {prev} с файла \"{file}\" и записываем в файл a.txt.");
                while (!sr.EndOfStream)
                {
                    Thread.Sleep(time);
                    string cur = sr.ReadLine();
                    if (Convert.ToInt64(prev.Split(";")[indexKey]) > Convert.ToInt64(cur.Split(";")[indexKey]))
                    {
                        flag = !flag;
                        segments++;
                        if (flag == true) writerB.WriteLine("\'");
                        else writerA.WriteLine("\'");
                    }

                    if (flag)
                    {
                        writerA.WriteLine(cur);
                        Console.WriteLine($"Считываем элемент {cur} с файла \"{file}\" и записываем в файл a.txt.");
                    }
                    else
                    {
                        writerB.WriteLine(cur);
                        Console.WriteLine($"Считываем элемент {cur} с файла \"{file}\" и записываем в файл b.txt.");
                    }
                    prev = cur;
                }
            }
            Console.WriteLine();
        }

        private void SplitToFilesString()
        {
            segments = 1;
            using (StreamReader sr = new StreamReader(file))
            using (StreamWriter writerA = new StreamWriter("a.txt"))
            using (StreamWriter writerB = new StreamWriter("b.txt"))
            {
                bool flag = true;
                string prev = sr.ReadLine();
                writerA.WriteLine(prev);
                Console.WriteLine($"Считываем элемент {prev} с файла \"{file}\" и записываем в файл a.txt.");
                while (!sr.EndOfStream)
                {
                    Thread.Sleep(time);
                    string cur = sr.ReadLine();
                    if (needToReOrder(prev, cur))
                    {
                        flag = !flag;
                        segments++;
                        if(flag == true) writerB.WriteLine("\'");
                        else writerA.WriteLine("\'");
                    }

                    if (flag)
                    {
                        writerA.WriteLine(cur);
                        Console.WriteLine($"Считываем элемент {cur} с файла \"{file}\" и записываем в файл a.txt.");
                    }
                    else
                    {
                        writerB.WriteLine(cur);
                        Console.WriteLine($"Считываем элемент {cur} с файла \"{file}\" и записываем в файл b.txt.");
                    }
                    prev = cur;
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
                string elementA = null, elementB = null;
                bool pickedA = false, pickedB = false;
                while (!readerA.EndOfStream || !readerB.EndOfStream || pickedA || pickedB)
                {
                    if (pickedA == false && pickedB == false)
                    {
                        elementA = "";
                        elementB = "";
                    }

                    if (!readerA.EndOfStream)
                    {
                        if (elementA != "\'" && !pickedA)
                        {
                            Thread.Sleep(time);
                            elementA = readerA.ReadLine();
                            Console.WriteLine($"Считываем элемент {elementA} с файла \"a.txt\".");
                            pickedA = true;
                        }
                        if(elementA == "\'") pickedA = false;
                    }

                    if (!readerB.EndOfStream)
                    {
                        if (elementB != "\'" && !pickedB)
                        {
                            Thread.Sleep(time);
                            elementB = readerB.ReadLine();
                            Console.WriteLine($"Считываем элемент {elementB} с файла \"b.txt\".");
                            pickedB = true;
                        }
                        if (elementB == "\'") pickedB = false;
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
                                pickedA = false;
                            }
                            else
                            {
                                Thread.Sleep(time);
                                Console.WriteLine($"Добовляем {elementB} из файла \"b.txt\" в файл \"{file}\".");
                                sr.WriteLine(elementB);
                                pickedB = false;
                            }
                        }
                        else
                        {
                            Thread.Sleep(time);
                            Console.WriteLine($"Добовляем {elementA} из файла \"a.txt\" в файл \"{file}\".");
                            sr.WriteLine(elementA);
                            pickedA = false;
                        }
                    }
                    else if (pickedB)
                    {
                        Thread.Sleep(time);
                        Console.WriteLine($"Добовляем {elementB} из файла \"b.txt\" в файл \"{file}\".");
                        sr.WriteLine(elementB);
                        pickedB = false;
                    }
                }
                Thread.Sleep(time);
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        private void MergeStringPairs()
        {
            using (StreamReader readerA = new StreamReader("a.txt"))
            using (StreamReader readerB = new StreamReader("b.txt"))
            using (StreamWriter sr = new StreamWriter(file))
            {
                string elementA = null, elementB = null;
                bool pickedA = false, pickedB = false;
                while (!readerA.EndOfStream || !readerB.EndOfStream || pickedA || pickedB)
                {
                    if (pickedA == false && pickedB == false)
                    {
                        elementA = "";
                        elementB = "";
                    }

                    if (!readerA.EndOfStream)
                    {
                        if (elementA != "\'" && !pickedA)
                        {
                            Thread.Sleep(time);
                            elementA = readerA.ReadLine();
                            Console.WriteLine($"Считываем элемент {elementA} с файла \"a.txt\".");
                            pickedA = true;
                        }
                        if (elementA == "\'") pickedA = false;
                    }

                    if (!readerB.EndOfStream)
                    {
                        if (elementB != "\'" && !pickedB)
                        {
                            Thread.Sleep(time);
                            elementB = readerB.ReadLine();
                            Console.WriteLine($"Считываем элемент {elementB} с файла \"b.txt\".");
                            pickedB = true;
                        }
                        if (elementB == "\'") pickedB = false;
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
                                pickedA = false;
                            }
                            else
                            {
                                Thread.Sleep(time);
                                Console.WriteLine($"Добовляем {elementB} из файла \"b.txt\" в файл \"{file}\".");
                                sr.WriteLine(elementB);
                                pickedB = false;
                            }
                        }
                        else
                        {
                            Thread.Sleep(time);
                            Console.WriteLine($"Добовляем {elementA} из файла \"a.txt\" в файл \"{file}\".");
                            sr.WriteLine(elementA);
                            pickedA = false;
                        }
                    }
                    else if (pickedB)
                    {
                        Thread.Sleep(time);
                        Console.WriteLine($"Добовляем {elementB} из файла \"b.txt\" в файл \"{file}\".");
                        sr.WriteLine(elementB);
                        pickedB = false;
                    }
                }
                Thread.Sleep(time);
                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }
}
