using System.Windows;
using MasrAccounts.Helpers;

namespace MasrAccounts
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            DatabaseSeeder.EnsureDatabaseCreated();
        }
    }
}
