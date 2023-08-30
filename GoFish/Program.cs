using System;
using System.Collections.Generic;
using System.Linq;

namespace GoFish_WPF
{
    class Program
    {
        static GameController gameController;
        static void Main(string[] args)
        {
            Console.Write("Введите своё имя: ");
            string humanPlayerName = Console.ReadLine();
            Console.Write("Введите число компьютерных противников от 1 до 5: ");
            if (int.TryParse(Console.ReadLine(), out int opponentsCount) && opponentsCount <= 5 && opponentsCount >= 1)
            {
                Console.WriteLine($"Добро пожаловать в игру, {humanPlayerName}");
                var opponentsNames = new List<string>() { };
                for (int i = 0; i < opponentsCount; i++)
                    opponentsNames.Add($"Компьютер #{i + 1}");
                gameController = new GameController(humanPlayerName, opponentsNames);
                Console.WriteLine(gameController.Status);
                while (true)
                {
                    Values valueToAskFor = PromptForAValue();
                    while (!gameController.HumanPlayer.Hand.Select(card => card.Value).Contains(valueToAskFor))
                    {
                        Console.WriteLine("У вас нет карты такого номинала. Введите номинал карты, которая у вас есть");
                        valueToAskFor = PromptForAValue();
                    }
                    Player opponentToAsk = PromptForAnOpponent();
                    Console.WriteLine();
                    gameController.NextRound(opponentToAsk, valueToAskFor);
                    Console.WriteLine(gameController.Status);
                    if (gameController.GameOver)
                    {
                        Console.WriteLine("Введите Q, чтобы выйти, любую другую клавишу, чтобы начать новую игру");
                        switch (Console.ReadKey(true).KeyChar.ToString().ToUpper())
                        {
                            case "Q":
                                return;
                            default:
                                gameController.NewGame();
                                break;
                        }
                    }
                }
            }
            Console.WriteLine("Выхожу...");
        }

        static Values PromptForAValue()
        {
            Console.WriteLine();
            Console.Write("У вас на руках:\n");
            foreach (Card card in gameController.HumanPlayer.Hand)
                Console.WriteLine(card);
            Console.Write("Карты какого номинала вы хотите попросить? ");
            var line = Console.ReadLine();
            if (Enum.TryParse<Values>(line, true, out Values value))
                return value;
            else
            {
                Values randomValue = gameController.HumanPlayer.RandomValueFromHand();
                Console.WriteLine($"Неправильный ввод, номинал случайной вашей карты: {randomValue}");
                return randomValue;
            }
        }

        static Player PromptForAnOpponent()
        {
            Console.WriteLine();
            for (int i = 0; i < gameController.Opponents.Count(); i++)
                Console.WriteLine($"{i + 1}. {gameController.Opponents.ToList()[i]}");
            Console.Write("У кого вы собираетесь просить карты? ");
            if (!int.TryParse(Console.ReadLine(), out int index) || index > gameController.Opponents.Count())
            {
                var randomPlayer = gameController.Opponents.ToList()[Player.Random.Next(gameController.Opponents.Count())];
                Console.WriteLine($"Неправильный ввод, случайный игрок: {randomPlayer}");
                return randomPlayer;
            }
            return gameController.Opponents.ToList()[index - 1];
        }
    }
}
