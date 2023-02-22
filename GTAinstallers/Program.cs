using System;
using System.Diagnostics;
using System.Threading;

namespace RandomWebsiteOpener
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define an array of websites to choose from
            string[] websites = { "https://www.google.com", "https://www.microsoft.com", "https://www.apple.com", "https://www.amazon.com", "https://www.facebook.com" };

            while (true)
            {
                // Get the number of available processor cores
                int numThreads = Environment.ProcessorCount;

                // Split the websites array into smaller arrays to be opened by each thread
                string[][] websiteGroups = SplitArray(websites, numThreads);

                // Start a new thread for each website group
                Thread[] threads = new Thread[numThreads];
                for (int i = 0; i < numThreads; i++)
                {
                    threads[i] = new Thread(() =>
                    {
                        // Open each website in the current group
                        foreach (string website in websiteGroups[i])
                        {
                            Process.Start(website);
                        }
                    });
                    threads[i].Start();
                }

                // Wait for all threads to complete
                foreach (Thread thread in threads)
                {
                    thread.Join();
                }
            }
        }

        static T[][] SplitArray<T>(T[] array, int numGroups)
        {
            // Split the array into smaller arrays of roughly equal size
            int groupSize = (int)Math.Ceiling((double)array.Length / numGroups);
            T[][] groups = new T[numGroups][];
            for (int i = 0; i < numGroups; i++)
            {
                int startIndex = i * groupSize;
                int length = Math.Min(groupSize, array.Length - startIndex);
                T[] group = new T[length];
                Array.Copy(array, startIndex, group, 0, length);
                groups[i] = group;
            }
            return groups;
        }
    }
}
