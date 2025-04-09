using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SQLite;

namespace CasinoGep
{
    public class DatabaseHelper
    {
        private string dbFile;

        public DatabaseHelper(string dbFilename)
        {
            dbFile = dbFilename;

            // Adatbázis létrehozása, ha nem létezik
            if (!File.Exists(dbFile))
            {
                SQLiteConnection.CreateFile(dbFile);
            }

            InitializeDatabase();
        }

        // Adatbázis inicializálása
        public void InitializeDatabase()
        {
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={dbFile};Version=3;"))
            {
                connection.Open();

                // Játékosok tábla létrehozása
                string createPlayersTable = @"
                CREATE TABLE IF NOT EXISTS Players (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Balance INTEGER NOT NULL
                )";

                // Játékok tábla létrehozása
                string createGamesTable = @"
                CREATE TABLE IF NOT EXISTS Games (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Bet INTEGER NOT NULL
                )";

                using (SQLiteCommand command = new SQLiteCommand(createPlayersTable, connection))
                {
                    command.ExecuteNonQuery();
                }

                using (SQLiteCommand command = new SQLiteCommand(createGamesTable, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Alapértelmezett adatok beszúrása, ha a táblák üresek
                InsertDefaultData(connection);
            }
        }

        // Alapértelmezett adatok beszúrása
        private void InsertDefaultData(SQLiteConnection connection)
        {
            // Ellenőrizzük, hogy vannak-e már adatok
            string countPlayers = "SELECT COUNT(*) FROM Players";
            string countGames = "SELECT COUNT(*) FROM Games";

            int playerCount = 0;
            int gameCount = 0;

            using (SQLiteCommand command = new SQLiteCommand(countPlayers, connection))
            {
                playerCount = Convert.ToInt32(command.ExecuteScalar());
            }

            using (SQLiteCommand command = new SQLiteCommand(countGames, connection))
            {
                gameCount = Convert.ToInt32(command.ExecuteScalar());
            }

            // Ha nincsenek játékosok, hozzáadunk néhányat
            if (playerCount == 0)
            {
                string insertPlayersQuery = @"
                INSERT INTO Players (Name, Balance) VALUES
                ('Játékos 1', 1000),
                ('Játékos 2', 500),
                ('Játékos 3', 2000)";

                using (SQLiteCommand command = new SQLiteCommand(insertPlayersQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            // Ha nincsenek játékok, hozzáadunk néhányat
            if (gameCount == 0)
            {
                string insertGamesQuery = @"
                INSERT INTO Games (Name, Bet) VALUES
                ('Slot Machine', 50),
                ('Blackjack', 100),
                ('Rulett', 75)";

                using (SQLiteCommand command = new SQLiteCommand(insertGamesQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        // Játékosok lekérdezése
        public List<Player> GetAllPlayers()
        {
            List<Player> players = new List<Player>();

            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={dbFile};Version=3;"))
            {
                connection.Open();

                string query = "SELECT * FROM Players";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            players.Add(new Player
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString(),
                                Balance = Convert.ToInt32(reader["Balance"])
                            });
                        }
                    }
                }
            }

            return players;
        }

        // Játékok lekérdezése
        public List<Game> GetAllGames()
        {
            List<Game> games = new List<Game>();

            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={dbFile};Version=3;"))
            {
                connection.Open();

                string query = "SELECT * FROM Games";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            games.Add(new Game
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString(),
                                Bet = Convert.ToInt32(reader["Bet"])
                            });
                        }
                    }
                }
            }

            return games;
        }

        // Új játékos létrehozása
        public void InsertPlayer(Player player)
        {
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={dbFile};Version=3;"))
            {
                connection.Open();

                string query = "INSERT INTO Players (Name, Balance) VALUES (@Name, @Balance)";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", player.Name);
                    command.Parameters.AddWithValue("@Balance", player.Balance);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Játékos törlése
        public void DeletePlayer(int id)
        {
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={dbFile};Version=3;"))
            {
                connection.Open();

                string query = "DELETE FROM Players WHERE Id = @Id";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Játékos frissítése
        public void UpdatePlayer(Player player)
        {
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={dbFile};Version=3;"))
            {
                connection.Open();

                string query = "UPDATE Players SET Name = @Name, Balance = @Balance WHERE Id = @Id";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", player.Id);
                    command.Parameters.AddWithValue("@Name", player.Name);
                    command.Parameters.AddWithValue("@Balance", player.Balance);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Új játék létrehozása
        public void InsertGame(Game game)
        {
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={dbFile};Version=3;"))
            {
                connection.Open();

                string query = "INSERT INTO Games (Name, Bet) VALUES (@Name, @Bet)";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", game.Name);
                    command.Parameters.AddWithValue("@Bet", game.Bet);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Játék törlése
        public void DeleteGame(int id)
        {
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={dbFile};Version=3;"))
            {
                connection.Open();

                string query = "DELETE FROM Games WHERE Id = @Id";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Játék frissítése
        public void UpdateGame(Game game)
        {
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={dbFile};Version=3;"))
            {
                connection.Open();

                string query = "UPDATE Games SET Name = @Name, Bet = @Bet WHERE Id = @Id";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", game.Id);
                    command.Parameters.AddWithValue("@Name", game.Name);
                    command.Parameters.AddWithValue("@Bet", game.Bet);
                    command.ExecuteNonQuery();
                }
            }
        }
}
