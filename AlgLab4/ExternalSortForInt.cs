namespace AlgLab4
{
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
}