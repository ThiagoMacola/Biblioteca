using Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controls
{
	public class ClienteControle
	{


		//Registra o Cliente
		//public static int IdCliente = 0;
		public static void Registrar(List<Cliente> lista)
		{
			// VARIAVEIS
			long idcliente = 0;
			string cpf, nome, telefone;
			string logradouro, bairro, cidade, estado, cep;
			DateTime dNascimento;

			//Insere o id do cliente de maneira sequencial e automatica
			if (lista.Count == 0)
				idcliente = 1;
			else
				idcliente = lista.Count + 1;
	
			cpf = LerCpf(lista); // VERIFICA CPF VALIDO 

			//VERIFICA SE CPF JÁ EXISTE NA LISTA
			if (CpfRepitido(lista, cpf))
			{
				Console.WriteLine("CPF já cadastrado");
				
			}
			else
			{
				nome = LeiaNome(); // LE O NOME 

				telefone = LeiaTelefone(); //LE O TELEFONE

				dNascimento = LeiaDNascimento(); // LE A DATA DE NASCIMENTO

				logradouro = LeiaLogradouro(); // LE LOGRADOURO

				bairro = LeiaBairro(); // LE BAIRRO

				cidade = LeiaCidade(); // LE CIDADE

				estado = LeiaEstado(); // LE ESTADO

				cep = LeiaCep(); // LE CEP

				Endereco novoEndereco = new Endereco()
				{
					Logradouro = logradouro,
					Bairro = bairro,
					Cidade = cidade,
					Estado = estado,
					Cep = cep,
				};

				Cliente novoCLiente = new Cliente()
				{
					IdCliente = idcliente,
					Cpf = cpf,
					Nome = nome,
					DataNascimento = dNascimento,
					Telefone = telefone,
					Endereco = novoEndereco
				};

				lista.Add(novoCLiente);
				lista = lista.OrderBy(x => x.IdCliente).ToList();
				EscreverArquivo(lista);
				Console.Clear();
				Console.WriteLine("Dados do cliente Cadastrados com sucesso");
				Console.WriteLine("Aperte qualquer tecla para voltar ao menu principal");
				Console.ReadKey();
				Console.Clear();
			}
			
		}

		// RETORNA CLIENTE NO FORMATO PARA ARQUIVO
		private static string FormatoParaArquivo(Cliente c)
		{
			return c.IdCliente.ToString().PadRight(5, ' ') + ";" + c.Cpf+";" + c.Nome.PadRight(50, ' ')+";" + c.DataNascimento.ToString("dd/MM/yyyy") + ";" +
			c.Telefone.PadRight(11, ' ') + ";" + c.Endereco.Logradouro.PadRight(30, ' ') + ";" + c.Endereco.Bairro.PadRight(15, ' ') + ";" + c.Endereco.Cidade.PadRight(15, ' ') + ";" + c.Endereco.Estado.PadRight(2, ' ') + ";"+ c.Endereco.Cep;
		}

		// ESCRITA DE ARQUIVO
		public static void EscreverArquivo(List<Cliente> listaCliente)
		{
			using (StreamWriter file = new StreamWriter(@"C:\Arquivos\CLIENTE.CSV"))
			{
				foreach (Cliente c in listaCliente)
					file.WriteLine(FormatoParaArquivo(c)); // ESCREVE A LISTA NO ARQUIVO SEPARADOS POR QUEBRA LINHA
			}
		}

		//LE O CPF E CHAMA OS MÉTODOS DE VERIFICAÇÃO
		public static string LerCpf(List<Cliente> lista)
		{
			string cpf;

			do
			{ // LAÇO VERIFICA CPF SE É VALIDO 
				Console.Write("Informe o CPF do cliente para validação (Apenas números e com o dígito): ");
				cpf = Console.ReadLine();

				if (!CpfEhValido(cpf))
					Console.WriteLine("CPF inválido");


			} while (!(CpfEhValido(cpf)));
			    

			return cpf;
		}

		// VERIFICA CPF REPETE
		public static bool CpfRepitido(List<Cliente> lista, string cpf)
		{
			foreach (Cliente i in lista)
			{
				if (i.Cpf.Equals(cpf))
					return true;
			}
			return false;
		}

		//MATEMATICA PARA VALIDAÇÃO DE CPF
		public static bool CpfEhValido(string value)
		{
			int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
			int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
			string tempCpf;
			string digito;
			int soma;
			int resto;

			value = value.Trim();
			value = value.Replace(".", "").Replace("-", "");

			if (value.Length != 11)
				return false;

			tempCpf = value.Substring(0, 9);
			soma = 0;

			for (int i = 0; i < 9; i++)
				soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

			resto = soma % 11;
			if (resto < 2)
				resto = 0;
			else
				resto = 11 - resto;

			digito = resto.ToString();

			tempCpf = tempCpf + digito;

			soma = 0;
			for (int i = 0; i < 10; i++)
				soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

			resto = soma % 11;
			if (resto < 2)
				resto = 0;
			else
				resto = 11 - resto;

			digito = digito + resto.ToString();

			return value.EndsWith(digito);
		}

		// LE NOME
		private static string LeiaNome()
		{
			string nome;

			do
			{ // LAÇO PARA NÃO DEIXAR CAMPO VAZIO E COM O TAMANHO MAXIMO PARA O ARQUIVO
				Console.Write("Informe o Nome: ");
				nome = Console.ReadLine();
				if (nome.Length > 50)
					Console.WriteLine("Maximo 50 caracteres no nome do cliente");

			} while (nome == "" || nome.Length > 50);
			return nome;
		}

		// LE TELEFONE
		private static string LeiaTelefone()
		{
			string telefone;

			do
			{ // LAÇO PARA NÃO DEIXAR CAMPO VAZIO E COM O TAMANHO MAXIMO PARA O ARQUIVO
				Console.Write("Informe o Telefone: ");
				telefone = Console.ReadLine();
				if (telefone.Length > 11)
					Console.WriteLine("Maximo 11 digitos no telefone do cliente");

			} while (telefone == "" || telefone.Length > 11);
			return telefone;
		}

		// LE DATA DE NASCIMENTO
		private static DateTime LeiaDNascimento()
		{
			DateTime dNascimento;
			CultureInfo CultureBr = new CultureInfo(name: "pt-BR"); // DATA NO FORMATO BRASILEIRO
			Console.Write("Informe sua Data de Nascimento: ");
			dNascimento = DateTime.ParseExact(Console.ReadLine(), "d", CultureBr); // CONVERTE FORMATO DATA INGLESA PRA BRASILEIRA;

			return dNascimento;
		}

		// LE LOGRADOURO
		private static string LeiaLogradouro()
		{
			string logradouro;

			do
			{ // LAÇO PARA NÃO DEIXAR CAMPO VAZIO E COM O TAMANHO MAXIMO PARA O ARQUIVO
				Console.Write("Informe o Logradouro: ");
				logradouro = Console.ReadLine();
				if (logradouro.Length > 30)
					Console.WriteLine("maximo 30 caracteres no nome do logradouro");

			} while (logradouro == "" || logradouro.Length > 30);
			return logradouro;
		}

		// LE BAIRRO
		private static string LeiaBairro()
		{
			string bairro;

			do
			{ // LAÇO PARA NÃO DEIXAR CAMPO VAZIO E COM O TAMANHO MAXIMO PARA O ARQUIVO
				Console.Write("Informe o Bairro: ");
				bairro = Console.ReadLine();
				if (bairro.Length > 15)
					Console.WriteLine("Maximo 15 caracteres no nome do bairro");

			} while (bairro == "" || bairro.Length > 15);
			return bairro;
		}

		// LE CIDADE
		private static string LeiaCidade()
		{
			string cidade;

			do
			{ // LAÇO PARA NÃO DEIXAR CAMPO VAZIO E COM O TAMANHO MAXIMO PARA O ARQUIVO
				Console.Write("Informe a Cidade: ");
				cidade = Console.ReadLine();
				if (cidade.Length > 15)
					Console.WriteLine("Nome da cidade tem que ter no maximo 15 caracteres");

			} while (cidade == "" || cidade.Length > 15);
			return cidade;
		}

		// LE ESTADO
		private static string LeiaEstado()
		{
			string estado;

			do
			{ // LAÇO PARA NÃO DEIXAR CAMPO VAZIO E COM O TAMANHO MAXIMO PARA O ARQUIVO
				Console.Write("Informe o Estado: ");
				estado = Console.ReadLine();
				if (estado.Length > 2)
					Console.WriteLine("Maximo 2 caracteres no campo estado");

			} while (estado == "" || estado.Length > 2);
			return estado;
		}

		// LE CEP
		private static string LeiaCep()
		{
			string cep;

			do
			{ // LAÇO PARA NÃO DEIXAR CAMPO VAZIO E COM O TAMANHO MAXIMO PARA O ARQUIVO
				Console.Write("Informe o Cep: ");
				cep = Console.ReadLine();
				if (cep.Length > 8)
					Console.WriteLine("Maximo 8 caracteres no campo Cep");

			} while (cep == "" || cep.Length > 8);
			return cep;
		}


		public static Cliente RetorneCliente(long buscarIdCliente)
		{
			bool clienteEncontrado = false;
			List<Cliente> Clientes = ConverteParaLista();
			foreach (var cliente in Clientes)
			{
				if (cliente.IdCliente == buscarIdCliente) // existe cliente
				{
					return cliente;

				}
			}
			if (!clienteEncontrado)
				Console.WriteLine("Cliente não cadastrado");
			return null;
		}

		//LE O ARQUIVO E TRANSFORMA EM LISTA
		public static List<Cliente> ConverteParaLista()
		{
			List<Cliente> Clientes = new List<Cliente>();
			try
			{
				ManipuladorArquivo file = new ManipuladorArquivo() { Path = @"C:\Arquivos\", Name = "CLIENTE.CSV" };
				string[] clienteArquivadas = ManipuladorArquivoControle.LerArquivo(file);


				foreach (var cliente in clienteArquivadas)
				{
					if (cliente.Length == 166)
					{
						string idcliente = cliente.Substring(0, 5);
						string cpf = cliente.Substring(6, 11);
						string nome = cliente.Substring(18, 50);
						string dnascimento = cliente.Substring(69, 10);
						string telefone = cliente.Substring(80, 10);
						string logradouro = cliente.Substring(92, 29);
						string bairro = cliente.Substring(123, 14);
						string cidade = cliente.Substring(139, 14);
						string estado = cliente.Substring(155, 2);
						string cep = cliente.Substring(158, 8);


						Endereco novoEndereco = new Endereco()
						{
							Logradouro = logradouro,
							Bairro = bairro,
							Cidade = cidade,
							Estado = estado,
							Cep = cep,
						};
						Cliente Cliente = new Cliente()
						{
							IdCliente = long.Parse(idcliente),
							Cpf = cpf,
							Nome = nome,
							DataNascimento = Convert.ToDateTime(dnascimento),
							Telefone = telefone,
							Endereco = novoEndereco
						};

						Clientes.Add(Cliente);
					}
				}
			}
			catch (FileNotFoundException ex)
			{
				Console.WriteLine("ERRO!!!!: " + ex.Message);
				Console.ReadKey();
			}

			return Clientes;
		}
	
	}
}
