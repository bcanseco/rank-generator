using System;

namespace Rank_Generator.Classes
{
    public class Rank : IComparable<Rank>, IEquatable<Rank>
    {
        private readonly int _tier; // Overall rank tier is Title.Tier + Prefix.Tier

        public Word Prefix { get; set; }
        public Word Title { get; set; }
        public Word Postfix { get; set; }

        public ERankFormat RankFormat
        {
            get
            {
                if (Prefix != null && Title != null && Postfix != null)
                {
                    return ERankFormat.Full;
                }

                if (Prefix != null && Title != null)
                {
                    return ERankFormat.PrefixAndTitle;
                }

                if (Title != null && Postfix != null)
                {
                    return ERankFormat.TitleAndPostfix;
                }

                return ERankFormat.TitleOnly;
            }
        }

        public Rank(Word title, Word prefix = null, Word postfix = null)
        {
            Title = title;
            Prefix = prefix;
            Postfix = postfix;

            _tier = Title.Tier;
            if (Prefix != null) _tier += Prefix.Tier;
        }

        public static implicit operator string(Rank rank)
        {
            return rank?.ToString() ?? "";
        }

        public override string ToString()
        {
            var printPrefix = Prefix == null ? "" : Prefix + " ";
            var printPostfix = Postfix == null ? "" : " " + Postfix;

            return printPrefix + Title + printPostfix;
        }

        public bool Equals(Rank rank)
        {
            if (rank == null) throw new ArgumentNullException();

            if (RankFormat != rank.RankFormat) return false;

            switch (RankFormat)
            {
                case ERankFormat.Full:
                    return Prefix.Equals(rank.Prefix) && Title.Equals(rank.Title) && Postfix.Equals(rank.Postfix);
                case ERankFormat.PrefixAndTitle:
                    return Prefix.Equals(rank.Prefix) && Title.Equals(rank.Title);
                case ERankFormat.TitleAndPostfix:
                    return Title.Equals(rank.Title) && Postfix.Equals(rank.Postfix);
                default: // ERankFormat.TitleOnly
                    return Title.Equals(rank.Title);
            }
        }

        public int CompareTo(Rank rank)
        {
            if (_tier > rank._tier) return 1;
            if (_tier == rank._tier) return 0;
            return -1;
        }
    }

    public enum ERankFormat
    {
        TitleOnly = 1,
        PrefixAndTitle = 2,
        TitleAndPostfix = 3,
        Full = 4
    }
}
