using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.ComponentModel;

namespace GoFish_WPF
{
    public class GameController: INotifyPropertyChanged
    {
        private GameState gameState;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public bool GameOver { get { return gameState.GameOver; } }
        public Player HumanPlayer { get { return gameState.HumanPlayer; } }
        public IEnumerable<Player> Opponents { get { return gameState.Opponents; } }
        private string status;
        public string Status { get { return status; } private set { OnPropertyChanged("Status"); status = value; } }
        public string BooksStatus { get; private set; }

        public GameController()
        {
            gameState = new GameState("Человек", new List<string>() { "Компьютер #1" }, new Deck());
            
        }

        public GameController(string humanPlayerName, IEnumerable<string> computerPlayerNames)
        {
            var stock = new Deck();
            stock.Shuffle();
            gameState = new GameState(humanPlayerName, computerPlayerNames, stock);
            OnPropertyChanged("HumanPlayer");
            OnPropertyChanged("Opponents");
            Status = $"Начинаем новую игру с игроками: {string.Join(", ", gameState.Players)}";
        }

        public void NextRound(Player playerToAsk, Values valueToAskFor)
        {
            BooksStatus = "";
            Status = gameState.PlayRound(HumanPlayer, playerToAsk, valueToAskFor, gameState.Stock) + Environment.NewLine;
            ComputerPlayersPlayNextRound();
            foreach (Player player in gameState.Players)
            {
                foreach (var book in player.Books)
                {
                    if (player == HumanPlayer)
                        BooksStatus += $"Вы собрали карты {Player.S3(book)} всех мастей\n";
                    else
                        BooksStatus += $"Игрок {player.Name} собрал карты {Player.S3(book)} всех мастей\n";
                }
            }
            Status += $"В колоде {gameState.Stock.Count()} карт{Player.S(gameState.Stock.Count())}{Environment.NewLine}";
            Status += gameState.CheckForWinner();
            OnPropertyChanged("HumanPlayer");
            OnPropertyChanged("BooksStatus");
        }

        private void ComputerPlayersPlayNextRound()
        {
            IEnumerable<Player> opponentsWithCards;
            if (HumanPlayer.Hand.Count() == 0)
                gameState.Stock.Clear();
            do
            {
                opponentsWithCards = gameState
                    .Opponents
                    .Where(player => player.Hand.Count() > 0);
                foreach (var player in opponentsWithCards)
                {
                    Status += gameState.PlayRound(player, gameState.RandomPlayer(player), player.RandomValueFromHand(), gameState.Stock) + Environment.NewLine;
                }
            } while ((opponentsWithCards.Count() > 0) && (HumanPlayer.Hand.Count() == 0));
            
        }
        
        public void NewGame()
        {
            Status = "Начинаем новую игру";
            var stock = new Deck();
            stock.Shuffle();
            gameState = new GameState(HumanPlayer.ToString(), Opponents.Select(player => player.ToString()).ToList(), stock);
            OnPropertyChanged("HumanPlayer");
            OnPropertyChanged("Opponents");
            BooksStatus = "";
            OnPropertyChanged("BooksStatus");
            OnPropertyChanged("Status");
        }
    }
}
