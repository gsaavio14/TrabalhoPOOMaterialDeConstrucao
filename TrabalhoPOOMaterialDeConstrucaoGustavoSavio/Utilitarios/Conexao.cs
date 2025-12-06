


using MySql.Data.MySqlClient;
using System;


namespace TrabalhoPOOMaterialDeConstrucaoGustavoSavio.Utilitarios
{
    internal class Conexao
    {
        private const string strconexao = "server=localhost;port=3306;uid=root;pwd=ROOT;database=TrabalhoPOOGustavo";



        public static MySqlConnection Conectar()
        {
            MySqlConnection conexao = new MySqlConnection(strconexao);
            try
            {

                conexao.Open();
                return conexao;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }
    }
}
