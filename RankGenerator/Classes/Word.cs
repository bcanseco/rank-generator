using System;
using System.Collections.Generic;

namespace Rank_Generator.Classes
{
    public class Word : IEquatable<Word>
    {
        public string Phrase { get; set; }
        public IList<string> Categories { get; set; }
        public IList<string> Whitelist { get; set; }
        public IList<string> Blacklist { get; set; }
        public int Tier { get; set; }
        public int? MinimumTier { get; set; }
        public int? MaximumTier { get; set; }
        public bool RestrictCategories { get; set; }

        public static implicit operator string(Word word)
        {
            return word.ToString();
        }

        public override string ToString()
        {
            return Phrase;
        }

        public bool Equals(Word word)
        {
            if (word == null) throw new ArgumentNullException();

            return Phrase.Equals(word.Phrase);
        }
    }
}
