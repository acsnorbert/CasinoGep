using System;
using System.Collections.ObjectModel;
using System.Numerics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace CasinoGep
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DatabaseHelper dbHelper;
        private ObservableCollection<Player> players;
        private ObservableCollection<Game> games;
        private Random random = new Random();
        private Player currentPlayer;
        public MainWindow()
        {
            InitializeComponent();

            dbHelper = new DatabaseHelper("casino.db");


            LoadData();

            currentPlayer = new Player { Id = 1, Name = "Játékos", Balance = 1000 };
            UpdateBalanceDisplay();
        }

        private void UpdateBalanceDisplay()
        {
            BalanceText.Text = $"Egyenleg: {currentPlayer.Balance}";
        }

        private void LoadData()
        {
            players = new ObservableCollection<Player>(dbHelper.GetAllPlayers());
            PlayersGrid.ItemsSource = players;

            games = new ObservableCollection<Game>(dbHelper.GetAllGames());
            GamesGrid.ItemsSource = games;
        }

        private void PlayersButton_Click(object sender, RoutedEventArgs e)
        {
            PlayersView.Visibility = Visibility.Visible;
            GamesView.Visibility = Visibility.Collapsed;
            SlotView.Visibility = Visibility.Collapsed;
        }

        private void GamesButton_Click(object sender, RoutedEventArgs e)
        {
            PlayersView.Visibility = Visibility.Collapsed;
            GamesView.Visibility = Visibility.Visible;
            SlotView.Visibility = Visibility.Collapsed;
        }
        private void SlotButton_Click(object sender, RoutedEventArgs e)
        {
            PlayersView.Visibility = Visibility.Collapsed;
            GamesView.Visibility = Visibility.Collapsed;
            SlotView.Visibility = Visibility.Visible;
        }
        private void AddPlayer_Click(object sender, RoutedEventArgs e)
        {
            Player newPlayer = new Player
            {
                Name = "Új Játékos",
                Balance = 500
            };

            dbHelper.InsertPlayer(newPlayer);
            LoadData();
        }
    }
        private void DeletePlayer_Click(object sender, RoutedEventArgs e)
        {
        Player selectedPlayer = PlayersGrid.SelectedItem as Player;
        if (selectedPlayer != null)
        {
            dbHelper.DeletePlayer(selectedPlayer.Id);
            LoadData();
        }
        else
        {
            MessageBox.Show("Válassz ki egy játékost!");
        }
    }
        private void UpdatePlayer_Click(object sender, RoutedEventArgs e)
        {
        Player selectedPlayer = PlayersGrid.SelectedItem as Player;
        if (selectedPlayer != null)
        {
            // Egyszerű módosítás: növeljük az egyenleget
            selectedPlayer.Balance += 100;
            dbHelper.UpdatePlayer(selectedPlayer);
            LoadData();
        }
        else
        {
            MessageBox.Show("Válassz ki egy játékost!");
        }
    }
        private void Spin_Click(object sender, RoutedEventArgs e)
        {
        int bet = 50;

        if (currentPlayer.Balance < bet)
        {
            MessageBox.Show("Nincs elég egyenleged!");
            return;
        }

        currentPlayer.Balance -= bet;

        // Random számok generálása
        int num1 = random.Next(1, 8);
        int num2 = random.Next(1, 8);
        int num3 = random.Next(1, 8);

        // Számok megjelenítése
        Slot1.Text = num1.ToString();
        Slot2.Text = num2.ToString();
        Slot3.Text = num3.ToString();

        // Nyeremény kiszámítása
        int win = 0;
        string resultMessage = "";

        if (num1 == num2 && num2 == num3)
        {
            // Három egyforma szám
            win = bet * 10;
            resultMessage = "JACKPOT! Nyereményed: " + win;
        }
        else if (num1 == num2 || num2 == num3 || num1 == num3)
        {
            // Két egyforma szám
            win = bet * 2;
            resultMessage = "Nyertél! Nyereményed: " + win;
        }
        else
        {
            resultMessage = "Sajnos most nem nyertél.";
        }

        // Nyeremény hozzáadása az egyenleghez
        currentPlayer.Balance += win;

        // UI frissítése
        ResultText.Text = resultMessage;
        UpdateBalanceDisplay();
    }
        private void AddGame_Click(object sender, RoutedEventArgs e)
        {
        Game newGame = new Game
        {
            Name = "Új Játék",
            Bet = 50
        };

        dbHelper.InsertGame(newGame);
        LoadData();
    }
        private void DeleteGame_Click(object sender, RoutedEventArgs e)
        {
        Game newGame = new Game
        {
            Name = "Új Játék",
            Bet = 50
        };

        dbHelper.InsertGame(newGame);
        LoadData();
    }
        private void UpdateGame_Click(object sender, RoutedEventArgs e)
        {
        Game selectedGame = GamesGrid.SelectedItem as Game;
        if (selectedGame != null)
        {
            selectedGame.Bet += 10;
            dbHelper.UpdateGame(selectedGame);
            LoadData();
        }
        else
        {
            MessageBox.Show("Válassz ki egy játékot!");
        }
    }
    }
}