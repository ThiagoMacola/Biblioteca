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
	public class ControladorEmprestimo
	{
		public static void Registrar(List<EmprestimoLivro> listaEmprestimo, List<Livro> listaLivro,List<Cliente> listaCliente, List<StatusEmprestimo> listaStatus)
		{
			// VARIAVEIS
			string cpf;
			long idcliente = 0, numeroTombo;
			DateTime demprestimo, ddevolucao;
			int status = 0;
			int statusfim = 0;
			int encontradoNumeroTombo = 0;
			int encontradoCpf = 0;
			string nomelivro = "";
			string cpfstatus = "";
			
			do
			{
				numeroTombo = LeiaNumeroTombo();


				foreach (var elemento in listaEmprestimo)
				{
					if (elemento.NumeroTombo == numeroTombo && elemento.StatusEmprestimo == 1)
					{
						Console.WriteLine("Livro emprestado, aguarde a sua devolução");
						statusfim = 1;
					}

				}


				if (statusfim != 1)
				{
						foreach (var elemento in listaLivro)
						{
							if (elemento.NumeroTombo == numeroTombo)
							{
								encontradoNumeroTombo++;
								nomelivro = elemento.Titulo;
							}
						}
					if (encontradoNumeroTombo == 0)
						Console.WriteLine("Livro não está disponivel para emprestimo");
					else
					{

						cpf = LerCpf();

						foreach (var elemento in listaCliente)
						{
							if (elemento.Cpf.ToString() == cpf)
							{
								idcliente = elemento.IdCliente;
								encontradoCpf++;
								cpfstatus = elemento.Cpf;

							}

						}
						if (encontradoCpf == 0)
							Console.WriteLine("Cliente não cadastrado");
						else
						{
							demprestimo = DateTime.Now;
							Console.Write("Data do Emprestimo: " + demprestimo + "\n");

							ddevolucao = LeiaDevolucao();

							status = 1;



							StatusEmprestimo novoStatus = new StatusEmprestimo()
							{
								Cpf = cpfstatus,
								Titulo = nomelivro,
								Status = status,
								DataEmprestimo = demprestimo,
								DataDevolucao = ddevolucao

							};


							EmprestimoLivro novoEmprestimo = new EmprestimoLivro()
							{
								IdCliente = idcliente,
								NumeroTombo = numeroTombo,
								DataEmprestimo = demprestimo,
								DataDevolucao = ddevolucao,
								StatusEmprestimo = status
							};

							listaStatus.Add(novoStatus);
							listaEmprestimo.Add(novoEmprestimo);
							listaEmprestimo = listaEmprestimo.OrderBy(x => x.IdCliente).ToList();
							listaStatus = listaStatus.OrderBy(x => x.Cpf).ToList();
							EscreverArquivo(listaEmprestimo);
							Console.Clear();
							Console.WriteLine("Emprestimo Cadastrado");
							Console.WriteLine("Aperte qualquer tecla para voltar ao menu principal");
							Console.ReadKey();
							Console.Clear();
						}
					}
				}	
			} while (encontradoCpf == 0 && statusfim != 1);
		}
		// LE NumeroTombo
		private static long LeiaNumeroTombo()
		{
			long numeroTombo;

			do
			{ // LAÇO PARA NÃO DEIXAR CAMPO VAZIO E COM O TAMANHO MAXIMO PARA O ARQUIVO
				Console.Write("Informe o NumeroTombo: ");
				numeroTombo = long.Parse(Console.ReadLine());
				if (numeroTombo > 999999)
					Console.WriteLine("Maximo 5 digitos");

			} while (numeroTombo > 999999);
			return numeroTombo;
		}

		//LE O CPF 
		public static string LerCpf()
		{
			string cpf;


			// LAÇO VERIFICA CPF SE É VALIDO 
			Console.Write("Informe o CPF do cliente: ");
			cpf = Console.ReadLine();
			return cpf;
		}

		// LE DATA DE Devolução
		private static DateTime LeiaDevolucao()
		{
			DateTime devolucao;
			CultureInfo CultureBr = new CultureInfo(name: "pt-BR"); // DATA NO FORMATO BRASILEIRO
			Console.Write("Informe sua Data de Devolução: ");
			devolucao = DateTime.ParseExact(Console.ReadLine(), "d", CultureBr); // CONVERTE FORMATO DATA INGLESA PRA BRASILEIRA;

			return devolucao;
		}

		//LE O ARQUIVO E TRANSFORMA EM LISTA
		public static List<Livro> ConverterParaListaLivro()
		{
			List<Livro> Livros = new List<Livro>();
			try
			{
				ManipuladorArquivo file = new ManipuladorArquivo() { Path = @"C:\Arquivos\", Name = "LIVRO.CSV" };
				string[] livroArquivados = ManipuladorArquivoControle.LerArquivo(file);


				foreach (var livro in livroArquivados)
				{
					if (livro.Length == 149)
					{
						string numerotombo = livro.Substring(0, 5);
						string isbn = livro.Substring(6, 13);
						string titulo = livro.Substring(20, 50);
						string genero = livro.Substring(71, 15);
						string datapublicacao = livro.Substring(87, 10);
						string autor = livro.Substring(98, 50);


						Livro Livro = new Livro()
						{
							NumeroTombo = long.Parse(numerotombo),
							Isbn = isbn,
							Titulo = titulo,
							Genero = genero,
							DataPublicacao = Convert.ToDateTime(datapublicacao),
							Autor = autor
						};

						Livros.Add(Livro);
					}
				}
			}
			catch (FileNotFoundException ex)
			{
				Console.WriteLine("ERRO!!!!: " + ex.Message);
				Console.ReadKey();
			}

			return Livros;
		}
		public static List<Cliente> ConverteParaListaCliente()
		{
			List<Cliente> Clientes = new List<Cliente>();
			try
			{
				ManipuladorArquivo file = new ManipuladorArquivo() { Path = @"C:\Arquivos\", Name = "CLIENTE.CSV" };
				string[] clienteArquivados = ManipuladorArquivoControle.LerArquivo(file);


				foreach (var cliente in clienteArquivados)
				{
					if (cliente.Length == 166)
					{
						string idcliente = cliente.Substring(0, 5);
						string cpf = cliente.Substring(6, 11);
						string nome = cliente.Substring(18, 50);
						string dnascimento = cliente.Substring(70, 10);
						string telefone = cliente.Substring(81, 10);
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
		public static List<EmprestimoLivro> ConverteParaListaEmprestimo()
		{
			List<EmprestimoLivro> EmprestimoLivro = new List<EmprestimoLivro>();
			try
			{
				ManipuladorArquivo file = new ManipuladorArquivo() { Path = @"C:\Arquivos\", Name = "EMPRESTIMO.CSV" };
				string[] emprestimoArquivados = ManipuladorArquivoControle.LerArquivo(file);


				foreach (var emprestado in emprestimoArquivados)
				{
					if (emprestado.Length == 35)
					{
						string idcliente = emprestado.Substring(0, 5);
						string numerotombo = emprestado.Substring(6, 5);
						string dataemprestimo = emprestado.Substring(12, 10);
						string datadevolucao = emprestado.Substring(23, 10);
						string statusemprestimo = emprestado.Substring(34, 1);



						EmprestimoLivro novoEmprestimo = new EmprestimoLivro()
						{
							IdCliente = long.Parse(idcliente),
							NumeroTombo = long.Parse(numerotombo),
							DataEmprestimo = Convert.ToDateTime(dataemprestimo),
							DataDevolucao = Convert.ToDateTime(datadevolucao),
							StatusEmprestimo = int.Parse(statusemprestimo)
						};


						EmprestimoLivro.Add(novoEmprestimo);
					}
				}
			}
			catch (FileNotFoundException ex)
			{
				Console.WriteLine("ERRO!!!!: " + ex.Message);
				Console.ReadKey();
			}

			return EmprestimoLivro;
		}

		// RETORNA EMPRESTIMO NO FORMATO PARA ARQUIVO
		private static string FormatoParaArquivo(EmprestimoLivro e)
		{
			return e.IdCliente.ToString().PadRight(5, ' ') + ";" + e.NumeroTombo.ToString().PadRight(5, ' ') + ";" +
			e.DataEmprestimo.ToString("dd/MM/yyyy") + ";" + e.DataDevolucao.ToString("dd/MM/yyyy") + ";" + e.StatusEmprestimo;
		}

		// ESCRITA DE ARQUIVO
		public static void EscreverArquivo(List<EmprestimoLivro> listaEmprestimo)
		{
			using (StreamWriter file = new StreamWriter(@"C:\Arquivos\EMPRESTIMO.CSV"))
			{
				foreach (EmprestimoLivro e in listaEmprestimo)
					file.WriteLine(FormatoParaArquivo(e)); // ESCREVE A LISTA NO ARQUIVO SEPARADOS POR QUEBRA LINHA  
			}
		}

		//Devolução
		public static void Devolucao(List<EmprestimoLivro> listaEmprestimo, List<StatusEmprestimo> listaStatus, List<Cliente> listaClient)
		{


			long numeroTombo;
			int encontradoNumeroTombo = 0;
			string cpf = "";
			long id = 0;
			DateTime datual = DateTime.Now;
			DateTime ddevolucao = DateTime.Now;
			TimeSpan result;
			double multa;

			/*foreach (var elemento in listaEmprestimo)
			{
				Console.WriteLine(elemento.ToString());
			}*/

			numeroTombo = LeiaNumeroTombo();



			foreach (var elemento in listaEmprestimo)
			{

				if (elemento.StatusEmprestimo == 2)
				{
					break;
				}	
				else if (elemento.NumeroTombo == numeroTombo)
				{
					encontradoNumeroTombo++;
					ddevolucao = elemento.DataDevolucao;
				}
			}
			if (encontradoNumeroTombo == 0)
				Console.WriteLine("Livro não encontrado para devolução");
			else
			{

				result = datual - ddevolucao;


				if (result.Days > 0)
				{
					multa = (result.Days * 0.10);

					Console.WriteLine("O valor da Multa é  R$:" + multa.ToString("N2"));
					for (int i = 0; i < listaEmprestimo.Count; i++)
					{
						if (listaEmprestimo[i].NumeroTombo == numeroTombo)
						{
							EmprestimoLivro el = new EmprestimoLivro();
							el.IdCliente = listaEmprestimo[i].IdCliente;
							el.NumeroTombo = listaEmprestimo[i].NumeroTombo;
							el.DataEmprestimo = listaEmprestimo[i].DataEmprestimo;
							el.DataDevolucao = listaEmprestimo[i].DataDevolucao;
							el.StatusEmprestimo = 2;
							listaEmprestimo[i] = el;
							EscreverArquivo(listaEmprestimo);
							id = el.IdCliente;


						}
					}
					for (int i = 0; i < listaClient.Count; i++)
					{
						if (listaClient[i].IdCliente == id)
							cpf = listaClient[i].Cpf;
					}

					for (int i = 0; i < listaStatus.Count; i++)
					{
						if (listaStatus[i].Cpf == cpf)
						{
							StatusEmprestimo se = new StatusEmprestimo();
							se.Cpf = listaStatus[i].Cpf;
							se.Titulo = listaStatus[i].Titulo;
							se.DataEmprestimo = listaStatus[i].DataEmprestimo;
							se.DataDevolucao = listaStatus[i].DataDevolucao;
							se.Status = 2;
							listaStatus[i] = se;

						}
					}
				}	
				else
				{
					Console.WriteLine("Não há multa a se pagar");
					for (int i = 0; i < listaEmprestimo.Count; i++)
					{
						if (listaEmprestimo[i].NumeroTombo == numeroTombo)
						{
							EmprestimoLivro el = new EmprestimoLivro();
							el.IdCliente = listaEmprestimo[i].IdCliente;
							el.NumeroTombo = listaEmprestimo[i].NumeroTombo;
							el.DataEmprestimo = listaEmprestimo[i].DataEmprestimo;
							el.DataDevolucao = listaEmprestimo[i].DataDevolucao;
							el.StatusEmprestimo = 2;
							listaEmprestimo[i] = el;
							EscreverArquivo(listaEmprestimo);
							id = el.IdCliente;


						}
					}
					for (int i = 0; i < listaClient.Count; i++)
					{
						if (listaClient[i].IdCliente == id)
							cpf = listaClient[i].Cpf;
					}

					for (int i = 0; i < listaStatus.Count; i++)
					{
						if (listaStatus[i].Cpf == cpf)
						{
							StatusEmprestimo se = new StatusEmprestimo();
							se.Cpf = listaStatus[i].Cpf;
							se.Titulo = listaStatus[i].Titulo;
							se.DataEmprestimo = listaStatus[i].DataEmprestimo;
							se.DataDevolucao = listaStatus[i].DataDevolucao;
							se.Status = 2;
							listaStatus[i] = se;

						}
					}

				}
				Console.WriteLine("Aperte qualquer tecla para retorar ao menu principal");
				Console.ReadKey();
			}

		}

	}

}

