using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Controls;

namespace Projeto_Biblioteca
{
	class Program
	{
		static void Main(string[] args)
		{
			List<Cliente> listaCliente = new List<Cliente>();
			List<Livro> listaLivro = new List<Livro>();
			List<EmprestimoLivro> listaEmprestimo = new List<EmprestimoLivro>();
			List<StatusEmprestimo> listaStatus = new List<StatusEmprestimo>();


			string opcao;

			do
			{ // MENU BIBLIOTECA
				Console.WriteLine("\n------->>> BIBLIOTECA <<<-------");
				Console.WriteLine("\n1 - Inserir Cliente" +
									"\n2 - Inserir Livro" +
									"\n3 - Emprestimos" +
									"\n4 - Devolvuções" +
									"\n5 - Imprimir Emprestimo/Devolução" +
									"\n0 - Sair" +
									"\n\n--------------------------");
				opcao = Console.ReadLine();

				Console.Clear();

				switch (opcao)
				{
					case "1":
						listaCliente = ClienteControle.ConverteParaLista();
						ClienteControle.Registrar(listaCliente); // CRIA CLIENTE E ADICIONA NA FILA                        
					break;
						
					case "2":
						listaLivro = LivroControle.ConverterParaLista();
						LivroControle.Registrar(listaLivro); // CRIA LIVRO E ADICIONA NA FILA
					break;
					
					case "3":
						listaEmprestimo = ControladorEmprestimo.ConverteParaListaEmprestimo();
						listaLivro = LivroControle.ConverterParaLista();
						listaCliente = ClienteControle.ConverteParaLista();
						ControladorEmprestimo.Registrar(listaEmprestimo, listaLivro,listaCliente, listaStatus);
						
						break;
					case "4":
						listaEmprestimo = ControladorEmprestimo.ConverteParaListaEmprestimo();
						listaCliente = ControladorEmprestimo.ConverteParaListaCliente();
						ControladorEmprestimo.Devolucao(listaEmprestimo, listaStatus, listaCliente);
						break;
					case "5":
						foreach (var elemento in listaStatus)
						{
							Console.WriteLine(elemento.ToString());
						}
						Console.WriteLine("Aperte qualquer tecla para retornar ao menu Principal");
						Console.ReadKey();
						break;
				
				}
			} while (opcao != "0");

		}
	}

}