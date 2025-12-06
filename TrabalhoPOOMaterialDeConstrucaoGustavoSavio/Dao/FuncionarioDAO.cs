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
    internal class FuncionarioDao : IDao<Funcionario>
    {
       
        public void Create(Funcionario funcionario)
        {
            try
            {
                
                string sql = @"INSERT INTO Funcionario (nomeFuncionario, cpfFuncionario, cargoFuncionario, telefoneFuncionario, FK_Endereco_id_endereco) 
                               VALUES (@NomeFuncionario, @CpfFuncionario, @CargoFuncionario, @TelefoneFuncionario, @FK_Endereco_id_endereco)";

                using (var conexao = Conexao.Conectar())
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    
                    cmd.Parameters.AddWithValue("@NomeFuncionario", funcionario.nomeFuncionario);
                    cmd.Parameters.AddWithValue("@CpfFuncionario", funcionario.cpfFuncionario); 
                    cmd.Parameters.AddWithValue("@CargoFuncionario", funcionario.cargoFuncionario); 
                    cmd.Parameters.AddWithValue("@TelefoneFuncionario", funcionario.telefoneFuncionario);

                    
                    cmd.Parameters.AddWithValue("@FK_Endereco_id_endereco", funcionario.ID_endereco);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar funcionário: {ex.Message}");
            }
        }

        
        public void Insert(Funcionario funcionario)
        {
            
        }

       
        public void Update(Funcionario funcionario)
        {
            try
            {
                
                string sql = @"UPDATE Funcionario SET nomeFuncionario = @NomeFuncionario, cpfFuncionario = @CpfFuncionario, cargoFuncionario = @CargoFuncionario, telefoneFuncionario = @TelefoneFuncionario, FK_Endereco_id_endereco = @FK_Endereco_id_endereco where ID_funcionario = @ID_funcionario";

                using (var conexao = Conexao.Conectar())
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    
                    cmd.Parameters.AddWithValue("@NomeFuncionario", funcionario.nomeFuncionario);
                    cmd.Parameters.AddWithValue("@CpfFuncionario", funcionario.cpfFuncionario);
                    cmd.Parameters.AddWithValue("@CargoFuncionario", funcionario.cargoFuncionario);
                    cmd.Parameters.AddWithValue("@TelefoneFuncionario", funcionario.telefoneFuncionario);
                    cmd.Parameters.AddWithValue("@FK_Endereco_id_endereco", funcionario.ID_endereco);

                    cmd.Parameters.AddWithValue("@ID_funcionario", funcionario.ID_funcionario);

                    var linhas = cmd.ExecuteNonQuery();

                    if (linhas == 0)
                    {
                        throw new Exception("Nenhum registro foi atualizado (verifique o ID_funcionario).");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar funcionário: {ex.Message}");
            }
        }

      
        public void Delete(int id_funcionario)
        {
            try
            {
                string sql = "DELETE FROM Funcionario WHERE ID_funcionario = @ID_funcionario";

                using (var conexao = Conexao.Conectar())
                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@ID_funcionario", id_funcionario);

                    var linhasAfetadas = cmd.ExecuteNonQuery();

                    if (linhasAfetadas == 0)
                    {
                        throw new Exception("Nenhum registro encontrado com esse ID.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao deletar funcionário: {ex.Message}");
            }
        }

       
        public List<Funcionario> GetAll()
        {
            List<Funcionario> listadeFuncionarios = new List<Funcionario>();

            try
            {
               
                var sql = "SELECT * FROM Funcionario ORDER BY nomeFuncionario";

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
                        f.telefoneFuncionario = dr.GetString("telefoneFuncionario");
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
