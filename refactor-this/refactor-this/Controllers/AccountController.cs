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
    public class AccountController : ApiController
    {
        [HttpGet, Route("api/Accounts/{id}")]
        public IHttpActionResult GetById(Guid id)
        {
            using (var connection = Helpers.NewConnection())
            {
                return Ok(Get(id));
            }
        }

        [HttpGet, Route("api/Accounts")]
        public IHttpActionResult Get()
        {
            using (var connection = Helpers.NewConnection())
            {
                SqlCommand command = new SqlCommand($"SELECT Id FROM Accounts", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                var accounts = new List<Account>();
                while (reader.Read())
                {
                    var id = Guid.Parse(reader["Id"].ToString());
                    var account = Get(id);
                    accounts.Add(account);
                }

                return Ok(accounts);
            }
        }

        private Account Get(Guid id)
        {
            using (var connection = Helpers.NewConnection())
            {
                var command = new SqlCommand("SELECT * FROM Accounts WHERE Id = '@Id'", connection);
                command.Parameters.Add("@Id", System.Data.SqlDbType.UniqueIdentifier); // UniqueIdentifier is GUID
                command.Parameters["@Id"].Value = id;

                connection.Open();
                var reader = command.ExecuteReader();
                if (!reader.Read())
                    throw new ArgumentException();

                var account = new Account(id);
                account.Name = reader["Name"].ToString();
                account.Number = reader["Number"].ToString();
                account.Amount = Decimal.Parse(reader["Amount"].ToString());
                return account;
            }
        }

        [HttpPost, Route("api/Accounts")]
        public IHttpActionResult Add(Account account)
        {
            account.Save();
            return Ok();
        }

        [HttpPut, Route("api/Accounts/{id}")]
        public IHttpActionResult Update(Guid id, Account account)
        {
            var existing = Get(id);
            existing.Name = account.Name;
            existing.Save();
            return Ok();
        }

        [HttpDelete, Route("api/Accounts/{id}")]
        public IHttpActionResult Delete(Guid id)
        {
            var existing = Get(id);
            existing.Delete();
            return Ok();
        }
    }
}