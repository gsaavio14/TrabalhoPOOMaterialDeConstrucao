using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TrabalhoPOOMaterialDeConstrucaoGustavoSavio.Interface;
using TrabalhoPOOMaterialDeConstrucaoGustavoSavio.Modelos;
using System.Text;
using System.Globalization;
using TrabalhoPOOMaterialDeConstrucaoGustavoSavio.Utilitarios;

namespace TrabalhoPOOMaterialDeConstrucaoGustavoSavio.Dao
{

    public class FuncionarioDao : IDao<Funcionario>
    {
        private readonly EnderecoDao _enderecoDao;


        public FuncionarioDao(EnderecoDao enderecoDao)
        {
            _enderecoDao = enderecoDao ?? throw new ArgumentNullException(nameof(enderecoDao));
        }

        public FuncionarioDao() : this(new EnderecoDao()) { }


        private string RemoverAcentos(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return texto;

            var normalizado = texto.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (char c in normalizado)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        private string ValidarNome(string nomeBruto)
        {
            if (string.IsNullOrWhiteSpace(nomeBruto))
            {
                throw new ArgumentException("O Nome do funcionário é obrigatório.");
            }

            string nomeProcessado = nomeBruto.Trim();
            nomeProcessado = RemoverAcentos(nomeProcessado);

            if (Regex.IsMatch(nomeProcessado, @"\d"))
            {
                throw new ArgumentException($"O Nome '{nomeBruto}' não pode conter números.");
            }

            return nomeProcessado;
        }

        private string ValidarCPF(string cpfBruto)
        {
            if (string.IsNullOrWhiteSpace(cpfBruto))
            {
                throw new ArgumentException("O CPF do funcionário é obrigatório.");
            }
            string cpfLimpo = Regex.Replace(cpfBruto, @"[^\d]", "");
            if (cpfLimpo.Length != 11)
            {
                throw new ArgumentException($"O CPF '{cpfBruto}' é inválido. Ele deve conter exatamente 11 dígitos numéricos.");
            }
            return cpfLimpo;
        }

        private string ValidarTelefone(string telefoneBruto)
        {
            if (string.IsNullOrWhiteSpace(telefoneBruto))
            {
                return null;
            }

            string telLimpo = Regex.Replace(telefoneBruto, @"[^\d]", "");

            if (telLimpo.Length < 10 || telLimpo.Length > 11)
            {
                throw new ArgumentException($"O Telefone '{telefoneBruto}' é inválido. Ele deve ter 10 ou 11 dígitos (incluindo DDD).");
            }

            return telLimpo;
        }



        public void Create(Funcionario funcionario)
        {

            funcionario.nomeFuncionario = ValidarNome(funcionario.nomeFuncionario);
            funcionario.cpfFuncionario = ValidarCPF(funcionario.cpfFuncionario);
            funcionario.telefoneFuncionario = ValidarTelefone(funcionario.telefoneFuncionario);

            object telefoneDB = funcionario.telefoneFuncionario == null
              ? (object)DBNull.Value
              : funcionario.telefoneFuncionario;

            try
            {

                string sql = "INSERT INTO FUNCIONARIO (nomeFuncionario, cpfFuncionario, cargoFuncionario, telefoneFuncionario, FK_Endereco_id_endereco) VALUES (@nomeFuncionario, @cpfFuncionario, @cargoFuncionario, @telefoneFuncionario, @FK_Endereco_id_endereco)";

                using (var conexao = Conexao.Conectar())
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@nomeFuncionario", funcionario.nomeFuncionario);
                    cmd.Parameters.AddWithValue("@cpfFuncionario", funcionario.cpfFuncionario);
                    cmd.Parameters.AddWithValue("@cargoFuncionario", funcionario.cargoFuncionario);

                    cmd.Parameters.AddWithValue("@telefoneFuncionario", telefoneDB);


                    cmd.Parameters.AddWithValue("@FK_Endereco_id_endereco", funcionario.ID_endereco);

                    cmd.ExecuteNonQuery();
                }
            }

            catch (MySqlException sqlEx)
            {

                if (sqlEx.Number == 1062)
                {
                    throw new Exception("Cadastro de funcionário inválido. CPF duplicado.");
                }
                throw new Exception($"Erro de banco de dados ao criar funcionário: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException) { throw; }
                throw new Exception($"Erro ao criar funcionário: {ex.Message}");
            }
        }



        public void Update(Funcionario funcionario)
        {

            funcionario.nomeFuncionario = ValidarNome(funcionario.nomeFuncionario);
            funcionario.cpfFuncionario = ValidarCPF(funcionario.cpfFuncionario);
            funcionario.telefoneFuncionario = ValidarTelefone(funcionario.telefoneFuncionario);

            object telefoneDB = funcionario.telefoneFuncionario == null
              ? (object)DBNull.Value
              : funcionario.telefoneFuncionario;

            try
            {

                string sql = "UPDATE FUNCIONARIO SET nomeFuncionario = @nomeFuncionario, cpfFuncionario = @cpfFuncionario, cargoFuncionario = @cargoFuncionario, telefoneFuncionario = @telefoneFuncionario, FK_Endereco_id_endereco = @FK_Endereco_id_endereco WHERE ID_funcionario = @ID_funcionario";

                using (var conexao = Conexao.Conectar())
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@nomeFuncionario", funcionario.nomeFuncionario);
                    cmd.Parameters.AddWithValue("@cpfFuncionario", funcionario.cpfFuncionario);
                    cmd.Parameters.AddWithValue("@cargoFuncionario", funcionario.cargoFuncionario);

                    cmd.Parameters.AddWithValue("@telefoneFuncionario", telefoneDB);

                    cmd.Parameters.AddWithValue("@FK_Endereco_id_endereco", funcionario.ID_endereco);
                    cmd.Parameters.AddWithValue("@ID_funcionario", funcionario.ID_funcionario);

                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nenhum funcionário encontrado para atualização.");
                    }
                }
            }

