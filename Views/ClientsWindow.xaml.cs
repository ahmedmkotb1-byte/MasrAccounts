using Microsoft.Data.Sqlite;
using System.Windows;

namespace MasrAccounts
{
    public partial class ClientsWindow : Window
    {
        private string db = "masraccounts.db";

        public ClientsWindow()
        {
            InitializeComponent();
            LoadClients();
        }

        private void LoadClients()
        {
            ClientsList.Items.Clear();
            using var conn = new SqliteConnection($"Data Source={db}");
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "CREATE TABLE IF NOT EXISTS Clients (Id INTEGER PRIMARY KEY, Name TEXT);";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "SELECT Name FROM Clients;";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ClientsList.Items.Add(reader.GetString(0));
            }
        }

        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            string name = ClientNameBox.Text.Trim();
            if (string.IsNullOrEmpty(name)) return;

            using var conn = new SqliteConnection($"Data Source={db}");
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Clients (Name) VALUES ($name);";
            cmd.Parameters.AddWithValue("$name", name);
            cmd.ExecuteNonQuery();
            LoadClients();
            ClientNameBox.Clear();
        }

        private void EditClient_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsList.SelectedItem == null) return;

            string oldName = ClientsList.SelectedItem.ToString();
            string newName = ClientNameBox.Text.Trim();

            if (string.IsNullOrEmpty(newName)) return;

            using var conn = new SqliteConnection($"Data Source={db}");
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Clients SET Name = $new WHERE Name = $old;";
            cmd.Parameters.AddWithValue("$new", newName);
            cmd.Parameters.AddWithValue("$old", oldName);
            cmd.ExecuteNonQuery();
            LoadClients();
            ClientNameBox.Clear();
        }

        private void DeleteClient_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsList.SelectedItem == null) return;

            string name = ClientsList.SelectedItem.ToString();
            var confirm = MessageBox.Show($"هل تريد حذف العميل '{name}'؟", "تأكيد الحذف", MessageBoxButton.YesNo);
            if (confirm != MessageBoxResult.Yes) return;

            using var conn = new SqliteConnection($"Data Source={db}");
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Clients WHERE Name = $name;";
            cmd.Parameters.AddWithValue("$name", name);
            cmd.ExecuteNonQuery();
            LoadClients();
        }

        private void SearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string keyword = SearchBox.Text.Trim();
            ClientsList.Items.Clear();

            using var conn = new SqliteConnection($"Data Source={db}");
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Name FROM Clients WHERE Name LIKE $kw;";
            cmd.Parameters.AddWithValue("$kw", $"%{keyword}%");

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ClientsList.Items.Add(reader.GetString(0));
            }
        }
    }
}
