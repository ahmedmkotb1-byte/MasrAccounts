using Microsoft.Data.Sqlite;
using System;
using System.Windows;

namespace MasrAccounts
{
    public partial class InvoiceWindow : Window
    {
        private string db = "masraccounts.db";

        public InvoiceWindow()
        {
            InitializeComponent();
            CreateInvoicesTable();
            LoadClients();
            LoadProducts();
        }

        private void CreateInvoicesTable()
        {
            using var conn = new SqliteConnection($"Data Source={db}");
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Invoices (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    ClientName TEXT,
                                    ProductName TEXT,
                                    Weight REAL,
                                    UnitPrice REAL,
                                    Total REAL,
                                    CreatedAt TEXT
                                );";
            cmd.ExecuteNonQuery();
        }

        private void LoadClients()
        {
            using var conn = new SqliteConnection($"Data Source={db}");
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Name FROM Clients;";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ClientCombo.Items.Add(reader.GetString(0));
            }
        }

        private void LoadProducts()
        {
            using var conn = new SqliteConnection($"Data Source={db}");
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Name FROM Products;";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ProductCombo.Items.Add(reader.GetString(0));
            }
        }

        private void CalculatePrice_Click(object sender, RoutedEventArgs e)
        {
            if (ClientCombo.SelectedItem == null || ProductCombo.SelectedItem == null)
            {
                MessageBox.Show("اختر العميل والصنف أولاً");
                return;
            }

            if (!double.TryParse(WeightBox.Text, out double weight))
            {
                MessageBox.Show("الرجاء إدخال وزن صحيح");
                return;
            }

            string product = ProductCombo.SelectedItem.ToString();
            string client = ClientCombo.SelectedItem.ToString();

            using var conn = new SqliteConnection($"Data Source={db}");
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Price FROM Products WHERE Name = $name;";
            cmd.Parameters.AddWithValue("$name", product);
            var price = Convert.ToDouble(cmd.ExecuteScalar());

            double total = price * weight;
            ResultText.Text = $"الإجمالي: {total} جنيه";

            // 📝 حفظ الفاتورة في قاعدة البيانات
            var save = conn.CreateCommand();
            save.CommandText = @"INSERT INTO Invoices (ClientName, ProductName, Weight, UnitPrice, Total, CreatedAt)
                                 VALUES ($client, $product, $weight, $unitPrice, $total, $date);";
            save.Parameters.AddWithValue("$client", client);
            save.Parameters.AddWithValue("$product", product);
            save.Parameters.AddWithValue("$weight", weight);
            save.Parameters.AddWithValue("$unitPrice", price);
            save.Parameters.AddWithValue("$total", total);
            save.Parameters.AddWithValue("$date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            save.ExecuteNonQuery();
        }

        // ✅ وظيفة طباعة آخر فاتورة محفوظة
        private void PrintLastInvoice_Click(object sender, RoutedEventArgs e)
        {
            var preview = new PrintPreviewWindow();
            preview.ShowDialog();
        }
    }
}
