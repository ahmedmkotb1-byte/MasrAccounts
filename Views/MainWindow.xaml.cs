using System.Windows;
using MasrAccounts.Helpers;

namespace MasrAccounts.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // 🟢 تأكد من وجود الجداول في قاعدة البيانات
            DatabaseSeeder.EnsureDatabaseCreated();
        }

        private void OpenClients_Click(object sender, RoutedEventArgs e)
        {
            var window = new ClientsWindow();
            window.ShowDialog();
        }

        private void OpenProducts_Click(object sender, RoutedEventArgs e)
        {
            var window = new ProductsWindow();
            window.ShowDialog();
        }

        private void OpenInvoice_Click(object sender, RoutedEventArgs e)
        {
            var window = new InvoiceWindow();
            window.ShowDialog();
        }

        private void OpenInvoices_Click(object sender, RoutedEventArgs e)
        {
            var window = new InvoicesWindow();
            window.ShowDialog();
        }

        private void OpenSettings_Click(object sender, RoutedEventArgs e)
        {
            var window = new SettingsWindow();
            window.ShowDialog();
        }

        private void OpenAbout_Click(object sender, RoutedEventArgs e)
        {
            var window = new AboutWindow();
            window.ShowDialog();
        }

        private void ExitApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
