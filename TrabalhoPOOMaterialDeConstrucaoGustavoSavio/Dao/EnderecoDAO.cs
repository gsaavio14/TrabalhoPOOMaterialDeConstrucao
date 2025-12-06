using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


using TrabalhoPOOMaterialDeConstrucaoGustavoSavio.Utilitarios;
using TrabalhoPOOMaterialDeConstrucaoGustavoSavio.Interface;
using TrabalhoPOOMaterialDeConstrucaoGustavoSavio.Modelos;

namespace TrabalhoPOOMaterialDeConstrucaoGustavoSavio.Dao
{

    public class EnderecoDao : IDao<Endereco>
    {
        private string ValidarECorrigirCep(string cepBruto)
        {
            if (string.IsNullOrWhiteSpace(cepBruto))
            {
                throw new ArgumentException("O campo CEP é obrigatório.");
            }
            string cepLimpo = Regex.Replace(cepBruto, @"[^\d]", "");
            if (cepLimpo.Length != 8)
            {
                throw new ArgumentException($"O CEP '{cepBruto}' é inválido. Ele deve conter 8 dígitos numéricos.");
            }
            return cepLimpo;
        }

        private string ValidarEstado(string estadoBruto)
        {
            if (string.IsNullOrWhiteSpace(estadoBruto))
            {
                throw new ArgumentException("O campo Estado (UF) é obrigatório.");
            }
            string estadoLimpo = estadoBruto.Trim().ToUpper();
            if (estadoLimpo.Length != 2 || !Regex.IsMatch(estadoLimpo, @"^[A-Z]{2}$"))
            {
                throw new ArgumentException($"O Estado '{estadoBruto}' é inválido. Ele deve conter exatamente duas letras (Ex: RO).");
            }
            return estadoLimpo;
        }

        private string ValidarCidade(string cidadeBruta)
        {
            if (string.IsNullOrWhiteSpace(cidadeBruta))
            {
                throw new ArgumentException("O campo Cidade é obrigatório.");
            }

            string cidadeLimpa = cidadeBruta.Trim();

            if (Regex.IsMatch(cidadeLimpa, @"\d"))
            {
                throw new ArgumentException($"O nome da Cidade '{cidadeBruta}' não pode conter números.");
            }

            return cidadeLimpa;
        }


