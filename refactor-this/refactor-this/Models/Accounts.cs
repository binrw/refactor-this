using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace refactor_this.Models
{
    public class Account
    {
        private bool isNew;

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Number { get; set; }

        public Decimal Amount { get; set; }

        public Account()
        {
            isNew = true;
        }

        public Account(Guid id)
        {
            isNew = false;
            Id = id;
        }

        public void Save()
        {
            using (var connection = Helpers.NewConnection())
            {
                SqlCommand command;
                if (isNew)
                    command = new SqlCommand($"INSERT INTO Accounts (Id, Name, Number, Amount) VALUES ('{Guid.NewGuid()}', '{Name}', {Number}, 0)", connection);
                else
                    command = new SqlCommand($"UPDATE Accounts SET Name = '{Name}' WHERE Id = '{Id}'", connection);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete()
        {
            using (var connection = Helpers.NewConnection())
            {
                SqlCommand command = new SqlCommand($"DELETE FROM Accounts WHERE Id = '{Id}'", connection);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}