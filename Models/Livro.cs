using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
	public class Livro
	{
		public long NumeroTombo { get; set; }
		public string Isbn { get; set; }
		public string Titulo { get; set; }
		public string Genero { get; set; }
		public DateTime DataPublicacao { get; set; }
		public string Autor { get; set; }


		public override string ToString()
		{
			return $"Numero Tombo: {NumeroTombo}\nISBN: {Isbn}\nTitulo: {Titulo}\nGenero: {Genero}\nData Publicação: {DataPublicacao.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}\nAutor: {Autor}";
		}


	}

}
