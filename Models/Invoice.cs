using System;

namespace MasrAccounts.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string ProductName { get; set; }
        public double Weight { get; set; }
        public double UnitPrice { get; set; }
        public double Total { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
