using eCommerce.API.Models;
using eCommerce.API.Database;
using Npgsql;
using System.Data;

namespace eCommerce.API.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private IDbConnection _connection;

        public UsuarioRepository()
        {
            var connString = "User Id=postgres; Password=Coxa@1909; Host=localhost; Port=5432; Database=eCommerce";

            _connection = new NpgsqlConnection(connString);

        }

        public List<Usuario> Get()
        {
            List<Usuario> listaUsuarios = new();
            try
            {
                NpgsqlCommand command = new()
                {
                    CommandText = "SELECT * FROM Usuarios",
                    Connection = (NpgsqlConnection)_connection
                };

                _connection.Open();

                NpgsqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {

                    Usuario usuario = new Usuario();
                    usuario.Id = dataReader.GetInt32("Id");
                    usuario.Nome = dataReader.GetString("Nome");
                    usuario.Email = dataReader.GetString("Email");
                    usuario.Sexo = dataReader.GetString("Sexo");
                    usuario.RG = dataReader.GetString("RG");
                    usuario.CPF = dataReader.GetString("CPF");
                    usuario.NomeMae = dataReader.GetString("NomeMae");
                    usuario.SituacaoCadastro = dataReader.GetString("SituacaoCadastro");
                    
                    listaUsuarios.Add(usuario);
                }
            }
            finally
            {
                _connection.Close();
            }
            return listaUsuarios;
        }

        public Usuario? Get(int id)
        {
            try
            {
                NpgsqlCommand command = new()
                {
                    CommandText = $"SELECT * FROM Usuarios WHERE Id = @Id"
                };
                command.Parameters.AddWithValue("@Id", id);
                command.Connection = (NpgsqlConnection)_connection;

                _connection.Open();
                NpgsqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    Usuario usuario = new()
                    {
                        Id = dataReader.GetInt32("Id"),
                        Nome = dataReader.GetString("Nome"),
                        Email = dataReader.GetString("Email"),
                        Sexo = dataReader.GetString("Sexo"),
                        RG = dataReader.GetString("RG"),
                        CPF = dataReader.GetString("CPF"),
                        NomeMae = dataReader.GetString("NomeMae"),
                        SituacaoCadastro = dataReader.GetString("SituacaoCadastro")
                    };

                    return usuario;
                }
                
            }
            finally
            {
                _connection.Close();
            }

            return null;
        }

        public void Insert(Usuario usuario)
        {
            _connection.Open();
            NpgsqlTransaction transaction = (NpgsqlTransaction)_connection.BeginTransaction();
            try
            {
                NpgsqlCommand command = new()
                {
                    Transaction = transaction,
                    Connection = (NpgsqlConnection)_connection,

                    CommandText = "INSERT INTO Usuarios(Nome, Email, Sexo, RG, CPF, NomeMae, SituacaoCadastro) VALUES(@Nome, @Email, @Sexo, @RG, @CPF, @NomeMae, @SituacaoCadastro)"
                    
                };

                command.Parameters.AddWithValue("@Nome", usuario.Nome);
                command.Parameters.AddWithValue("@Email", usuario.Email);
                command.Parameters.AddWithValue("@Sexo", usuario.Sexo);
                command.Parameters.AddWithValue("@RG", usuario.RG);
                command.Parameters.AddWithValue("@CPF", usuario.CPF);
                command.Parameters.AddWithValue("@NomeMae", usuario.NomeMae);
                command.Parameters.AddWithValue("@SituacaoCadastro", usuario.SituacaoCadastro);

                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch(Exception ex)
            {
                try { 
                    transaction.Rollback();
                }
                catch
                {
                    // Adicionar no log que o Rollback falhou
                }

                throw new Exception("Erro ao tentar inserir os dados!");
            }

            finally
            {
                _connection.Close();
            }
        }

        public void Update(Usuario usuario)
        {
            try
            {
                NpgsqlCommand command = new()
                {
                    CommandText = "UPDATE Usuarios SET Nome = @Nome, Email = @Email, Sexo = @Sexo, RG = @RG, CPF = @CPF, NomeMae = @NomeMae, SituacaoCadastro = @SituacaoCadastro WHERE Id = @Id",
                    Connection = (NpgsqlConnection)_connection
                };

                command.Parameters.AddWithValue("@Nome", usuario.Nome);
                command.Parameters.AddWithValue("@Email", usuario.Email);
                command.Parameters.AddWithValue("@Sexo", usuario.Sexo);
                command.Parameters.AddWithValue("@RG", usuario.RG);
                command.Parameters.AddWithValue("@CPF", usuario.CPF);
                command.Parameters.AddWithValue("@NomeMae", usuario.NomeMae);
                command.Parameters.AddWithValue("@SituacaoCadastro", usuario.SituacaoCadastro);

                command.Parameters.AddWithValue("@Id", usuario.Id);

                _connection.Open();
                command.ExecuteNonQuery();
            }
            finally
            {
                _connection.Close();
            }
        }
        public void Delete(int id)
        {
            try
            {
                NpgsqlCommand command = new()
                {
                    CommandText = "DELETE FROM Usuarios WHERE Id = @Id",
                    Connection = (NpgsqlConnection)_connection
                };

                command.Parameters.AddWithValue("@Id", id);
                _connection.Open();
                command.ExecuteNonQuery();
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}
