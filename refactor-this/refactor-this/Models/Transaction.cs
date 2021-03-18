using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace refactor_this.Models
{
    public class Transaction
    {
        public Decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public Transaction(Decimal amount, DateTime date)
        {
            Amount = amount;
            Date = date;
        }
    }
}