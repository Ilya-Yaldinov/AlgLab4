namespace AlgLab4
{
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
}