using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFish_WPF
{
    public class Deck: ObservableCollection<Card>
    {
        private static Random random = Player.Random;

        public Deck()
        {
            Reset();
        }

        public void Reset()
        {
            Clear();
            for (int suit = 0; suit < 4; suit++)
                for (int value = 1; value < 14; value++)
                    Add(new Card((Values)value, (Suits)suit));
        }
        public Card Deal(int index)
        {
            Card card = base[index];
            RemoveAt(index);
            return card;
        }
        public void Shuffle()
        {
            List<Card> deckCopy = new List<Card>(this);
            Clear();
            while (deckCopy.Count > 0)
            {
                int index = random.Next(deckCopy.Count);
                Card card = deckCopy[index];
                deckCopy.RemoveAt(index);
                Add(card);
            }
        }
        public void Sort()
        {
            List<Card> sortedCards = new List<Card>(this);
            sortedCards.Sort(new CardComparerByValue());
            Clear();
            foreach (Card card in sortedCards)
            {
                Add(card);
            }
        }
    }
}
