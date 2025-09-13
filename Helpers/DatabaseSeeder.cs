using MasrAccounts.Helpers;
using Microsoft.Data.Sqlite;
using System;

namespace MasrAccounts.Helpers
{
    public static class DatabaseSeeder
    {
        private static readonly string dbPath = "masraccounts.db";
        private static readonly string connectionString = $"Data Source={dbPath}";

        public static void EnsureDatabaseCreated()
        {
            using var conn = new SqliteConnection(connectionString);
            conn.Open();

            CreateClientsTable(conn);
            CreateProductsTable(conn);
            CreateInvoicesTable(conn);
        }

        private static void CreateClientsTable(SqliteConnection conn)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Clients (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL UNIQUE
                );";
            cmd.ExecuteNonQuery();
        }

        private static void CreateProductsTable(SqliteConnection conn)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Products (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL UNIQUE,
                    Price REAL NOT NULL
                );";
            cmd.ExecuteNonQuery();
        }

        private static void CreateInvoicesTable(SqliteConnection conn)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Invoices (
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
    }
}
