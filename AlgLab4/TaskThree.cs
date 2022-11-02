using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgLab4
{
    public static class TaskThree
    {
        private const string path = "Alice.txt";
        private static char[] chars = { '!', '?', ',', '.', '@', '/', '*', '+', '-',' ','\r','\n','\'','\"',';',':','-','(',')', '—' };
        private static StringBuilder text = new StringBuilder();
        private static List<string> words = File.ReadAllText($"{path}").Split(chars, StringSplitOptions.RemoveEmptyEntries).ToList();


        public static void DefaultSort()
        {
            for (int i = 0; i < words.Count; i++)
            {
                for (int j = 0; j < words.Count - 1; j++)
                {
                    CompareStrings(j,j + 1);
                }
            }

            CountUnique();
        }

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

            CountUnique();
        }

        private static void CountUnique()
        {
            Dictionary<string, int> countContains = new Dictionary<string, int>();
            foreach (string word in words)
                if (countContains.ContainsKey(word))
                    countContains[word]++;
                else
                    countContains.Add(word, 1);

            Console.WriteLine();

            foreach (string word in words)
                Console.WriteLine(word);

            Console.WriteLine();

            foreach (var dWord in countContains)
                Console.WriteLine($"{dWord.Key} {dWord.Value}");
        }

        private static bool CompareStrings(int one, int two)
        {
            int minLength = Math.Min(words[one].Length, words[two].Length);

            for (int wordChar = 0; wordChar < minLength; wordChar++)
            {
                if (words[one][wordChar] > words[two][wordChar])
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
    }
}
