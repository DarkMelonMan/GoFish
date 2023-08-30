using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GoFish_WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string humanPlayerName;
        static int numberOfOpponents;
        static List<string> opponentsNames = new List<string>();
        private GameController gameController;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void FirstStageConfirmationButton_Click(object sender, RoutedEventArgs e)
        {
            if (EnterHumanPlayerName.Text != "")
            {
                humanPlayerName = EnterHumanPlayerName.Text;
                EnterHumanPlayerName.Visibility = Visibility.Hidden;
                FirstStageLabel.Visibility = Visibility.Hidden;
                FirstStageConfirmationButton.Visibility = Visibility.Hidden;
                SecondStageLabel.Visibility = Visibility.Visible;
                ChooseNumberOfOpponents.Visibility = Visibility.Visible;
                SecondStageConfirmationButton.Visibility = Visibility.Visible;
                Debug.WriteLine(humanPlayerName);
            }
        }

        private void SecondStageConfirmationButton_Click(object sender, RoutedEventArgs e)
        {
            numberOfOpponents = (int)ChooseNumberOfOpponents.Value;
            SecondStageLabel.Visibility = Visibility.Hidden;
            ChooseNumberOfOpponents.Visibility = Visibility.Hidden;
            SecondStageConfirmationButton.Visibility = Visibility.Hidden;
            OpponentsNames.Visibility = Visibility.Visible;
            ThirdStageAddButton.Visibility = Visibility.Visible;
            ThirdStageLabel.Visibility = Visibility.Visible;
            ThirdStageConfirmationButton.Visibility = Visibility.Visible;
            EditOpponentName.Visibility = Visibility.Visible;
            Debug.WriteLine(numberOfOpponents);
        }

        private void ThirdStageAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (EditOpponentName.Text != "" && OpponentsNames.Items.Count < numberOfOpponents)
            {
                OpponentsNames.Items.Add(EditOpponentName.Text);
                EditOpponentName.Text = "";
            }
        }

        private void ThirdStageConfirmationButton_Click(object sender, RoutedEventArgs e)
        {
            if (OpponentsNames.Items.Count == numberOfOpponents) {
                foreach (var item in OpponentsNames.Items)
                {
                    opponentsNames.Add(item.ToString());
                    Debug.WriteLine(item.ToString());
                }
                OpponentsNames.Visibility = Visibility.Hidden;
                ThirdStageAddButton.Visibility = Visibility.Hidden;
                ThirdStageLabel.Visibility = Visibility.Hidden;
                ThirdStageConfirmationButton.Visibility = Visibility.Hidden;
                EditOpponentName.Visibility = Visibility.Hidden;
                GameProgressLabel.Visibility = Visibility.Visible;
                HandLabel.Visibility = Visibility.Visible;
                BooksLabel.Visibility = Visibility.Visible;
                Resources.Add("gameController", new GameController(humanPlayerName, opponentsNames));
                gameController = Resources["gameController"] as GameController;
                grid.DataContext = Resources["gameController"];
                GameStatus.Visibility = Visibility.Visible;
                Books.Visibility = Visibility.Visible;
                Hand.Visibility = Visibility.Visible;
                AskForACard.Visibility = Visibility.Visible;
                Debug.WriteLine(gameController.Status);
            }
        }

        private void AskForACard_Click(object sender, RoutedEventArgs e)
        {
            ChoosePlayerToAskFor.Visibility = Visibility.Visible;
            ChooseButton.Visibility = Visibility.Visible;
            AskForACard.Visibility = Visibility.Hidden;
        }

        private void ChooseButton_Click(object sender, RoutedEventArgs e)
        {
            if ((ChoosePlayerToAskFor.SelectedItem != null) && (Hand.SelectedItem != null))
            {
                Values valueToAskFor = gameController.HumanPlayer.Hand.Where(card => card.Name == Hand.SelectedItem.ToString()).Select(card => card.Value).First();
                string chosenOpponent = ChoosePlayerToAskFor.SelectedItem.ToString();
                gameController.NextRound(gameController.Opponents
                    .Where(player => player.Name == chosenOpponent).First(), valueToAskFor);
                ChoosePlayerToAskFor.Visibility = Visibility.Hidden;
                ChooseButton.Visibility = Visibility.Hidden;
                AskForACard.Visibility = Visibility.Visible;
            }
            if (gameController.GameOver)
            {
                AskForACard.Visibility = Visibility.Hidden;
                NewGame.Visibility = Visibility.Visible;
                Finish.Visibility = Visibility.Visible;
                Hand.Visibility = Visibility.Hidden;
                EndGame.Visibility = Visibility.Visible;
                HandLabel.Visibility = Visibility.Hidden;
                if (gameController.Opponents.Append(gameController.HumanPlayer).Select(player => player.Books.Count()).Max() == gameController.HumanPlayer.Books.Count())
                {
                    EndGame.Text = "Поздравляем! Вы победили!\nНе хотите сыграть снова?";
                }
                else
                {
                    EndGame.Text = "Вы проиграли.\nНе хотите попробовать снова?";
                }
            }
        }

        private void NewGame_Checked(object sender, RoutedEventArgs e)
        {
            gameController.NewGame();
            HandLabel.Visibility = Visibility.Visible;
            Hand.Visibility = Visibility.Visible;
            EndGame.Visibility = Visibility.Hidden;
            AskForACard.Visibility = Visibility.Visible;
            NewGame.Visibility = Visibility.Hidden;
            Finish.Visibility = Visibility.Hidden;
            Debug.WriteLine(gameController.HumanPlayer.Hand.Count());
            Debug.WriteLine(gameController.Status);
            Debug.WriteLine(gameController.BooksStatus);
        }

        private void Finish_Checked(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
