using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoreLinq;
using Newtonsoft.Json;
using Rank_Generator.Classes;

namespace Rank_Generator
{
    public class RankGenerator
    {
        private readonly Random _rand;
        private readonly IList<Rank> _history;
        private bool _exhausted;

        public IList<Word> Prefixes { get; set; }
        public IList<Word> Titles { get; set; }
        public IList<Word> Postfixes { get; set; }

        private RankGenerator()
        {
            /* Must use RankList.Initialize() to create instance */
            _rand = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            _history = new List<Rank>();
        }

        /// <summary>
        /// Constructs a new RankGenerator object from a JSON file of phrases.
        /// </summary>
        /// <param name="rankPath">The path of the JSON file to use.</param>
        /// <returns>A RankGenerator object.</returns>
        /// <exception cref="DirectoryNotFoundException">Thrown when the path is not found.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the JSON file is not found.</exception>
        public static RankGenerator Initialize(string rankPath)
        {
            using (var file = File.OpenText(rankPath))
            {
                var serializer = new JsonSerializer();
                return (RankGenerator)serializer.Deserialize(file, typeof(RankGenerator));
            }
        }

        /// <summary>
        /// Adds the phrases and history from another Rank Generator.
        /// </summary>
        /// <param name="rankGenerator">The RankGenerator to use.</param>
        public void Merge(RankGenerator rankGenerator)
        {
            ((List<Rank>)_history).AddRange(rankGenerator._history);

            ((List<Word>)Prefixes).AddRange(rankGenerator.Prefixes);
            ((List<Word>)Titles).AddRange(rankGenerator.Titles);
            ((List<Word>)Postfixes).AddRange(rankGenerator.Postfixes);
        }

        /// <summary>
        /// Checks whether all possible ranks have been generated.
        /// </summary>
        /// <returns>True if no more ranks can be generated, False otherwise.</returns>
        public bool IsExhausted()
        {
            return _exhausted;
        }

        /// <summary>
        /// Generates the lowest possible tiered rank that hasn't been generated yet.
        /// </summary>
        /// <param name="withPostfix">Optional param dictating whether to include a postfix or not.</param>
        /// <returns>The generated Rank object.</returns>
        public Rank NextRank(bool withPostfix = false)
        {
            Rank nextRank;

            if (!_history.Any()) nextRank = LowestRankFrom(UnusedTitles());
            else
            {
                // Grab the last generated rank.
                var latestRank = _history.ElementAt(_history.Count - 1);

                // Grab lists of valid prefixes that can be paired with the last generated rank's title.
                var negativePrefixes = UnusedPrefixesFor(latestRank.Title).Where(prefix => prefix.Tier < 0);
                var positivePrefixes = UnusedPrefixesFor(latestRank.Title).Where(prefix => prefix.Tier > 0);

                if (negativePrefixes.Any())
                {
                    // Pair the title and the negative prefix with the smallest tier.
                    nextRank = new Rank(latestRank.Title,
                        prefix: negativePrefixes.MinBy(prefix => prefix.Tier));
                }
                else if (latestRank.Prefix != null && latestRank.Prefix.Tier < 0)
                {
                    // Use the title on its own.
                    nextRank = new Rank(latestRank.Title);
                }
                else if (positivePrefixes.Any())
                {
                    // Pair the title and the positive prefix with the smallest tier.
                    nextRank = new Rank(latestRank.Title,
                        prefix: positivePrefixes.MinBy(prefix => prefix.Tier));
                }
                else nextRank = LowestRankFrom(UnusedTitles()); // Use a new title altogether.
            }

            if (nextRank != null)
            {
                if (withPostfix) nextRank.Postfix = RandomPostfixFor(nextRank.Title);
                _history.Add(nextRank);
            }
            else _exhausted = true;

            return nextRank;
        }

