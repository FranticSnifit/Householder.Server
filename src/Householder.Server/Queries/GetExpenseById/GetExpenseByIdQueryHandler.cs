using System.Collections.Generic;
using System.Threading.Tasks;
using Householder.Server.DataAccess;
using Householder.Server.Models;
using MySqlConnector;

namespace Householder.Server.Queries
{
    public class GetExpenseByIdQueryHandler : IQueryHandler<GetExpenseByIdQuery, Expense>
    {
        private MySqlDatabase database;

        public GetExpenseByIdQueryHandler(MySqlDatabase database)
        {
            this.database = database;
        }

        public async Task<Expense> Handle(GetExpenseByIdQuery query)
        {
            var results = new List<Expense>();
            var id = query.Id;

            var cmd = database.Connection.CreateCommand();

            cmd.CommandText = @"SELECT r.name AS resident_name, e.amount, e.transaction_date, e.note, e.status_id FROM `expense` e NATURAL JOIN `resident` r WHERE e.id = @id";

            cmd.Parameters.Add(new MySqlParameter("@id", id));

            var reader = await cmd.ExecuteReaderAsync();

            await reader.ReadAsync();

            return new Expense(
                    new Resident(reader.GetString("resident_name")),
                    reader.GetDouble("amount"),
                    reader.GetDateTime("transaction_date"),
                    reader.GetString("note"),
                    (ExpenseStatus)reader.GetInt32("status_id")
                );
        }
    }
}