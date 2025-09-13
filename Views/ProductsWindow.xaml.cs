using Microsoft.Data.Sqlite;
using System;
using System.Windows;

namespace MasrAccounts
{
    public partial class ProductsWindow : Window
    {
        private string db = "masraccounts.db";

        public ProductsWindow()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            ProductsList.Items.Clear();
            using var conn = new SqliteConnection($"Data Source={db}");
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "CREATE TABLE IF NOT EXISTS Products (Id INTEGER PRIMARY KEY, Name TEXT, Price REAL);";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "SELECT Name, Price FROM Products;";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ProductsList.Items.Add($"{reader.GetString(0)} - {reader.GetDouble(1)} جنيه");
            }
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            string name = ProductNameBox.Text.Trim();
            if (!double.TryParse(PriceBox.Text, out double price)) return;

            using var conn = new SqliteConnection($"Data Source={db}");
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Products (Name, Price) VALUES ($name, $price);";
            cmd.Parameters.AddWithValue("$name", name);
            cmd.Parameters.AddWithValue("$price", price);
            cmd.ExecuteNonQuery();
            LoadProducts();
            ProductNameBox.Clear();
            PriceBox.Clear();
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsList.SelectedItem == null) return;

            string selected = ProductsList.SelectedItem.ToString();
            string oldName = selected.Split('-')[0].Trim();
            string newName = ProductNameBox.Text.Trim();
            if (!double.TryParse(PriceBox.Text, out double newPrice)) return;

            using var conn = new SqliteConnection($"Data Source={db}");
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Products SET Name = $newName, Price = $newPrice WHERE Name = $oldName;";
            cmd.Parameters.AddWithValue("$newName", newName);
            cmd.Parameters.AddWithValue("$newPrice", newPrice);
            cmd.Parameters.AddWithValue("$oldName", oldName);
            cmd.ExecuteNonQuery();
            LoadProducts();
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsList.SelectedItem == null) return;

            string selected = ProductsList.SelectedItem.ToString();
            string name = selected.Split('-')[0].Trim();

            var confirm = MessageBox.Show($"هل تريد حذف الصنف '{name}'؟", "تأكيد الحذف", MessageBoxButton.YesNo);
            if (confirm != MessageBoxResult.Yes) return;

            using var conn = new SqliteConnection($"Data Source={db}");
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Products WHERE Name = $name;";
            cmd.Parameters.AddWithValue("$name", name);
            cmd.ExecuteNonQuery();
            LoadProducts();
        }

        private void SearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string keyword = SearchBox.Text.Trim();
            ProductsList.Items.Clear();

            using var conn = new SqliteConnection($"Data Source={db}");
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Name, Price FROM Products WHERE Name LIKE $kw;";
            cmd.Parameters.AddWithValue("$kw", $"%{keyword}%");

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ProductsList.Items.Add($"{reader.GetString(0)} - {reader.GetDouble(1)} جنيه");
            }
        }
    }
}
