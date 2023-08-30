using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace GoFish_WPF
{
    public class GameController
    {
        private GameState gameState;
        public bool GameOver { get { return gameState.GameOver; } }
        public Player HumanPlayer { get { return gameState.HumanPlayer; } }
        public IEnumerable<Player> Opponents { get { return gameState.Opponents; } }
        public string Status { get; private set; }

        public GameController(string humanPlayerName, IEnumerable<string> computerPlayerNames)
        {
            var stock = new Deck();
            stock.Shuffle();
            gameState = new GameState(humanPlayerName, computerPlayerNames, stock);
            Status = $"Начинаем новую игру с игроками: {string.Join(", ", gameState.Players)}";
        }

        public void NextRound(Player playerToAsk, Values valueToAskFor)
        {
            Status = gameState.PlayRound(HumanPlayer, playerToAsk, valueToAskFor, gameState.Stock) + Environment.NewLine;
            ComputerPlayersPlayNextRound();
            foreach (Player player in gameState.Players)
                Status += player.Status + Environment.NewLine;
            Status += $"В колоде {gameState.Stock.Count()} карт{Player.S(gameState.Stock.Count())}{Environment.NewLine}";
            Status += gameState.CheckForWinner();
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
        }
    }
}
