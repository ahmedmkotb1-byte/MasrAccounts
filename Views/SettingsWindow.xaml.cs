using Microsoft.Data.Sqlite;
using System.Windows;

namespace MasrAccounts
{
    public partial class SettingsWindow : Window
    {
        private string db = "masraccounts.db";

        public SettingsWindow()
        {
            InitializeComponent();
            LoadCurrentSettings();
        }

        private void LoadCurrentSettings()
        {
            using var conn = new SqliteConnection($"Data Source={db}");
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Settings (
                                    Id INTEGER PRIMARY KEY,
                                    Username TEXT,
                                    Password TEXT
                                );";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "SELECT Username, Password FROM Settings LIMIT 1;";
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                UsernameBox.Text = reader.GetString(0);
                PasswordBox.Password = reader.GetString(1);
            }
        }

        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            string newUser = UsernameBox.Text.Trim();
            string newPass = PasswordBox.Password.Trim();

            if (string.IsNullOrWhiteSpace(newUser) || string.IsNullOrWhiteSpace(newPass))
            {
                MessageBox.Show("الرجاء إدخال بيانات صحيحة");
                return;
            }

            using var conn = new SqliteConnection($"Data Source={db}");
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Settings;";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO Settings (Username, Password) VALUES ($u, $p);";
            cmd.Parameters.AddWithValue("$u", newUser);
            cmd.Parameters.AddWithValue("$p", newPass);
            cmd.ExecuteNonQuery();

            MessageBox.Show("تم حفظ التغييرات");
            this.Close();
        }
    }
}
