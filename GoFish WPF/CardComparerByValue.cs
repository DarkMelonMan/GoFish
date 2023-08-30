using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace GoFish_WPF
{
    public class CardComparerByValue : IComparer<Card>
    {
        public int Compare(Card x, Card y)
        {
            if (x.Suit > y.Suit) return 1;
            if (x.Suit < y.Suit) return -1;
            if (x.Value > y.Value) return 1;
            if (x.Value < y.Value) return -1;
            return 0;
        }
    }
}
