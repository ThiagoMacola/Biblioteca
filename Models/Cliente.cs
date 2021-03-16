using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
	public class Cliente
	{

		public long IdCliente { get; set; }
		public string Cpf { get; set; }
		public string Nome { get; set; }
		public DateTime DataNascimento { get; set; }
		public string Telefone { get; set; }
		public Endereco Endereco { get; set;}

		public override string ToString()
		{
			return $"ID CLIENTE: {IdCliente}\nCPF: {Cpf}\nNome: {Nome}\nData Nascimento:{DataNascimento.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}\nTelefone: {Telefone}\nLogradouro: {Endereco.Logradouro}\nBairro: {Endereco.Bairro}\nCidade: {Endereco.Cidade}\nEstado: {Endereco.Estado }\nCEP: {Endereco.Cep}";
		}


	}

}

