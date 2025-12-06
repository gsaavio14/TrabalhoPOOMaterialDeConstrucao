using System;
using System.Collections.Generic;
using TrabalhoPOOMaterialDeConstrucaoGustavoSavio.Dao;
using TrabalhoPOOMaterialDeConstrucaoGustavoSavio.Modelos;
using TrabalhoPOOMaterialDeConstrucaoGustavoSavio.Utilitarios;

namespace TrabalhoPOOMaterialDeConstrucaoGustavoSavio
{
    public class Program
    {

        public static FuncionarioDao globalFuncionarioDao = new FuncionarioDao();
        public static EnderecoDao globalEnderecoDao = new EnderecoDao();

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


            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("|===========================================================================|");
                Console.WriteLine("|                                                                           |");
                Console.WriteLine("|    BEM VINDO AO MENU DE CADASTRO DO MATERIAL DE CONSTRUÇÃO GUSTAVO LTDA    |");
                Console.WriteLine("|                                                                           |");
                Console.WriteLine("|===========================================================================|");
                Console.WriteLine("|                                                                           |");
                Console.WriteLine("|                      1 - Cadastrar Funcionário                            |");
                Console.WriteLine("|                      2 - Listar Funcionários                              |");
                Console.WriteLine("|                      3 - Atualizar Funcionário                            |");
                Console.WriteLine("|                      4 - Deletar Funcionário                              |");
                Console.WriteLine("|                                                                           |");
                Console.WriteLine("|                          0 - Sair                                         |");
                Console.WriteLine("|                      Escolha uma opção:                                   |");
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
                        ListarFuncionarios();
                        break;
                    case "3":
                        AtualizarFuncionario();
                        break;
                    case "4":
                        DeletarFuncionario();
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

  
        public static void ListarFuncionarios()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("--- LISTA DE FUNCIONÁRIOS CADASTRADOS ---");
            Console.ResetColor();

            try
            {
                
                List<Funcionario> funcionarios = globalFuncionarioDao.GetAll();

                if (funcionarios.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nNão há funcionários cadastrados no banco de dados.");
                    Console.ResetColor();
                }
                else
                {
                   
                    Console.WriteLine("\n------------------------------------------------------------------------------------------------------------------");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"| {"ID",-5} | {"Nome Completo",-30} | {"CPF",-15} | {"Cargo",-20} | {"Telefone",-15} | {"ID Endereço",-12} |");
                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------");
                    Console.ResetColor();

                
                    foreach (Funcionario f in funcionarios)
                    {
                        Console.WriteLine($"| {f.ID_funcionario,-5} | {f.nomeFuncionario,-30} | {f.cpfFuncionario,-15} | {f.cargoFuncionario,-20} | {f.telefoneFuncionario,-15} | {f.ID_endereco,-12} |");
                    }
                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nTotal de Funcionários: {funcionarios.Count}");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nERRO ao listar funcionários: {ex.Message}");
                Console.ResetColor();
            }

            Console.WriteLine("\nPressione ENTER para retornar ao Menu.");
            Console.ReadLine();
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


                try
                {
                    Console.WriteLine("\nTentando excluir o endereço criado...");
                    Program.globalEnderecoDao.Delete(f.ID_endereco);
                    Console.WriteLine("Endereço revertido com sucesso. Operação de cadastro cancelada.");
                }
                catch (Exception deleteEx)
                {
                    Console.WriteLine($"AVISO: Falha ao deletar o endereço órfão (ID: {f.ID_endereco}): {deleteEx.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n ERRO ao cadastrar funcionário: {ex.Message}");
                Console.ResetColor();

                try
                {
                    Console.WriteLine("\nTentando excluir o endereço criado...");
                    Program.globalEnderecoDao.Delete(f.ID_endereco);
                    Console.WriteLine("Endereço revertido com sucesso. Operação de cadastro cancelada.");
                }
                catch (Exception deleteEx)
                {
                    Console.WriteLine($"AVISO: Falha ao deletar o endereço órfão (ID: {f.ID_endereco}): {deleteEx.Message}");
                }
            }

