using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace GoFish_WPF
{
    public class GameState
    {
        public readonly IEnumerable<Player> Players;
        public readonly IEnumerable<Player> Opponents;
        public readonly Player HumanPlayer;
        public bool GameOver { get; private set; } = false;

        public readonly Deck Stock;

        /// <summary>
        /// Конструктор создаёт игроков и выдаёт им первые карты
        /// </summary>
        /// <param name="humanPlayerName">Имя игрока-человека</param>
        /// <param name="opponentsNames">Имена компьютерных игроков</param>
        /// <param name="stock">Перетасованная колода карт, из которых достают карты игроки</param>
        public GameState(string humanPlayerName, IEnumerable<string> opponentsNames, Deck stock)
        {
            Stock = stock;
            HumanPlayer = new Player(humanPlayerName);
            Opponents = opponentsNames.Select(name => new Player(name)).ToList();
            Players = new List<Player>() { HumanPlayer };
            HumanPlayer.GetNextHand(Stock);
            foreach (Player player in Opponents)
            {
                Players = Players.Append(player);
                player.GetNextHand(Stock);
            }
        }

        /// <summary>
        /// Получает случайного игрока, который не является текущим игроком
        /// </summary>
        /// <param name="currentPlayer">Текущий игрок</param>
        /// <returns>Случайный игрок, у которого текущий игрок может взять карты</returns>
        public Player RandomPlayer(Player currentPlayer) => Players
            .Where(player => player != currentPlayer)
            .Skip(Player.Random.Next(Players.Count() - 1))
            .First();

        /// <summary>
        /// Позволяет игроку сделать ход
        /// </summary>
        /// <param name="player">Игрок, который просит карты</param>
        /// <param name="playerToAsk">Игрок, у которого попросили взять карты</param>
        /// <param name="valueToAskFor">Значение, которое просит игрок у другого</param>
        /// <param name="stock">Колода, откуда можно вытянуть карты</param>
        /// <returns>Сообщение, которое описывает, что только что произошло</returns>
        public string PlayRound(Player player, Player playerToAsk, Values valueToAskFor, Deck stock)
        {
            var cardsToGive = playerToAsk.DoYouHaveAny(valueToAskFor, stock);
            string s = Player.S3(valueToAskFor);
            var message = $"{player.Name} попросил у игрока {playerToAsk.Name} {s}{Environment.NewLine}";
            if (cardsToGive.Count() > 0)
            {
                player.AddCardsAndPullOutBooks(cardsToGive);
                message += $"У игрока {playerToAsk.Name} есть {cardsToGive.Count()} {Player.S4(cardsToGive.Count(), valueToAskFor)}";
            }
            else if (Stock.Count == 0)
                message += "В колоде закончились карты";
            else
            {
                player.DrawCard(stock);
                message += $"{player.Name} вытягивает карту";
            }
            if (player.Hand.Count() == 0)
            {
                player.GetNextHand(stock);
                message += $"{Environment.NewLine}У игрока {player.Name} закончились карты, вытягивает {player.Hand.Count()} из колоды";
            }
            return message;
        }

        public string CheckForWinner()
        {
            var playerCards = Players.Select(player => player.Hand.Count()).Sum();
            if (playerCards > 0)
                return "";
            GameOver = true;
            var winningBookCount = Players.Select(player => player.Books.Count()).Max();
            var winners = Players
                .Where(player => player.Books.Count() == winningBookCount);
            if (winners.Count() == 1)
                return $"Победитель - {winners.First().Name}";
            return $"Победителями стали {string.Join(" и ", winners)}";
        }
    }
}
