using System;
using Npgsql;

namespace eCommerce.API.Database
{
    public class DatabaseConnect
    {
        readonly NpgsqlConnection conexao = new NpgsqlConnection(connectionString: "Default");
    }
}
