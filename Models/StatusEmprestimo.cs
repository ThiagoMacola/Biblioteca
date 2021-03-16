using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
	public class StatusEmprestimo
	{

		public string Cpf { get; set; }
		public string Titulo { get; set; }
		public DateTime DataEmprestimo { get; set; }
		public DateTime DataDevolucao { get; set; }
		public int Status { get; set; }

		public override string ToString()
		{
			return $"CPF: {Cpf}\nTITULO: {Titulo}\nData Emprestimo:{DataEmprestimo.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}\nData Devolucao:{DataDevolucao.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}\nStatus: {Status}";
		}




	}



}
