using System;
using static Rank_Generator.RankGenerator;

namespace RankGenerator.Examples
{
    public class PrintSequentialRanks
    {
        public static void Do()
        {
            var ranks = Initialize(@"..\..\Sample Data\\military.json");

            Console.WriteLine("\nPress Enter to generate sequential ranks from the Military dictionary.\n");

            while (!ranks.IsExhausted())
            {
                Console.WriteLine(ranks.NextRank());
                Console.ReadKey();
            }
        }
    }
}
