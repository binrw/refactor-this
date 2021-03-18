using refactor_this.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace refactor_this.Controllers
{
    public class TransactionController : ApiController
    {
        [HttpGet, Route("api/Accounts/{id}/Transactions")]
        public IHttpActionResult GetTransactions(Guid id)
        {
            using (var connection = Helpers.NewConnection())
            {
                var command = new SqlCommand("select Amount, Date from Transactions where AccountId = '@Id'", connection);
                command.Parameters.Add("@Id", System.Data.SqlDbType.UniqueIdentifier); // UniqueIdentifier is GUID
                command.Parameters["@Id"].Value = id;

                connection.Open();
                var reader = command.ExecuteReader();
                var transactions = new List<Transaction>();
                while (reader.Read())
                {
                    var amount = (Decimal)reader.GetDouble(0);
                    var date = reader.GetDateTime(1);
                    transactions.Add(new Transaction(amount, date));
                }
                return Ok(transactions);
            }
        }

        [HttpPost, Route("api/Accounts/{id}/Transactions")]
        public IHttpActionResult AddTransaction(Guid id, Transaction transaction)
        {
            using (var connection = Helpers.NewConnection())
            {
                var command = new SqlCommand("update Accounts set Amount = Amount + @Amount where Id = '@Id'", connection);
                command.Parameters.Add("@Id", System.Data.SqlDbType.UniqueIdentifier); // UniqueIdentifier is GUID
                command.Parameters.Add("@Amount", System.Data.SqlDbType.Decimal);
                command.Parameters["@Id"].Value = id;
                command.Parameters["@Amount"].Value = transaction.Amount;

                connection.Open();
                if (command.ExecuteNonQuery() != 1)
                    return BadRequest("Could not update account amount");

                command = new SqlCommand($"INSERT INTO Transactions (Id, Amount, Date, AccountId) VALUES ('{Guid.NewGuid()}', {transaction.Amount}, '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', '{id}')", connection);
                if (command.ExecuteNonQuery() != 1)
                    return BadRequest("Could not insert the transaction");

                return Ok();
            }
        }
    }
}