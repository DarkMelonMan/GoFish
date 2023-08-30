using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace GoFish_WPF
{
    public class Player
    {
        public static Random Random = new Random();

        private List<Card> hand = new List<Card>();
        private List<Values> books = new List<Values>();

        /// <summary>
        /// Карты у игрока на руках
        /// </summary>
        public IEnumerable<Card> Hand => hand;

        /// <summary>
        /// Книги, которые игрок собрал
        /// </summary>
        public IEnumerable<Values> Books => books;

        public readonly string Name;

        /// <summary>
        /// Добавляет слову множественное число, если параметр не равен 1
        /// </summary>
        public static string S(int s) => s == 1 ? "а" : s == 0 ? "" : s <= 4 ? "ы" : "";
        public static string S2(int s) => s == 1 ? "ка" : s == 0 ? "ок" : s <= 4 ? "ки" : "ок";
      
        /// <summary>
        /// Конструктор чтобы создать игрока
        /// </summary>
        /// <param name="name">Имя созданного игрока</param>
        public Player(string name) => Name = name;

        /// <summary>
        /// Альтернативный конструктор для создания игрока
        /// </summary>
        /// <param name="name">Имя созданного игрока</param>
        /// <param name="cards">Выданные игроку карты</param>
        public Player(string name, IEnumerable<Card> cards)
        {
            Name = name;
            hand.AddRange(cards);
        }

        /// <summary>
        /// Получает 5 карт из стопки
        /// </summary>
        /// <param name="stock">Стопка, чтобы собрать следующую руку</param>
        public void GetNextHand(Deck stock)
        {
            while ((stock.Count() > 0) && (hand.Count < 5))
            {
                hand.Add(stock.Deal(0));
            }
        }

        /// <summary>
        /// Если у меня есть карты, подходящие по значению, вернуть их.
        /// Если у меня закончились карты, собрать новую руку.
        /// </summary>
        /// <param name="value">Значение, о котором меня спросили</param>
        /// <param name="deck">Стопка, из которой я соберу новую руку</param>
        /// <returns>Карты, которые вытянули из руки игрока</returns>
        public IEnumerable<Card> DoYouHaveAny(Values value, Deck deck)
        {
            var cardsToGive = hand.Where(card => card.Value == value).OrderBy(card => card.Suit);
            hand = hand.Where(card => card.Value != value).ToList();
            if (hand.Count() == 0)
                GetNextHand(deck);
            return cardsToGive;
        }

        /// <summary>
        /// Когда игрок получает карты от другого игрока, добавляет их к своей руке
        /// и вытягивает собранные книги
        /// </summary>
        /// <param name="cards"></param>
        public void AddCardsAndPullOutBooks(IEnumerable<Card> cards)
        {
            hand.AddRange(cards);
            var cardGroups =
                from card in hand
                group card by card.Value into cardGroup
                where cardGroup.Count() == 4
                select cardGroup.Key;
            books.AddRange(cardGroups);
            books.Sort();
            hand = hand.Where(card => !books.Contains(card.Value)).ToList();
        }

        /// <summary>
        /// Вытягивает карту из стопки и добавляет к руке игрока
        /// </summary>
        /// <param name="stock"></param>
        public void DrawCard(Deck stock)
        {
            if (stock.Count > 0)
                AddCardsAndPullOutBooks(new List<Card> { stock.Deal(0) });
        }

        /// <summary>
        /// Получает случайное значение с руки игрока
        /// </summary>
        /// <returns>Значение случайно выбранной карты с руки игрока</returns>
        public Values RandomValueFromHand() => hand.OrderBy(card => card.Value)
            .Select(card => card.Value)
            .Skip(Random.Next(hand.Count()))
            .First();

        public override string ToString() => Name;  

        public static string S3(Values value)
        {
            string s; 
            switch (value)
            {
                case Values.Туз: s = "Тузов"; break;
                case Values.Двойка:
                    s = "Двоек"; break;
                case Values.Тройка:
                    s = "Троек"; break;
                case Values.Четвёрка:
                    s = "Четвёрок"; break;
                case Values.Пятёрка:
                    s = "Пятёрок"; break;
                case Values.Шестёрка:
                    s = "Шестёрок"; break;
                case Values.Семёрка:
                    s = "Семёрок"; break;
                case Values.Восьмёрка:
                    s = "Восьмёрок"; break;
                case Values.Девятка:
                    s = "Девяток"; break;
                case Values.Десятка:
                    s = "Десяток"; break;
                case Values.Валет:
                    s = "Вальтов"; break;
                case Values.Королева:
                    s = "Королев"; break;
                case Values.Король:
                    s = "Королей"; break;
                default: s = ""; break;
            };
            return s;
        }
        public static string S4(int count, Values value)
        {
            if (count == 1)
                return value.ToString();
            string s;
            switch (value)
            {
                case Values.Туз: s = count >= 5 ? S3(value) : "Туза"; break;
                case Values.Двойка: s = count >= 5 ? S3(value) : "Двойки"; break;
                case Values.Тройка: s = count >= 5 ? S3(value) : "Тройки"; break;
                case Values.Четвёрка: s = count >= 5 ? S3(value) : "Четвёрки"; break;
                case Values.Пятёрка: s = count >= 5 ? S3(value) : "Пятёрки"; break;
                case Values.Шестёрка: s = count >= 5 ? S3(value) : "Шестёрки"; break;
                case Values.Семёрка: s = count >= 5 ? S3(value) : "Семёрки"; break;
                case Values.Восьмёрка: s = count >= 5 ? S3(value) : "Восьмёрки"; break;
                case Values.Девятка: s = count >= 5 ? S3(value) : "Девятки"; break;
                case Values.Десятка: s = count >= 5 ? S3(value) : "Десятки"; break;
                case Values.Валет: s = count >= 5 ? S3(value) : "Вальта"; break;
                case Values.Королева: s = count >= 5 ? S3(value) : "Королевы"; break;
                case Values.Король: s = count >= 5 ? S3(value) : "Короля"; break;
                default: s = ""; break;
            };
            return s;
        }
    }
}
