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
        /// Возвращает текущий статус игрока: количество его карт и книг
        /// </summary>
        public string Status => $"У {Name} {hand.Count()} карт{S(hand.Count())} и {books.Count()} четвёр{S2(books.Count())} карт одного достоинства";

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
            string s = value switch
            {
                Values.Туз => "Тузов",
                Values.Двойка => "Двоек",
                Values.Тройка => "Троек",
                Values.Четвёрка => "Четвёрок",
                Values.Пятёрка => "Пятёрок",
                Values.Шестёрка => "Шестёрок",
                Values.Семёрка => "Семёрок",
                Values.Восьмёрка => "Восьмёрок",
                Values.Девятка => "Девяток",
                Values.Десятка => "Десяток",
                Values.Валет => "Вальтов",
                Values.Королева => "Королев",
                Values.Король => "Королей",
                _ => ""
            };
            return s;
        }
        public static string S4(int count, Values value)
        {
            if (count == 1)
                return value.ToString();
            string s = value switch
            {
                Values.Туз => count >= 5 ? S3(value) : "Туза",
                Values.Двойка => count >= 5 ? S3(value) : "Двойки",
                Values.Тройка => count >= 5 ? S3(value) : "Тройки",
                Values.Четвёрка => count >= 5 ? S3(value) : "Четвёрки",
                Values.Пятёрка => count >= 5 ? S3(value) : "Пятёрки",
                Values.Шестёрка => count >= 5 ? S3(value) : "Шестёрки",
                Values.Семёрка => count >= 5 ? S3(value) : "Семёрки",
                Values.Восьмёрка => count >= 5 ? S3(value) : "Восьмёрки",
                Values.Девятка => count >= 5 ? S3(value) : "Девятки",
                Values.Десятка => count >= 5 ? S3(value) : "Десятки",
                Values.Валет => count >= 5 ? S3(value) : "Вальта",
                Values.Королева => count >= 5 ? S3(value) : "Королевы",
                Values.Король => count >= 5 ? S3(value) : "Короля",
                _ => ""
            };
            return s;
        }
    }
}
