using System;
using System.Collections.Generic;
using System.Text;

namespace GoFish_WPF
{
    public class Card
    {
        public Suits Suit { get; private set; }
        public Values Value { get; private set; }
        public string Name { get { return $"{Value} {Suit}"; } }
        
        public Card(Values value, Suits suit)
        {
            Value = value;
            Suit = suit;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
