using System;

namespace RankGenerator.Examples
{
    public class Controller
    {
        public static void Main(string[] args)
        {
            Console.Title = "Press CTRL + C to quit.";

            while (true)
            {
                Console.WriteLine("Enter a number corresponding to a feature demonstration:"
                    + "\n 1. Print Random Ranks"
                    + "\n 2. Print Sequential Ranks"
                    + "\n 3. Merge Generators"
                    + "\n");

                switch (Console.ReadLine())
                {
                    case "1":
                        PrintRandomRanks.Do();
                        break;
                    case "2":
                        PrintSequentialRanks.Do();
                        break;
                    case "3":
                        MergeGenerators.Do();
                        break;
                    default:
                        return;
                }
            }
        }
    }
}