        /// <summary>
        /// Generates a random rank (Full, Title only, Title and Prefix, or Title and Postfix).
        /// </summary>
        /// <returns>The generated Rank object.</returns>
        public Rank RandomRank()
        {
            var title = RandomTitle();

            switch (_rand.Next(4))
            {
                case 0: return new Rank(title);
                case 1: return new Rank(title, prefix: RandomPrefixFor(title));
                case 2: return new Rank(title, postfix: RandomPostfixFor(title));
                case 3: return new Rank(title, RandomPrefixFor(title), RandomPostfixFor(title));
                default: return null; // Impossible branch
            }
        }

        /// <summary>
        /// Generates either a Title-only or Title + Prefix Rank with the lowest tier possible.
        /// </summary>
        /// <param name="titles">The collection of titles to use.</param>
        /// <returns>The generated Rank object, or null if none could be generated.</returns>
        private Rank LowestRankFrom(IEnumerable<Word> titles)
        {
            if (!titles.Any()) return null;
            var nextTitle = titles.MinBy(title => title.Tier);

            // We don't check positively tiered prefixes because a title on its own is always tiered lower.
            var negativePrefixes = UnusedPrefixesFor(nextTitle).Where(prefix => prefix.Tier < 0);

            return !negativePrefixes.Any() ? new Rank(nextTitle) : new Rank(nextTitle, prefix: negativePrefixes.MinBy(prefix => prefix.Tier));
        }

        /// <summary>
        ///  Gets a random title from the Titles collection.
        /// </summary>
        /// <returns>The retrieved Word object.</returns>
        private Word RandomTitle()
        {
            return Titles[_rand.Next(Titles.Count)];
        }

        /// <summary>
        /// Gets a valid prefix for a title.
        /// </summary>
        /// <returns>The retrieved Word object.</returns>
        private Word RandomPrefixFor(Word title)
        {
            return RandomAffixFor(title, true);
        }

        /// <summary>
        /// Gets a valid postfix for a title.
        /// </summary>
        /// <returns>The retrieved Word object.</returns>
        private Word RandomPostfixFor(Word title)
        {
            return RandomAffixFor(title, false);
        }

        /// <summary>
        /// Gets a valid affix for a title.
        /// </summary>
        /// <returns>The retrieved Word object, or null if no valid affixes were found.</returns>
        private Word RandomAffixFor(Word title, bool location)
        {
            IList<Word> glossary = location ? Prefixes : Postfixes;

            var validAffixes = AffixesFor(title, glossary);

            if (!validAffixes.Any()) return null;
            else return validAffixes.ElementAt(_rand.Next(validAffixes.Count()));
        }

        /// <summary>
        /// Retrieves a collection of Word objects encapsulating unused Titles.
        /// </summary>
        /// <returns>The retrieved collection of Word objects.</returns>
        private IEnumerable<Word> UnusedTitles()
        {
            return
                from title in Titles
                where _history.All(rank => rank.Title != title)
                select title;
        }

        /// <summary>
        /// Retrieves a collection of valid prefixes that haven't been paired with a given title.
        /// </summary>
        /// <param name="title">The Word object to use for the title.</param>
        /// <returns>The retrieved collection of Word objects.</returns>
        private IEnumerable<Word> UnusedPrefixesFor(Word title)
        {
            return
                from prefix in AffixesFor(title, Prefixes)
                where !_history.Any(rank => rank.Prefix == prefix && rank.Title == title)
                select prefix;
        }

        /// <summary>
        /// Retrieves a collection of valid affixes for a title.
        /// </summary>
        /// <param name="title">The Word object encapsulating the title.</param>
        /// <param name="glossary">The glossary to use (Prefixes / Postfixes).</param>
        /// <returns>The retrieved collection of Word objects.</returns>
        private IEnumerable<Word> AffixesFor(Word title, IList<Word> glossary)
        {
            return
                from affix in glossary
                where affix.MinimumTier == null || title.Tier >= affix.MinimumTier
                where affix.MaximumTier == null || title.Tier <= affix.MaximumTier
                where affix.Whitelist == null || affix.Whitelist.Contains(title)
                where affix.Blacklist == null || !affix.Blacklist.Contains(title)
                where !affix.RestrictCategories || (affix.Categories.Intersect(title.Categories).Any())
                select affix;
        }
    }
}
