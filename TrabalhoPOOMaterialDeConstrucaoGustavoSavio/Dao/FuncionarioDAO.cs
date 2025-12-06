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
                
                string sql = "INSERT INTO FUNCIONARIO (nomeFuncionario, cpfFuncionario, cargoFuncionario, telefoneFuncionario, FK_Endereco_id_endereco) VALUES (@nomeFuncionario, @cpfFuncionario, @cargoFuncionario, @telefoneFuncionario, @ID_endereco)";

                using (var conexao = Conexao.Conectar())
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@nomeFuncionario", funcionario.nomeFuncionario);
                    cmd.Parameters.AddWithValue("@cpfFuncionario", funcionario.cpfFuncionario);
                    cmd.Parameters.AddWithValue("@cargoFuncionario", funcionario.cargoFuncionario);

                    cmd.Parameters.AddWithValue("@telefoneFuncionario", telefoneDB);

                    cmd.Parameters.AddWithValue("@ID_endereco", funcionario.ID_endereco);

                    cmd.ExecuteNonQuery();
                }
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
                
                string sql = "UPDATE FUNCIONARIO SET nomeFuncionario = @nomeFuncionario, cpfFuncionario = @cpfFuncionario, cargoFuncionario = @cargoFuncionario, telefoneFuncionario = @telefoneFuncionario, FK_Endereco_id_endereco = @ID_endereco WHERE ID_funcionario = @ID_funcionario";

                using (var conexao = Conexao.Conectar())
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@nomeFuncionario", funcionario.nomeFuncionario);
                    cmd.Parameters.AddWithValue("@cpfFuncionario", funcionario.cpfFuncionario);
                    cmd.Parameters.AddWithValue("@cargoFuncionario", funcionario.cargoFuncionario);

                    cmd.Parameters.AddWithValue("@telefoneFuncionario", telefoneDB);

                    cmd.Parameters.AddWithValue("@ID_endereco", funcionario.ID_endereco);
                    cmd.Parameters.AddWithValue("@ID_funcionario", funcionario.ID_funcionario);

                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nenhum funcionário encontrado para atualização.");
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException) { throw; }
                throw new Exception($"Erro ao atualizar funcionário: {ex.Message}");
            }
        }

     
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Funcionario GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<Funcionario> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}