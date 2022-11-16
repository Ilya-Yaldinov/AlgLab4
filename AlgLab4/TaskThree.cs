using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgLab4
{
    public static class TaskThree
    {
        public static string path = "Alice.txt";
        private const string pathSortText = "SortText.txt";
        private const string pathCountUnique = "AliceUnique.txt";
        private static char[] chars = { '!', '?', ',', '.', '@', '/', '*', '+', '-',' ','\r','\n','\'','\"',';',':','-','(',')', '—','[',']'};
        private static StringBuilder text = new StringBuilder();
        public static string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static List<string> words = File.ReadAllText($"{path}").Split(chars, StringSplitOptions.RemoveEmptyEntries).ToList();
        private static Dictionary<string, int> countContains = new Dictionary<string, int>();

        public static void ShellSort()
        {
            int j;
            int step = words.Count / 2;
            while (step > 0)
            {
                for (int i = 0; i < (words.Count - step); i++)
                {
                    j = i;
                    while ((j >= 0) && (CompareStrings(j, j + step)))
                    {
                        j -= step;
                    }
                }
                step = step / 2;
            }
           File.WriteAllLines(pathSortText, words.ToArray());
           CountUnique();
        }

        private static void CountUnique()
        {
            foreach (string word in words)
                if (countContains.ContainsKey(word))
                    countContains[word]++;
                else
                    countContains.Add(word, 1);

            foreach (var dWord in countContains)
                Console.WriteLine($"{dWord.Key} {dWord.Value}");
        }

        public static void LSDSort()
        {
            int maxLength = words.Max(word => word.Length);
            var workingList = words;
            var tempResult = new List<string>();

            var alphaDict = new Dictionary<char, int>();
            for (int i = 0; i < alphabet.Length; i++)
                alphaDict.Add(alphabet[i], i + 1);

            // loop for each char index (starting at last - least significant - char) цикл для каждого индекса символа (начало с последнего - наименее значимого символа)						
            for (int charLoc = maxLength - 1; charLoc >= 0; charLoc--)
            {

                var queues = new Queue<string>[alphabet.Length + 1];
                for (int i = 0; i < alphabet.Length + 1; i++)
                    queues[i] = new Queue<string>();
                //помещаем разные строки в соответствующую очередь
                foreach (var str in workingList)
                {
                    int queueIndex = 0;
                    if (charLoc < str.Length)
                    {
                        char cr = str[charLoc];
                        queueIndex = alphaDict[cr];
                    }
                    queues[queueIndex].Enqueue(str);
                }
                //объединяем все очереди
                for (int queueIndex = 0; queueIndex <= alphabet.Length; queueIndex++)
                {
                    var queue = queues[queueIndex];
                    if (queue != null)
                    {

                        tempResult.AddRange(queue.ToArray());

                    }
                }
                workingList = tempResult;
                tempResult = new List<string>();
            }
            words = workingList;
            File.WriteAllLines(pathSortText, workingList.ToArray());
            CountUnique();
        }

        private static bool CompareStrings(int one, int two)
        {
            int minLength = Math.Min(words[one].Length, words[two].Length);
            for (int wordChar = 0; wordChar < minLength; wordChar++)
            {
                if ( (words[one][wordChar] > words[two][wordChar]) && char.IsUpper(words[one][wordChar]) && char.IsUpper(words[two][wordChar]) 
                    || (words[one][wordChar] > words[two][wordChar]) && char.IsLower(words[one][wordChar]) && char.IsLower(words[two][wordChar])
                    || char.IsUpper(words[one][wordChar]) && char.IsLower(words[two][wordChar])
                    || words[one][wordChar] == words[two][wordChar] && words[one].Length > words[two].Length && wordChar + 1 == minLength
                    ) 
                {
                    string tmp = words[one];
                    words[one] = words[two];
                    words[two] = tmp;
                    return true;
                }
                if (words[one][wordChar] < words[two][wordChar])
                {
                    return false;
                }
            }
            return false;
        }


        public static void Test()
        {
            double average = 0;
            List<string> TimeArr = new List<string>();


            TimeArr.Add("Array Size" + ";" + "Average Time (ms)");

            for (int i = 0; i < 25001; i += 500)
            {
                for (int j = 0; j < 5; j++)
                {
                    List<string> testText = TaskThree.words.Take(i).ToList();
                    //length = i;
                    //int[] arrayOne = RandomArray(i);

                    Stopwatch sw = Stopwatch.StartNew();
                    LSDSort();

                    sw.Stop();
                    average += sw.Elapsed.TotalMilliseconds;
                    sw.Reset();
                    //Console.WriteLine($"Время работы (мс) {sw.Elapsed.Milliseconds}");
                    //File.WriteAllText(@"..\\..\\..\\result6.json", JsonConvert.SerializeObject(result));
                }
                TimeArr.Add(i + ";" + (average / 5).ToString());
            }

            File.WriteAllLines(@"..\\..\\..\\LSDSort.csv", TimeArr);
            TimeArr.Clear();
        }
    }
}
