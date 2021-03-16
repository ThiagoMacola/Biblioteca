using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controls
{
	class ManipuladorArquivoControle
	{

        public static void InicializarArquivo(ManipuladorArquivo arquivo)
        {
            if (!Directory.Exists(arquivo.Path))//CRIA PASTA
            {
                Directory.CreateDirectory(arquivo.Path);
            }

            if (!File.Exists($@"{arquivo.Path}\{arquivo.Name}"))
            {
                using (File.Create($@"{arquivo.Path}\{arquivo.Name}"))
                {
                    Console.WriteLine($"Arquivo {arquivo.Name} Criado com sucesso!");
                }
            }

        }
        public static void EscreverNoArquivo(ManipuladorArquivo arquivo, string[] conteudo)
        {
            using (StreamWriter streamWriter = File.CreateText($@"{arquivo.Path}\{arquivo.Name}"))
            {
                foreach (var linha in conteudo)
                {
                    streamWriter.Write(linha);
                }
            }
        }
        public static string[] LerArquivo(ManipuladorArquivo arquivo)
        {
            return File.ReadAllLines($@"{arquivo.Path}\{arquivo.Name}");
        }


    }
}
