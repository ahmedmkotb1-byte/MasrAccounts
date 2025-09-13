using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;

namespace MasrAccounts.Helpers
{
    public static class DatabaseHelper
    {
        private static readonly string dbPath = "masraccounts.db";
        private static readonly string connectionString = $"Data Source={dbPath}";

        /// <summary>
        /// تنفيذ أمر SQL لا يُرجع بيانات (INSERT, UPDATE, DELETE)
        /// </summary>
        public static void ExecuteNonQuery(string query, Dictionary<string, object> parameters = null)
        {
            using var conn = new SqliteConnection(connectionString);
            conn.Open();
            using var cmd = new SqliteCommand(query, conn);

            if (parameters != null)
            {
                foreach (var param in parameters)
                    cmd.Parameters.AddWithValue(param.Key, param.Value);
            }

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// تنفيذ SELECT وإرجاع النتائج في DataTable
        /// </summary>
        public static DataTable ExecuteQuery(string query, Dictionary<string, object> parameters = null)
        {
            using var conn = new SqliteConnection(connectionString);
            conn.Open();
            using var cmd = new SqliteCommand(query, conn);

            if (parameters != null)
            {
                foreach (var param in parameters)
                    cmd.Parameters.AddWithValue(param.Key, param.Value);
            }

            using var reader = cmd.ExecuteReader();
            var table = new DataTable();
            table.Load(reader);
            return table;
        }

        /// <summary>
        /// تنفيذ SELECT وإرجاع أول نتيجة فقط (مفيدة في COUNT أو الحصول على قيمة واحدة)
        /// </summary>
        public static object ExecuteScalar(string query, Dictionary<string, object> parameters = null)
        {
            using var conn = new SqliteConnection(connectionString);
            conn.Open();
            using var cmd = new SqliteCommand(query, conn);

            if (parameters != null)
            {
                foreach (var param in parameters)
                    cmd.Parameters.AddWithValue(param.Key, param.Value);
            }

            return cmd.ExecuteScalar();
        }
    }
}
