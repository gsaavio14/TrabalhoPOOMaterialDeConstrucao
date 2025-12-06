using ConexaoBancodeDados.Utilitarios;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrabalhoPOOMaterialDeConstrucaoGustavoSavio.Interface;
using TrabalhoPOOMaterialDeConstrucaoGustavoSavio.Modelos;

namespace TrabalhoPOOMaterialDeConstrucaoGustavoSavio.Dao
{
    internal class EnderecoDao : IDao<Endereco>
    {

        public int CreateAndGetId(Endereco endereco)
        {
            try
            {
                string sql = @"INSERT INTO ENDERECO (estado, rua, referencia, numero, bairro, cidade, cep, logradouro) 
                               VALUES (@estado, @rua, @referencia, @numero, @bairro, @cidade, @cep, @logradouro)";

                using (var conexao = Conexao.Conectar())
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@estado", endereco.estado);
                    cmd.Parameters.AddWithValue("@rua", endereco.rua);
                    cmd.Parameters.AddWithValue("@referencia", endereco.referencia);
                    cmd.Parameters.AddWithValue("@numero", endereco.numero);
                    cmd.Parameters.AddWithValue("@bairro", endereco.bairro);
                    cmd.Parameters.AddWithValue("@cidade", endereco.cidade);
                    cmd.Parameters.AddWithValue("@cep", endereco.cep);
                    cmd.Parameters.AddWithValue("@logradouro", endereco.logradouro);

                    cmd.ExecuteNonQuery();

                    return (int)cmd.LastInsertedId;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar endereço e obter ID: {ex.Message}");
            }
        }


        public void Create(Endereco endereco)
        {
            CreateAndGetId(endereco);
        }

        public void Update(Endereco endereco)
        {
            try
            {
                string sql = @"UPDATE ENDERECO SET estado = @estado, rua = @rua, referencia = @referencia, numero = @numero, 
                               bairro = @bairro, cidade = @cidade, cep = @cep, logradouro = @logradouro 
                               WHERE ID_endereco = @ID_endereco";

                using (var conexao = Conexao.Conectar())
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@estado", endereco.estado);
                    cmd.Parameters.AddWithValue("@rua", endereco.rua);
                    cmd.Parameters.AddWithValue("@referencia", endereco.referencia);
                    cmd.Parameters.AddWithValue("@numero", endereco.numero);
                    cmd.Parameters.AddWithValue("@bairro", endereco.bairro);
                    cmd.Parameters.AddWithValue("@cidade", endereco.cidade);
                    cmd.Parameters.AddWithValue("@cep", endereco.cep);
                    cmd.Parameters.AddWithValue("@logradouro", endereco.logradouro);
                    cmd.Parameters.AddWithValue("@ID_endereco", endereco.ID_endereco);

                    var linhasAfetadas = cmd.ExecuteNonQuery();

                    if (linhasAfetadas == 0)
                    {
                        throw new Exception("Nenhum endereço encontrado para atualização.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar endereço: {ex.Message}");
            }
        }


        public void Delete(int id)
        {
            try
            {
                string sql = "DELETE FROM ENDERECO WHERE ID_endereco = @ID_endereco";

                using (var conexao = Conexao.Conectar())
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@ID_endereco", id);

                    var linhasAfetadas = cmd.ExecuteNonQuery();
                    if (linhasAfetadas == 0)
                    {
                        throw new Exception("Nenhum registro encontrado com esse ID.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao deletar endereço: {ex.Message}");
            }
        }

        public List<Endereco> GetAll()
        {
            List<Endereco> listaDeEnderecos = new List<Endereco>();

            try
            {
                var sql = "SELECT * FROM ENDERECO ORDER BY cidade, rua";

                using (var conexao = Conexao.Conectar())
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Endereco e = new Endereco();

                        e.ID_endereco = dr.GetInt32("ID_endereco");
                        e.estado = dr.GetString("estado");
                        e.rua = dr.GetString("rua");
                        e.referencia = dr.GetString("referencia");
                        e.numero = dr.GetString("numero");
                        e.bairro = dr.GetString("bairro");
                        e.cidade = dr.GetString("cidade");
                        e.cep = dr.GetString("cep");
                        e.logradouro = dr.GetString("logradouro");

                        listaDeEnderecos.Add(e);
                    }
                }
                return listaDeEnderecos;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar endereços: {ex.Message}");
            }
        }
    }
}