            catch (MySqlException sqlEx)
            {

                if (sqlEx.Number == 1062)
                {
                    throw new Exception("Erro ao atualizar funcionário. O CPF informado já pertence a outro funcionário.");
                }
                throw new Exception($"Erro de banco de dados ao atualizar funcionário: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException) { throw; }
                throw new Exception($"Erro ao atualizar funcionário: {ex.Message}");
            }
        }



        public void Delete(int id_funcionario)
        {

            Funcionario funcionarioParaDeletar = GetById(id_funcionario);

            if (funcionarioParaDeletar == null)
            {
                throw new Exception("Nenhum funcionário encontrado com o ID fornecido para exclusão.");
            }

            int id_endereco_a_deletar = funcionarioParaDeletar.ID_endereco;


            try
            {
                string sqlFuncionario = "DELETE FROM FUNCIONARIO WHERE ID_funcionario = @ID_funcionario";

                using (var conexao = Conexao.Conectar())
                using (var cmdFuncionario = new MySqlCommand(sqlFuncionario, conexao))
                {
                    cmdFuncionario.Parameters.AddWithValue("@ID_funcionario", id_funcionario);
                    var linhasAfetadas = cmdFuncionario.ExecuteNonQuery();

                    if (linhasAfetadas == 0)
                    {
                   
                        throw new Exception("Nenhum funcionário encontrado com o ID fornecido para exclusão.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao deletar funcionário (etapa 1): {ex.Message}");
            }


            try
            {
                _enderecoDao.Delete(id_endereco_a_deletar);
            }
            catch (Exception ex)
            {
                throw new Exception($"Atenção: Funcionário deletado, mas falha ao deletar Endereço (ID: {id_endereco_a_deletar}): {ex.Message}");
            }
        }


        public Funcionario GetById(int id)
        {
            try
            {
                var sql = "SELECT * FROM FUNCIONARIO WHERE ID_funcionario = @ID_funcionario";
                Funcionario funcionario = null;

                using (var conexao = Conexao.Conectar())
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@ID_funcionario", id);
                    var dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        funcionario = new Funcionario
                        {
                            ID_funcionario = dr.GetInt32("ID_funcionario"),
                            nomeFuncionario = dr.GetString("nomeFuncionario"),
                            cpfFuncionario = dr.GetString("cpfFuncionario"),
                            cargoFuncionario = dr.GetString("cargoFuncionario"),

                            telefoneFuncionario = dr.IsDBNull(dr.GetOrdinal("telefoneFuncionario")) ? null : dr.GetString("telefoneFuncionario"),
                            ID_endereco = dr.GetInt32("FK_Endereco_id_endereco")
                        };
                    }
                    return funcionario;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar funcionário por ID: {ex.Message}");
            }
        }


        public List<Funcionario> GetAll()
        {
            List<Funcionario> listadeFuncionarios = new List<Funcionario>();

            try
            {
                var sql = "SELECT * FROM FUNCIONARIO ORDER BY nomeFuncionario";

                using (var conexao = Conexao.Conectar())
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Funcionario f = new Funcionario();

                        f.ID_funcionario = dr.GetInt32("ID_funcionario");
                        f.nomeFuncionario = dr.GetString("nomeFuncionario");
                        f.cpfFuncionario = dr.GetString("cpfFuncionario");
                        f.cargoFuncionario = dr.GetString("cargoFuncionario");

                        f.telefoneFuncionario = dr.IsDBNull(dr.GetOrdinal("telefoneFuncionario")) ? null : dr.GetString("telefoneFuncionario");
                        f.ID_endereco = dr.GetInt32("FK_Endereco_id_endereco");

                        listadeFuncionarios.Add(f);
                    }
                }
                return listadeFuncionarios;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar funcionários: {ex.Message}");
            }
        }
    }
}