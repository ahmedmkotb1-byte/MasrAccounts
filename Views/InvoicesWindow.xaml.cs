using System.Data;
using System.Windows;
using MasrAccounts.Helpers;

namespace MasrAccounts.Views
{
    public partial class InvoicesWindow : Window
    {
        public InvoicesWindow()
        {
            InitializeComponent();
            LoadInvoices();
        }

        private void LoadInvoices()
        {
            string query = "SELECT Id, ClientName AS 'العميل', ProductName AS 'الصنف', Weight AS 'الوزن', UnitPrice AS 'سعر الوحدة', Total AS 'الإجمالي', CreatedAt AS 'التاريخ' FROM Invoices ORDER BY CreatedAt DESC;";
            DataTable table = DatabaseHelper.ExecuteQuery(query);
            InvoicesGrid.ItemsSource = table.DefaultView;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
