using com.sun.xml.@internal.xsom.impl.scd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgLab4
{
    public class MultipathMergeSort
    {
        private string file { get; set; }
        private long segments;
        private int time;
        private int indexKey = 0;

        public MultipathMergeSort(string file, int indexKey, int time)
        {
            this.file = file;
            this.indexKey = indexKey;
            this.time = time;
        }

        public MultipathMergeSort(string file, int time)
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
            SplitToFilesInt();
            if (segments == 1)
            {
                Console.WriteLine("Сортировка законченна.");
                return;
            }
            while (true)
            {
                MergeIntPairs("1", "2", "3", "4", "5", "6");
                if (segments == 1) break;
                MergeIntPairs("4", "5", "6", "1", "2", "3");
            }
            Console.WriteLine("Сортировка законченна.");
        }

        public void SortForString()
        {
            SplitToFilesString();
            if(segments == 1)
            {
                Console.WriteLine("Сортировка законченна.");
                return;
            }
            while (true)
            {
                MergeStringPairs("1", "2", "3", "4", "5", "6");
                if (segments == 1) break;
                MergeStringPairs("4", "5", "6", "1", "2", "3");
            }
            Console.WriteLine("Сортировка законченна.");
        }

        private void SplitToFilesInt()
        {
            segments = 1;
            using (StreamReader sr = new StreamReader(file))
            using (StreamWriter writer1 = new StreamWriter("1.txt"))
            using (StreamWriter writer2 = new StreamWriter("2.txt"))
            using (StreamWriter writer3 = new StreamWriter("3.txt"))
            {
                string prev = sr.ReadLine();
                writer1.WriteLine(prev);
                Console.WriteLine($"Считываем элемент {prev} с файла \"{file}\" и записываем в файл 1.txt.");
                while (!sr.EndOfStream)
                {
                    Thread.Sleep(time);
                    string cur = sr.ReadLine();
                    if (Convert.ToInt64(prev.Split(";")[indexKey]) > Convert.ToInt64(cur.Split(";")[indexKey]))
                    {
                        segments++;
                        if (segments % 3 == 0) writer2.WriteLine("\'");
                        else if (segments % 3 == 1) writer3.WriteLine("\'");
                        else writer1.WriteLine("\'");
                    }

                    if (segments % 3 == 1)
                    {
                        writer1.WriteLine(cur);
                        Console.WriteLine($"Считываем элемент {cur} с файла \"{file}\" и записываем в файл 1.txt.");
                    }
                    else if(segments % 3 == 2)
                    {
                        writer2.WriteLine(cur);
                        Console.WriteLine($"Считываем элемент {cur} с файла \"{file}\" и записываем в файл 2.txt.");
                    }
                    else
                    {
                        writer3.WriteLine(cur);
                        Console.WriteLine($"Считываем элемент {cur} с файла \"{file}\" и записываем в файл 3.txt.");
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
            using (StreamWriter writer1 = new StreamWriter("1.txt"))
            using (StreamWriter writer2 = new StreamWriter("2.txt"))
            using (StreamWriter writer3 = new StreamWriter("3.txt"))
            {
                string prev = sr.ReadLine();
                writer1.WriteLine(prev);
                Console.WriteLine($"Считываем элемент {prev} с файла \"{file}\" и записываем в файл 1.txt.");
                while (!sr.EndOfStream)
                {
                    Thread.Sleep(time);
                    string cur = sr.ReadLine();
                    if (needToReOrder(prev, cur))
                    {
                        segments++;
                        if (segments % 3 == 0) writer2.WriteLine("\'");
                        else if (segments % 3 == 1) writer3.WriteLine("\'");
                        else writer1.WriteLine("\'");
                    }

                    if (segments % 3 == 1)
                    {
                        writer1.WriteLine(cur);
                        Console.WriteLine($"Считываем элемент {cur} с файла \"{file}\" и записываем в файл 1.txt.");
                    }
                    else if (segments % 3 == 2)
                    {
                        writer2.WriteLine(cur);
                        Console.WriteLine($"Считываем элемент {cur} с файла \"{file}\" и записываем в файл 2.txt.");
                    }
                    else
                    {
                        writer3.WriteLine(cur);
                        Console.WriteLine($"Считываем элемент {cur} с файла \"{file}\" и записываем в файл 3.txt.");
                    }
                    prev = cur;
                }
            }
            Console.WriteLine();
        }

        private void MergeIntPairs(string a, string b, string c, string d, string e, string f)
        {
            segments = 0;
            string curFile = d;
            using (StreamReader reader1 = new StreamReader($"{a}.txt"))
            using (StreamReader reader2 = new StreamReader($"{b}.txt"))
            using (StreamReader reader3 = new StreamReader($"{c}.txt"))
            using (StreamWriter writer1 = new StreamWriter($"{d}.txt"))
            using (StreamWriter writer2 = new StreamWriter($"{e}.txt"))
            using (StreamWriter writer3 = new StreamWriter($"{f}.txt"))
            {
                string element1 = null, element2 = null, element3 = null;
                bool picked1 = false, picked2 = false, picked3 = false;
                while (!reader1.EndOfStream || !reader2.EndOfStream || !reader2.EndOfStream || picked1 || picked2 || picked3)
                {
                    if (picked1 == false && picked2 == false && picked3 == false)
                    {
                        element1 = "";
                        element2 = "";
                        element3 = "";
                        if (segments > 1)
                        {
                            int count = WriteToFile(writer1, writer2, writer3, "\'");
                            if (count == 1) curFile = d;
                            if (count == 2) curFile = e;
                            else curFile = f;
                        }
                        segments++;
                    }

                    if (!reader1.EndOfStream)
                    {
                        if (element1 != "\'" && !picked1)
                        {
                            Thread.Sleep(time);
                            element1 = reader1.ReadLine();
                            Console.WriteLine($"Считываем элемент {element1} с файла \"{a}.txt\".");
                            picked1 = true;
                        }
                        if (element1 == "\'") picked1 = false;
                    }

                    if (!reader2.EndOfStream)
                    {
                        if (element2 != "\'" && !picked2)
                        {
                            Thread.Sleep(time);
                            element2 = reader2.ReadLine();
                            Console.WriteLine($"Считываем элемент {element2} с файла \"{b}.txt\".");
                            picked2 = true;
                        }
                        if (element2 == "\'") picked2 = false;
                    }

                    if (!reader3.EndOfStream)
                    {
                        if (element3 != "\'" && !picked3)
                        {
                            Thread.Sleep(time);
                            element3 = reader3.ReadLine();
                            Console.WriteLine($"Считываем элемент {element3} с файла \"{c}.txt\".");
                            picked3 = true;
                        }
                        if (element3 == "\'") picked3 = false;
                    }

                    if (picked1)
                    {
                        if (picked2)
                        {
                            if (Convert.ToInt64(element1.Split(";")[indexKey]) < Convert.ToInt64(element2.Split(";")[indexKey]))
                            {
                                if (picked3)
                                {
                                    if (Convert.ToInt64(element1.Split(";")[indexKey]) < Convert.ToInt64(element3.Split(";")[indexKey]))
                                    {
                                        Thread.Sleep(time);
                                        Console.WriteLine($"Добовляем {element1} из файла \"{a}.txt\" в файл \"{curFile}.txt\".");
                                        WriteToFile(writer1, writer2, writer3, element1);
                                        picked1 = false;
                                    }
                                    else
                                    {
                                        Thread.Sleep(time);
                                        Console.WriteLine($"Добовляем {element3} из файла \"{a}.txt\" в файл \"{curFile}.txt\".");
                                        WriteToFile(writer1, writer2, writer3, element3);
                                        picked3 = false;
                                    }
                                }
                                else
                                {
                                    Thread.Sleep(time);
                                    Console.WriteLine($"Добовляем {element1} из файла \"{a}.txt\" в файл \"{curFile}.txt\".");
                                    WriteToFile(writer1, writer2, writer3, element1);
                                    picked1 = false;
                                }
                            }
                            else
                            {
                                if (picked3)
                                {
                                    if (Convert.ToInt64(element2.Split(";")[indexKey]) < Convert.ToInt64(element3.Split(";")[indexKey]))
                                    {
                                        Thread.Sleep(time);
                                        Console.WriteLine($"Добовляем {element2} из файла \"{a}.txt\" в файл \"{curFile}.txt\".");
                                        WriteToFile(writer1, writer2, writer3, element2);
                                        picked2 = false;
                                    }
                                    else
                                    {
                                        Thread.Sleep(time);
                                        Console.WriteLine($"Добовляем {element3} из файла \"{a}.txt\" в файл \"{curFile}.txt\".");
                                        WriteToFile(writer1, writer2, writer3, element3);
                                        picked3 = false;
                                    }
                                }
                                else
                                {
                                    Thread.Sleep(time);
                                    Console.WriteLine($"Добовляем {element2} из файла \"{a}.txt\" в файл \"{curFile}.txt\".");
                                    WriteToFile(writer1, writer2, writer3, element2);
                                    picked2 = false;
                                }
                            }
                        }
                        else if (picked3)
                        {
                            if (Convert.ToInt64(element1.Split(";")[indexKey]) < Convert.ToInt64(element3.Split(";")[indexKey]))
                            {
                                Thread.Sleep(time);
                                Console.WriteLine($"Добовляем {element1} из файла \"{a}.txt\" в файл \"{curFile}.txt\".");
                                WriteToFile(writer1, writer2, writer3, element1);
                                picked1 = false;
                            }
                            else
                            {
                                Thread.Sleep(time);
                                Console.WriteLine($"Добовляем {element3} из файла \"{c}.txt\" в файл \"{curFile}.txt\".");
                                WriteToFile(writer1, writer2, writer3, element3);
                                picked3 = false;
                            }
                        }
                        else
                        {
                            Thread.Sleep(time);
                            Console.WriteLine($"Добовляем {element1} из файла \"{a}.txt\" в файл \"{curFile}.txt\".");
                            WriteToFile(writer1, writer2, writer3, element1);
                            picked1 = false;
                        }
                    }
                    else if (picked2)
                    {
                        if (picked3)
                        {
                            if (Convert.ToInt64(element2.Split(";")[indexKey]) < Convert.ToInt64(element3.Split(";")[indexKey]))
                            {
                                Thread.Sleep(time);
                                Console.WriteLine($"Добовляем {element2} из файла \"{b}.txt\" в файл \"{curFile}.txt\".");
                                WriteToFile(writer1, writer2, writer3, element2);
                                picked2 = false;
                            }
                            else
                            {
                                Thread.Sleep(time);
                                Console.WriteLine($"Добовляем {element3} из файла \"{c}.txt\" в файл \"{curFile}.txt\".");
                                WriteToFile(writer1, writer2, writer3, element3);
                                picked3 = false;
                            }
                        }
                        else
                        {
                            Thread.Sleep(time);
                            Console.WriteLine($"Добовляем {element2} из файла \"{b}.txt\" в файл \"{curFile}.txt\".");
                            WriteToFile(writer1, writer2, writer3, element2);
                            picked2 = false;
                        }
                    }
                    else if (picked3)
                    {
                        Thread.Sleep(time);
                        Console.WriteLine($"Добовляем {element3} из файла \"{b}.txt\" в файл \"{curFile}.txt\".");
                        WriteToFile(writer1, writer2, writer3, element3);
                        picked3 = false;
                    }
                }
                Thread.Sleep(time);
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        private void MergeStringPairs(string a, string b, string c, string d, string e, string f)
        {
            segments = 0;
            string curFile = d;
            using (StreamReader reader1 = new StreamReader($"{a}.txt"))
            using (StreamReader reader2 = new StreamReader($"{b}.txt"))
            using (StreamReader reader3 = new StreamReader($"{c}.txt"))
            using (StreamWriter writer1 = new StreamWriter($"{d}.txt"))
            using (StreamWriter writer2 = new StreamWriter($"{e}.txt"))
            using (StreamWriter writer3 = new StreamWriter($"{f}.txt"))
            {
                string element1 = null, element2 = null, element3 = null;
                bool picked1 = false, picked2 = false, picked3 = false;
                while (!reader1.EndOfStream || !reader2.EndOfStream || !reader2.EndOfStream || picked1 || picked2 || picked3)
                {
                    if (picked1 == false && picked2 == false && picked3 == false)
                    {
                        element1 = "";
                        element2 = "";
                        element3 = "";
                        if (segments > 1)
                        {
                            int count = WriteToFile(writer1, writer2, writer3, "\'");
                            if (count == 1) curFile = d;
                            if (count == 2) curFile = e;
                            else curFile = f;
                        }
                        segments++;
                    }

                    if (!reader1.EndOfStream)
                    {
                        if (element1 != "\'" && !picked1)
                        {
                            Thread.Sleep(time);
                            element1 = reader1.ReadLine();
                            Console.WriteLine($"Считываем элемент {element1} с файла \"{a}.txt\".");
                            picked1 = true;
                        }
                        if (element1 == "\'") picked1 = false;
                    }

                    if (!reader2.EndOfStream)
                    {
                        if (element2 != "\'" && !picked2)
                        {
                            Thread.Sleep(time);
                            element2 = reader2.ReadLine();
                            Console.WriteLine($"Считываем элемент {element2} с файла \"{b}.txt\".");
                            picked2 = true;
                        }
                        if (element2 == "\'") picked2 = false;
                    }

                    if (!reader3.EndOfStream)
                    {
                        if (element3 != "\'" && !picked3)
                        {
                            Thread.Sleep(time);
                            element3 = reader3.ReadLine();
                            Console.WriteLine($"Считываем элемент {element3} с файла \"{c}.txt\".");
                            picked3 = true;
                        }
                        if (element3 == "\'") picked3 = false;
                    }

                    if (picked1)
                    {
                        if (picked2)
                        {
                            if (needToReOrder(element1, element2) == false)
                            {
                                if (picked3)
                                {
                                    if (needToReOrder(element1, element3) == false)
                                    {
                                        Thread.Sleep(time);
                                        Console.WriteLine($"Добовляем {element1} из файла \"{a}.txt\" в файл \"{curFile}.txt\".");
                                        WriteToFile(writer1, writer2, writer3, element1);
                                        picked1 = false;
                                    }
                                    else
                                    {
                                        Thread.Sleep(time);
                                        Console.WriteLine($"Добовляем {element3} из файла \"{a}.txt\" в файл \"{curFile}.txt\".");
                                        WriteToFile(writer1, writer2, writer3, element3);
                                        picked3 = false;
                                    }
                                }
                                else
                                {
                                    Thread.Sleep(time);
                                    Console.WriteLine($"Добовляем {element1} из файла \"{a}.txt\" в файл \"{curFile}.txt\".");
                                    WriteToFile(writer1, writer2, writer3, element1);
                                    picked1 = false;
                                }
                            }
                            else
                            {
                                if (picked3)
                                {
                                    if (needToReOrder(element2, element3) == false)
                                    {
                                        Thread.Sleep(time);
                                        Console.WriteLine($"Добовляем {element2} из файла \"{a}.txt\" в файл \"{curFile}.txt\".");
                                        WriteToFile(writer1, writer2, writer3, element2);
                                        picked2 = false;
                                    }
                                    else
                                    {
                                        Thread.Sleep(time);
                                        Console.WriteLine($"Добовляем {element3} из файла \"{a}.txt\" в файл \"{curFile}.txt\".");
                                        WriteToFile(writer1, writer2, writer3, element3);
                                        picked3 = false;
                                    }
                                }
                                else
                                {
                                    Thread.Sleep(time);
                                    Console.WriteLine($"Добовляем {element2} из файла \"{a}.txt\" в файл \"{curFile}.txt\".");
                                    WriteToFile(writer1, writer2, writer3, element2);
                                    picked2 = false;
                                }
                            }
                        }
                        else if (picked3)
                        {
                            if (needToReOrder(element1, element3) == false)
                            {
                                Thread.Sleep(time);
                                Console.WriteLine($"Добовляем {element1} из файла \"{a}.txt\" в файл \"{curFile}.txt\".");
                                WriteToFile(writer1, writer2, writer3, element1);
                                picked1 = false;
                            }
                            else
                            {
                                Thread.Sleep(time);
                                Console.WriteLine($"Добовляем {element3} из файла \"{c}.txt\" в файл \"{curFile}.txt\".");
                                WriteToFile(writer1, writer2, writer3, element3);
                                picked3 = false;
                            }
                        }
                        else
                        {
                            Thread.Sleep(time);
                            Console.WriteLine($"Добовляем {element1} из файла \"{a}.txt\" в файл \"{curFile}.txt\".");
                            WriteToFile(writer1, writer2, writer3, element1);
                            picked1 = false;
                        }
                    }
                    else if (picked2)
                    {
                        if (picked3)
                        {
                            if (needToReOrder(element2, element3) == false)
                            {
                                Thread.Sleep(time);
                                Console.WriteLine($"Добовляем {element2} из файла \"{b}.txt\" в файл \"{curFile}.txt\".");
                                WriteToFile(writer1, writer2, writer3, element2);
                                picked2 = false;
                            }
                            else
                            {
                                Thread.Sleep(time);
                                Console.WriteLine($"Добовляем {element3} из файла \"{c}.txt\" в файл \"{curFile}.txt\".");
                                WriteToFile(writer1, writer2, writer3, element3);
                                picked3 = false;
                            }
                        }
                        else
                        {
                            Thread.Sleep(time);
                            Console.WriteLine($"Добовляем {element2} из файла \"{b}.txt\" в файл \"{curFile}.txt\".");
                            WriteToFile(writer1, writer2, writer3, element2);
                            picked2 = false;
                        }
                    }
                    else if (picked3)
                    {
                        Thread.Sleep(time);
                        Console.WriteLine($"Добовляем {element3} из файла \"{b}.txt\" в файл \"{curFile}.txt\".");
                        WriteToFile(writer1, writer2, writer3, element3);
                        picked3 = false;
                    }
                }
                Thread.Sleep(time);
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        private int WriteToFile(StreamWriter writer1, StreamWriter writer2, StreamWriter writer3, string element)
        {
            if (segments % 3 == 1)
            {
                writer1.WriteLine(element);
                return 1;
            }
            else if (segments % 3 == 2)
            {
                writer2.WriteLine(element);
                return 2;
            }
            else
            {
                writer3.WriteLine(element);
                return 3;
            }
        }
    }
}
