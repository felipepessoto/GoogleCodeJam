using System;
using System.IO;

namespace Algoritmos.AlienNumbers
{
    public class AlgoritmoAlienNumbers
    {
        private const string CaminhoEntradaSmall = @".\AlienNumbers\A-small-practice.in";
        private const string CaminhoSaidaSmall = @".\AlienNumbers\A-small-practice.out";
        private const string CaminhoEntradaLarge = @".\AlienNumbers\A-large-practice.in";
        private const string CaminhoSaidaLarge = @".\AlienNumbers\A-large-practice.out";

        public void Executar()
        {
            ExecutarArquivo(CaminhoEntradaSmall, CaminhoSaidaSmall);
            ExecutarArquivo(CaminhoEntradaLarge, CaminhoSaidaLarge);
        }

        private static void ExecutarArquivo(string caminhoEntrada, string caminhoSaida)
        {
            int x = 1;
            using (StreamReader entrada = new StreamReader(caminhoEntrada))
            using (StreamWriter saida = new StreamWriter(caminhoSaida))
            {
                foreach (string linha in entrada.ReadToEnd().Split(new[] {"\n"}, StringSplitOptions.RemoveEmptyEntries))
                {
                    string[] partes = linha.Split(' ');
                    if (partes.Length == 3)
                    {
                        saida.WriteLine("Case #" + x.ToString() + ": " + AlienToAlien(partes[0], partes[1], partes[2]));
                        x++;
                    }
                }
            }
        }

        public static string AlienToAlien(string numero, string fonte, string destino)
        {
            return DecimalToAlien(AlienToDecimal(numero, fonte).ToString(), destino);
        }

        private static string DecimalToAlien(string decimalNumber, string targetLanguage)
        {
            string resultado = "";
            int total = int.Parse(decimalNumber);
            int totaldigitos = CalcularQtDigitos(total, targetLanguage);

            while (total > 0 || resultado.Length < totaldigitos)
            {
                int numero = total;
                int qtdivisoes = 0;

                while (numero >= targetLanguage.Length)
                {
                    numero = numero / targetLanguage.Length;
                    qtdivisoes++;
                }
                int retirardototal = (numero) * ((int)Math.Pow(targetLanguage.Length, qtdivisoes));

                //Verifica se ele ta na casa certa, pois o numero que era pra ficar 10011 ta ficando 11100, pois ele coloca os zeros no meio dos numeros

                string zeros;
                do
                {
                    zeros = "";
                    for (int i = resultado.Length + 1; i < totaldigitos; i++)
                    {
                        zeros += targetLanguage[0];
                    }
                    if (AlienToDecimal(targetLanguage[numero] + zeros, targetLanguage) > total)
                    {
                        resultado += targetLanguage[0];
                    }
                } while (AlienToDecimal(targetLanguage[numero] + zeros, targetLanguage) > total);

                total -= retirardototal;
                resultado += targetLanguage[numero];
            }

            return resultado;
        }

        private static int CalcularQtDigitos(int decimalNumber, string targetLanguage)
        {
            int qtdivisoes = 0;

            while (decimalNumber >= 1)
            {
                decimalNumber = decimalNumber / targetLanguage.Length;
                qtdivisoes++;
            }

            return qtdivisoes;
        }

        private static int AlienToDecimal(string alienNumber, string sourceLanguage)
        {
            int total = 0;

            for (int i = 0; i < alienNumber.Length; i++)
            {
                int numero = sourceLanguage.IndexOf(alienNumber[i]);
                int resultado = numero * (int)(Math.Pow(sourceLanguage.Length, alienNumber.Length - i - 1));
                total += resultado;
            }

            return total;
        }
    }
}