            Console.WriteLine("\nPressione ENTER para retornar ao Menu.");
            Console.ReadLine();
        }

        public static void AtualizarFuncionario()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n--- ATUALIZAÇÃO DE FUNCIONÁRIO ---");
            Console.ResetColor();

            Console.Write("Digite o ID do funcionário que deseja atualizar: ");
            if (!int.TryParse(Console.ReadLine(), out int idFuncionario))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nID inválido. Pressione ENTER para retornar ao Menu.");
                Console.ResetColor();
                Console.ReadLine();
                return;
            }

            Funcionario f = null;
            Endereco e = null;

            try
            {

                f = globalFuncionarioDao.GetById(idFuncionario);
                if (f == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nFuncionário com ID {idFuncionario} não encontrado.");
                    Console.ResetColor();
                    Console.WriteLine("Pressione ENTER para retornar ao Menu.");
                    Console.ReadLine();
                    return;
                }


                e = globalEnderecoDao.GetById(f.ID_endereco);
                if (e == null)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\nAVISO: Endereço (ID: {f.ID_endereco}) do funcionário não foi encontrado. Apenas os dados do funcionário serão atualizados.");
                    Console.ResetColor();
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nFuncionário encontrado: {f.nomeFuncionario} (CPF: {f.cpfFuncionario})");
                Console.ResetColor();
                Console.WriteLine("\nPreencha os novos dados. Deixe o campo em branco para manter o valor atual.");


                Console.WriteLine("\n-- Atualizar Dados do Funcionário --");

                Console.Write($"Nome Completo (Atual: {f.nomeFuncionario}): ");
                string novoNome = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(novoNome)) f.nomeFuncionario = novoNome;

                Console.Write($"CPF (Atual: {f.cpfFuncionario}): ");
                string novoCpf = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(novoCpf)) f.cpfFuncionario = novoCpf;

                Console.Write($"Cargo (Atual: {f.cargoFuncionario}): ");
                string novoCargo = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(novoCargo)) f.cargoFuncionario = novoCargo;

                Console.Write($"Telefone (Atual: {f.telefoneFuncionario ?? "Vazio"}): ");
                string novoTelefone = Console.ReadLine();

                if (novoTelefone.ToLower() == "null" || novoTelefone.ToLower() == "vazio")
                    f.telefoneFuncionario = null;
                else if (!string.IsNullOrWhiteSpace(novoTelefone))
                    f.telefoneFuncionario = novoTelefone;



                if (e != null)
                {
                    Console.WriteLine("\n-- Atualizar Dados do Endereço --");

                    Console.Write($"Cidade (Atual: {e.cidade}): ");
                    string novaCidade = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(novaCidade)) e.cidade = novaCidade;

                    Console.Write($"Estado (Atual: {e.estado}): ");
                    string novoEstado = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(novoEstado)) e.estado = novoEstado;

                    Console.Write($"Bairro (Atual: {e.bairro}): ");
                    string novoBairro = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(novoBairro)) e.bairro = novoBairro;

                    Console.Write($"Rua (Atual: {e.rua}): ");
                    string novaRua = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(novaRua)) e.rua = novaRua;

                    Console.Write($"Número (Atual: {e.numero}): ");
                    string novoNumero = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(novoNumero)) e.numero = novoNumero;

                    Console.Write($"CEP (Atual: {e.cep}): ");
                    string novoCep = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(novoCep)) e.cep = novoCep;

                    Console.Write($"Logradouro (Atual: {e.logradouro ?? "Vazio"}): ");
                    string novoLogradouro = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(novoLogradouro)) e.logradouro = novoLogradouro;
                    else if (novoLogradouro == string.Empty) e.logradouro = null;

                    Console.Write($"Referência (Atual: {e.referencia ?? "Vazio"}): ");
                    string novaReferencia = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(novaReferencia)) e.referencia = novaReferencia;
                    else if (novaReferencia == string.Empty) e.referencia = null;
                }


                if (e != null)
                {
                    globalEnderecoDao.Update(e);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\nEndereço atualizado com sucesso!");
                    Console.ResetColor();
                }


                globalFuncionarioDao.Update(f);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nFuncionário {f.nomeFuncionario} (ID: {f.ID_funcionario}) atualizado com sucesso!");
                Console.ResetColor();
            }
            catch (ArgumentException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nERRO DE VALIDAÇÃO: {ex.Message}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nERRO AO ATUALIZAR: {ex.Message}");
                Console.ResetColor();
            }

            Console.WriteLine("\nPressione ENTER para retornar ao Menu.");
            Console.ReadLine();
        }

        public static void DeletarFuncionario()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n--- EXCLUSÃO DE FUNCIONÁRIO ---");
            Console.ResetColor();

            Console.Write("Digite o ID do Funcionário que deseja deletar permanentemente: ");

            if (int.TryParse(Console.ReadLine(), out int idFuncionario))
            {

                try
                {

                    Funcionario f = globalFuncionarioDao.GetById(idFuncionario);

                    if (f == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"\n ERRO: Funcionário com ID {idFuncionario} não encontrado.");
                        Console.ResetColor();
                        Console.WriteLine("Pressione ENTER para retornar ao Menu.");
                        Console.ReadLine();
                        return;
                    }

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"\nCONFIRMA EXCLUSÃO do funcionário **{f.nomeFuncionario}** (ID: {idFuncionario}) e seu endereço? (S/N): ");
                    Console.ResetColor();

                    if (Console.ReadLine().ToUpper() == "S")
                    {
                        globalFuncionarioDao.Delete(idFuncionario);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n Funcionário **{f.nomeFuncionario}** e seu endereço foram deletados com sucesso!");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine("\nOperação de exclusão cancelada.");
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n ERRO ao deletar funcionário: {ex.Message}");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n ID inválido. Por favor, digite um número.");
                Console.ResetColor();
            }

            Console.WriteLine("\nPressione ENTER para retornar ao Menu.");
            Console.ReadLine();
        }
    }
}