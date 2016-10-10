using System;
using static Rank_Generator.RankGenerator;

namespace RankGenerator.Examples
{
    public class MergeGenerators
    {
        public static void Do()
        {
            var myGenerator = Initialize(@"..\..\Sample Data\\fable.json");
            var anotherGenerator = Initialize(@"..\..\Sample Data\\politics.json");
            
            myGenerator.Merge(anotherGenerator);

            Console.WriteLine("\nThe Fable and Politics dictionaries have been merged." +
                              "\nPress Enter to generate random ranks.\n");

            while (!myGenerator.IsExhausted())
            {
                Console.WriteLine(myGenerator.RandomRank());
                Console.ReadKey();
            }
        }
    }
}
