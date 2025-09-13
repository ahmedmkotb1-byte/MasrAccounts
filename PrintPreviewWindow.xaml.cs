using Microsoft.Data.Sqlite;
using System;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace MasrAccounts
{
    public partial class PrintPreviewWindow : Window
    {
        private string db = "masraccounts.db";

        public PrintPreviewWindow()
        {
            InitializeComponent();
            LoadLastInvoice();
        }

        private void LoadLastInvoice()
        {
            using var conn = new SqliteConnection($"Data Source={db}");
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ClientName, ProductName, Weight, UnitPrice, Total, CreatedAt FROM Invoices ORDER BY Id DESC LIMIT 1;";
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                ClientText.Text = $"العميل: {reader.GetString(0)}";
                ProductText.Text = $"الصنف: {reader.GetString(1)}";
                WeightText.Text = $"الوزن: {reader.GetDouble(2)} كجم";
                PriceText.Text = $"سعر الوحدة: {reader.GetDouble(3)} جنيه";
                TotalText.Text = $"الإجمالي: {reader.GetDouble(4)} جنيه";
                DateText.Text = $"التاريخ: {reader.GetString(5)}";
            }
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() == true)
            {
                pd.PrintVisual(InvoicePanel, "فاتورة بيع");
            }
        }
    }
}
