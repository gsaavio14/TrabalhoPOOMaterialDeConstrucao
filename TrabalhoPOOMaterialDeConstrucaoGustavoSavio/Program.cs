using System;
using System.Collections.Generic;
using TrabalhoPOOMaterialDeConstrucaoGustavoSavio.Dao;
using TrabalhoPOOMaterialDeConstrucaoGustavoSavio.Modelos;
using TrabalhoPOOMaterialDeConstrucaoGustavoSavio.Utilitarios;

namespace TrabalhoPOOMaterialDeConstrucaoGustavoSavio
{
    public class Program
    {
       
        internal static FuncionarioDao globalFuncionarioDao = new FuncionarioDao();
        internal static EnderecoDao globalEnderecoDao = new EnderecoDao();

        public static void Main(string[] args)
        {
           
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Conexao.Conectar();
                Console.WriteLine("Conexão com o banco de dados estabelecida com sucesso!");
                Console.WriteLine();
                Console.WriteLine();
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERRO CRÍTICO: Não foi possível conectar ao banco de dados: {ex.Message}");
                Console.ResetColor();
                return;
            }


            // --- Menu Principal ---
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("|===========================================================================|");
                Console.WriteLine("|                                                                           |");
                Console.WriteLine("|    BEM VINDO AO MENU DE CADASTRO DO MATERIAL DE CONSTRUÇÃO GUSTAVO LTDA   |");
                Console.WriteLine("|                                                                           |");
                Console.WriteLine("|===========================================================================|");
                Console.WriteLine("|                                                                           |");
                Console.WriteLine("|                      1 - Cadastrar Funcionário                            |");
                Console.WriteLine("|                2 - Cadastrar Cliente (Ainda não implementado)             |");
                Console.WriteLine("|                          0 - Sair                                         |");
                Console.WriteLine("|                       Escolha uma opção:                                  |");
                Console.WriteLine("|                                                                           |");
                Console.WriteLine("|===========================================================================|");


                Console.ResetColor();
                Console.WriteLine("----------------------------------");
                string escolha = Console.ReadLine();

                switch (escolha)
                {
                    case "1":
                        CadastrarFuncionario();
                        break;
                    case "2":
                        Console.WriteLine("Opção indisponível no momento. Pressione ENTER para continuar.");
                        Console.ReadLine();
                        break;
                    case "0":
                        Console.WriteLine("Encerrando o sistema...");
                        return;
                    default:
                        Console.WriteLine("Opção inválida. Pressione ENTER para tentar novamente.");
                        Console.ReadLine();
                        break;
                }
            }
        } 


        public static void CadastrarFuncionario()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n---  CADASTRO DE NOVO FUNCIONÁRIO ---");
            Console.ResetColor();

        
            Console.WriteLine("\n-- Dados do Endereço --");
            Endereco e = new Endereco();

            Console.Write("Cidade: ");
            e.cidade = Console.ReadLine();
            Console.Write("Estado (Ex: RO): ");
            e.estado = Console.ReadLine();
            Console.Write("Bairro: ");
            e.bairro = Console.ReadLine();
            Console.Write("Rua: ");
            e.rua = Console.ReadLine();
            Console.Write("Número: ");
            e.numero = Console.ReadLine();
            Console.Write("CEP (8 dígitos): ");
            e.cep = Console.ReadLine();
            Console.Write("Logradouro (Opcional - Enter para pular): ");
            e.logradouro = Console.ReadLine();
            Console.Write("Referência (Opcional - Enter para pular): ");
            e.referencia = Console.ReadLine();

            int novoIdEndereco = 0;

            try
            {
               
                novoIdEndereco = Program.globalEnderecoDao.CreateAndGetId(e);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n Endereço salvo com sucesso. ID {novoIdEndereco} gerado.");
                Console.ResetColor();
            }
            catch (ArgumentException ex) 
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n ERRO DE VALIDAÇÃO DO ENDEREÇO: {ex.Message}");
                Console.ResetColor();
                Console.WriteLine("Pressione ENTER para retornar ao Menu.");
                Console.ReadLine();
                return; 
            }
            catch (Exception ex) 
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n ERRO GERAL ao salvar endereço: {ex.Message}");
                Console.ResetColor();
                Console.WriteLine("Pressione ENTER para retornar ao Menu.");
                Console.ReadLine();
                return;
            }

           
            Console.WriteLine("\n-- Dados do Funcionário --");
            Funcionario f = new Funcionario();

            Console.Write("Nome Completo: ");
            f.nomeFuncionario = Console.ReadLine();
            Console.Write("CPF (11 dígitos, Ex: 123.456.789-00): ");
            f.cpfFuncionario = Console.ReadLine();
            Console.Write("Cargo: ");
            f.cargoFuncionario = Console.ReadLine();
            Console.Write("Telefone: ");
            f.telefoneFuncionario = Console.ReadLine();

           
            f.ID_endereco = novoIdEndereco;

            try
            {
               
                Program.globalFuncionarioDao.Create(f);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n Funcionário {f.nomeFuncionario}  cadastrado com sucesso!");
                Console.ResetColor();
            }
            catch (ArgumentException ex) 
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n ERRO DE VALIDAÇÃO DO FUNCIONÁRIO: {ex.Message}");
                Console.ResetColor();
            }
            catch (Exception ex) 
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n ERRO ao cadastrar funcionário: {ex.Message}");
                Console.ResetColor();
            }

            Console.WriteLine("\nPressione ENTER para retornar ao Menu.");
            Console.ReadLine();
        }
    } 
} 