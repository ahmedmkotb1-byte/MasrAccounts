using MasrAccounts.Helpers;

namespace MasrAccounts
{
    public static class LicenseChecker
    {
        public static bool IsValidLicense(string key)
        {
            return key == "TRIAL-1234"; // حط المفتاح اللي تريده هنا
        }
    }
}
