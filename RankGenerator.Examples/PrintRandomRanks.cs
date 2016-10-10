using System;
using static Rank_Generator.RankGenerator;

namespace RankGenerator.Examples
{
    public class PrintRandomRanks
    {
        public static void Do()
        {
            var ranks = Initialize(@"..\..\Sample Data\\politics.json");

            Console.WriteLine("\nPress Enter to generate random ranks from the Politics dictionary.\n");

            while (!ranks.IsExhausted())
            {
                Console.WriteLine(ranks.RandomRank());
                Console.ReadKey();
            }
        }
    }
}