        public int CreateAndGetId(Endereco endereco)
        {

            endereco.estado = ValidarEstado(endereco.estado);
            endereco.cep = ValidarECorrigirCep(endereco.cep);
            endereco.cidade = ValidarCidade(endereco.cidade);

            
            object ruaDB = string.IsNullOrWhiteSpace(endereco.rua) ? (object)DBNull.Value : endereco.rua;
            object numeroDB = string.IsNullOrWhiteSpace(endereco.numero) ? (object)DBNull.Value : endereco.numero;
            object bairroDB = string.IsNullOrWhiteSpace(endereco.bairro) ? (object)DBNull.Value : endereco.bairro;
            object logradouroDB = string.IsNullOrWhiteSpace(endereco.logradouro) ? (object)DBNull.Value : endereco.logradouro;
            object referenciaDB = string.IsNullOrWhiteSpace(endereco.referencia) ? (object)DBNull.Value : endereco.referencia;

            try
            {
                string sql = @"INSERT INTO ENDERECO (estado, rua, referencia, numero, bairro, cidade, cep, logradouro) 
                                 VALUES (@estado, @rua, @referencia, @numero, @bairro, @cidade, @cep, @logradouro)";

                using (var conexao = Conexao.Conectar())
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    
                    cmd.Parameters.AddWithValue("@estado", endereco.estado);
                    cmd.Parameters.AddWithValue("@rua", ruaDB);
                    cmd.Parameters.AddWithValue("@referencia", referenciaDB);
                    cmd.Parameters.AddWithValue("@numero", numeroDB);
                    cmd.Parameters.AddWithValue("@bairro", bairroDB);
                    cmd.Parameters.AddWithValue("@cidade", endereco.cidade);
                    cmd.Parameters.AddWithValue("@cep", endereco.cep);
                    cmd.Parameters.AddWithValue("@logradouro", logradouroDB);


                    cmd.ExecuteNonQuery();

                    return (int)cmd.LastInsertedId;
                }
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException) { throw; }
                throw new Exception($"Erro ao criar endereço e obter ID: {ex.Message}");
            }
        }

        public void Create(Endereco endereco) => CreateAndGetId(endereco);



        public Endereco GetById(int id)
        {
            try
            {
                var sql = "SELECT * FROM ENDERECO WHERE ID_endereco = @ID_endereco";
                using (var conexao = Conexao.Conectar())
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@ID_endereco", id);
                    var dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        Endereco e = new Endereco();
                        e.ID_endereco = dr.GetInt32("ID_endereco");
                        e.estado = dr.GetString("estado");

                      
                        e.rua = dr.IsDBNull(dr.GetOrdinal("rua")) ? null : dr.GetString("rua");

                        e.referencia = dr.IsDBNull(dr.GetOrdinal("referencia")) ? null : dr.GetString("referencia");
                        e.logradouro = dr.IsDBNull(dr.GetOrdinal("logradouro")) ? null : dr.GetString("logradouro");

                        e.numero = dr.IsDBNull(dr.GetOrdinal("numero")) ? null : dr.GetString("numero");
                        e.bairro = dr.IsDBNull(dr.GetOrdinal("bairro")) ? null : dr.GetString("bairro");

                        e.cidade = dr.GetString("cidade");
                        e.cep = dr.GetString("cep");
                        return e;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar endereço por ID: {ex.Message}");
            }
        }

        public void Update(Endereco endereco)
        {

            endereco.estado = ValidarEstado(endereco.estado);
            endereco.cep = ValidarECorrigirCep(endereco.cep);
            endereco.cidade = ValidarCidade(endereco.cidade);

           
            object ruaDB = string.IsNullOrWhiteSpace(endereco.rua) ? (object)DBNull.Value : endereco.rua;
            object numeroDB = string.IsNullOrWhiteSpace(endereco.numero) ? (object)DBNull.Value : endereco.numero;
            object bairroDB = string.IsNullOrWhiteSpace(endereco.bairro) ? (object)DBNull.Value : endereco.bairro;
            object logradouroDB = string.IsNullOrWhiteSpace(endereco.logradouro) ? (object)DBNull.Value : endereco.logradouro;
            object referenciaDB = string.IsNullOrWhiteSpace(endereco.referencia) ? (object)DBNull.Value : endereco.referencia;

            try
            {

                string sql = @"UPDATE ENDERECO SET 
                                 estado = @estado, 
                                 rua = @rua, 
                                 referencia = @referencia, 
                                 numero = @numero, 
                                 bairro = @bairro, 
                                 cidade = @cidade, 
                                 cep = @cep, 
                                 logradouro = @logradouro 
                              WHERE ID_endereco = @ID_endereco";

                using (var conexao = Conexao.Conectar())
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@estado", endereco.estado);
                    cmd.Parameters.AddWithValue("@rua", ruaDB);
                    cmd.Parameters.AddWithValue("@referencia", referenciaDB);
                    cmd.Parameters.AddWithValue("@numero", numeroDB);
                    cmd.Parameters.AddWithValue("@bairro", bairroDB);
                    cmd.Parameters.AddWithValue("@cidade", endereco.cidade);
                    cmd.Parameters.AddWithValue("@cep", endereco.cep);
                    cmd.Parameters.AddWithValue("@logradouro", logradouroDB);

                    
                    cmd.Parameters.AddWithValue("@ID_endereco", endereco.ID_endereco);


                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nenhum endereço encontrado para atualização.");
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException) { throw; }
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
                    if (cmd.ExecuteNonQuery() == 0)
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

                       
                        e.rua = dr.IsDBNull(dr.GetOrdinal("rua")) ? null : dr.GetString("rua");


                        e.referencia = dr.IsDBNull(dr.GetOrdinal("referencia")) ? null : dr.GetString("referencia");
                        e.logradouro = dr.IsDBNull(dr.GetOrdinal("logradouro")) ? null : dr.GetString("logradouro");

                        e.numero = dr.IsDBNull(dr.GetOrdinal("numero")) ? null : dr.GetString("numero");
                        e.bairro = dr.IsDBNull(dr.GetOrdinal("bairro")) ? null : dr.GetString("bairro");

                        e.cidade = dr.GetString("cidade");
                        e.cep = dr.GetString("cep");

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