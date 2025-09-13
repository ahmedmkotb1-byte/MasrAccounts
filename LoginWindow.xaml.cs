using System.Windows;

namespace MasrAccounts
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var user = UsernameBox.Text;
            var pass = PasswordBox.Password;
            var license = LicenseBox.Text;

            if (ValidateLogin(user, pass))
            {
                if (LicenseChecker.IsValidLicense(license))
                {
                    MainWindow main = new MainWindow();
                    main.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("مفتاح التفعيل غير صالح");
                }
            }
            else
            {
                MessageBox.Show("بيانات الدخول غير صحيحة");
            }
        }
    }
}
