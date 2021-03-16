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
	public class LivroControle
	{
		//Registra o Livro
		public static void Registrar(List<Livro> lista)
		{
			// VARIAVEIS
			long NumeroTombo = 0;
			string isbn, titulo, genero, autor;
			DateTime dpublicacao;

			//INSERE O NUMERO TOMBO DE MANEIRA SEQUENCIAL E AUTOMATICA
			if (lista.Count == 0)
				NumeroTombo = 1;
	
			
			else
				NumeroTombo = lista.Count + 1;		
			isbn = LeiaIsbn(lista); // VERIFICA Isbn VALIDO 

			//VERIFICA SE ISBN JÁ EXISTE NA LISTA
			if (IsbnRepetido(lista, isbn))
			{
				Console.WriteLine("Isbn já cadastrado");

			}
			else
			{
				titulo = LeiaTitulo(); // LE O TITULO DO LIVRO

				genero = LeiaGenero(); //LE O GENERO

				dpublicacao = LeiaPublicacao(); // LE A DATA DE PUBLICAÇÃO

				autor = LeiaAutor(); // LE NOME AUTOR

				Livro novoLivro = new Livro()
				{
					NumeroTombo = NumeroTombo,
					Isbn = isbn,
					Titulo = titulo,
					Genero = genero,
					DataPublicacao = dpublicacao,
					Autor = autor
				};

				lista.Add(novoLivro);
				lista = lista.OrderBy(x => x.NumeroTombo).ToList();
				EscreverArquivo(lista);
				Console.Clear();
				Console.WriteLine("Livro cadastrado com sucesso");
				Console.WriteLine("Seu numero tombo é: " + NumeroTombo);
				Console.WriteLine("Não esqueça de inserir na TAG do livro o numero tombo");
				Console.WriteLine("Aperte qualquer tecla para voltar ao menu principal");
				Console.ReadKey();
				Console.Clear();
			}

		}

		public static string LeiaIsbn(List<Livro> lista)
		{
			string isbn;

			do
			{ // LAÇO VERIFICA ISBN SE É VALIDO 
				Console.Write("Informe o Isbn do Livro para validação (Apenas números e com o dígito): ");
				isbn = Console.ReadLine();

				if (!IsbnEhValido(isbn))
					Console.WriteLine("ISBN inválido");


			} while (!(IsbnEhValido(isbn)));


			return isbn;
		}

		// VERIFICA ISBN REPETE
		public static bool IsbnRepetido(List<Livro> lista, string isbn)
		{
			foreach (Livro i in lista)
			{
				if (i.Isbn.Equals(isbn))
					return true;
			}
			return false;
		}

		//MATEMATICA PARA VALIDAÇÃO DE ISBN
		public static bool IsbnEhValido(string value)
		{
			int multiplicador1 = 1;
			int multiplicador2 = 3;
			string tempIsbn;
			int soma;
			int resto;

			value = value.Trim();
			value = value.Replace("-", "");

			if (value.Length != 13)
				return false;

			tempIsbn = value.Substring(0, 12);
			soma = 0;

			for (int i = 0; i < tempIsbn.Length ; i = i + 2)
			{
				
					soma += int.Parse(tempIsbn[i].ToString()) * multiplicador1;
			}
			for (int i = 1; i < tempIsbn.Length ; i = i + 2)
			{
					soma += int.Parse(tempIsbn[i].ToString()) * multiplicador2;
			}

			resto = soma % 10;
			if (resto == 0)
				soma = soma + 0;
			else
				soma = soma + (10 - resto);



			if (soma % 10 == 0)
				return true;


			return false;
		}

		// LE TITULO
		private static string LeiaTitulo()
		{
			string titulo;

			do
			{ // LAÇO PARA NÃO DEIXAR CAMPO VAZIO E COM O TAMANHO MAXIMO PARA O ARQUIVO
				Console.Write("Informe o Titulo do Livro: ");
				titulo = Console.ReadLine();
				if (titulo.Length > 50)
					Console.WriteLine("Maximo 50 caracteres no Titulo do Livro");

			} while (titulo == "" || titulo.Length > 50);
			return titulo;
		}

		// LE GENERO
		private static string LeiaGenero()
		{
			string genero;

			do
			{ // LAÇO PARA NÃO DEIXAR CAMPO VAZIO E COM O TAMANHO MAXIMO PARA O ARQUIVO
				Console.Write("Informe o Genero do Livro: ");
				genero = Console.ReadLine();
				if (genero.Length > 15)
					Console.WriteLine("Maximo 15 caracteres para Genero do Livro");

			} while (genero == "" || genero.Length > 15);
			return genero;
		}

		// LE DATA DE PUBLICAÇÃO
		private static DateTime LeiaPublicacao()
		{
			DateTime dPublicacao;
			CultureInfo CultureBr = new CultureInfo(name: "pt-BR"); // DATA NO FORMATO BRASILEIRO
			Console.Write("Informe a data de publicação do Livro: ");
			dPublicacao = DateTime.ParseExact(Console.ReadLine(), "d", CultureBr); // CONVERTE FORMATO DATA INGLESA PRA BRASILEIRA;

			return dPublicacao;
		}

		// LE AUTOR
		private static string LeiaAutor()
		{
			string autor;

			do
			{ // LAÇO PARA NÃO DEIXAR CAMPO VAZIO E COM O TAMANHO MAXIMO PARA O ARQUIVO
				Console.Write("Informe o Autor do Livro: ");
				autor = Console.ReadLine();
				if (autor.Length > 50)
					Console.WriteLine("Maximo 50 caracteres para campo Autor do Livro");

			} while (autor == "" || autor.Length > 50);
			return autor;
		}


		// RETORNA LIVRO NO FORMATO PARA ARQUIVO
		private static string FormatoParaArquivo(Livro l)
		{
			return l.NumeroTombo.ToString().PadRight(5, ' ') + ";" + l.Isbn + ";" + l.Titulo.PadRight(50, ' ') + ";" + l.Genero.PadRight(15, ' ') + ";" +
			l.DataPublicacao.ToString("dd/MM/yyyy") + ";" + l.Autor.PadRight(50, ' ') +".";
		}

		// ESCRITA DE ARQUIVO
		public static void EscreverArquivo(List<Livro> listaLivro)
		{
			using (StreamWriter file = new StreamWriter(@"C:\Arquivos\LIVRO.CSV"))
			{
				foreach (Livro l in listaLivro)
					file.WriteLine(FormatoParaArquivo(l)); // ESCREVE A LISTA NO ARQUIVO SEPARADOS POR QUEBRA LINHA  
			}
		}

		//LE O ARQUIVO E TRANSFORMA EM LISTA
		public static List<Livro> ConverterParaLista()
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




	}
}
