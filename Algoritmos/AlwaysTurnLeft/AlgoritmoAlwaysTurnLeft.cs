using System;
using System.IO;

namespace Algoritmos.AlwaysTurnLeft
{
    public class AlgoritmoAlwaysTurnLeft
    {
        private const string CaminhoEntradaSmall = @".\AlwaysTurnLeft\B-small-practice.in";
        private const string CaminhoSaidaSmall = @".\AlwaysTurnLeft\B-small-practice.out";
        private const string CaminhoEntradaLarge = @".\AlwaysTurnLeft\B-large-practice.in";
        private const string CaminhoSaidaLarge = @".\AlwaysTurnLeft\B-large-practice.out";

        public void Executar()
        {
            Executar(CaminhoEntradaSmall, CaminhoSaidaSmall);
            Executar(CaminhoEntradaLarge, CaminhoSaidaLarge);
        }

        private void Executar(string caminhoEntrada, string caminhoSaida)
        {
            using (StreamReader entrada = new StreamReader(caminhoEntrada))
            {
                File.Delete(caminhoSaida);

                int contagem = 1;
                entrada.ReadLine();
                while (!entrada.EndOfStream)
                {
                    string temp = entrada.ReadLine();
                    string caminhoIda = temp.Split(' ')[0];
                    string caminhoVolta = temp.Split(' ')[1];

                    int[] tamanho = DescobrirTamanho(caminhoIda, caminhoVolta);
                    SalaQuadrada[,] salao = Resolver(tamanho, caminhoIda, caminhoVolta);

                    EscreverSaida(salao, contagem, caminhoSaida);
                    contagem++;
                }
            
                entrada.Close();
            }
        }

        private void EscreverSaida(SalaQuadrada[,] salao, int contagem, string caminhoSaida)
        {
            using (StreamWriter saida = new StreamWriter(caminhoSaida, true))
            {
                saida.WriteLine("Case #" + contagem.ToString() + ":");

                for (int i = 0; i < salao.GetLength(1); i++)
                {
                    for (int x = 0; x < salao.GetLength(0); x++)
                    {
                        if (i == 3 && x == 0)
                            Console.Write("");

                        int valorSaida = 0;
                        if (salao[x, i].AcimaAberta.HasValue && salao[x, i].AcimaAberta.Value)
                            valorSaida += 1;
                        if (salao[x, i].AbaixoAberta.HasValue && salao[x, i].AbaixoAberta.Value)
                            valorSaida += 2;
                        if (salao[x, i].EsquerdaAberta.HasValue && salao[x, i].EsquerdaAberta.Value)
                            valorSaida += 4;
                        if (salao[x, i].DireitaAberta.HasValue && salao[x, i].DireitaAberta.Value)
                            valorSaida += 8;

                        char charSaida = valorSaida.ToString("x")[0];

                        saida.Write(charSaida);
                    }
                    saida.WriteLine();
                }
                saida.Close();
            }
        }

        private SalaQuadrada[,] Resolver(int[] tamanho, string caminhoIda, string caminhoVolta)
        {
            Direcao direcaoAtual = Direcao.Baixo;
            SalaQuadrada[,] salao = new SalaQuadrada[tamanho[0], tamanho[1]];
            //O eixoX pode ficar negativo, entao soma-se o limite esquerdo, é o eixo inicial em X
            int eixoX = tamanho[2];
            if (eixoX < 0)
                eixoX *= -1;
            int eixoY = 0;

            //Não conta o primeiro e ultimo comando, pois é do lado de fora do labirinto
            for (int i = 1; i < caminhoIda.Length; i++)
            {
                if (eixoX == 1 && eixoY == 3)
                    Console.Write("");

                //Inicializacao
                if (salao[eixoX, eixoY] == null)
                    salao[eixoX, eixoY] = new SalaQuadrada();

                //Primeiro muda a direcao pra saber qual a direcao que ele esta virando
                direcaoAtual = MudaDirecao(direcaoAtual, caminhoIda[i]);

                if (caminhoIda[i] == 'L')
                {
                    if (direcaoAtual == Direcao.Esquerda)
                        salao[eixoX, eixoY].EsquerdaAberta = true;
                    else if (direcaoAtual == Direcao.Direita)
                        salao[eixoX, eixoY].DireitaAberta = true;
                    else if (direcaoAtual == Direcao.Cima)
                        salao[eixoX, eixoY].AcimaAberta = true;
                    else if (direcaoAtual == Direcao.Baixo)
                        salao[eixoX, eixoY].AbaixoAberta = true;
                }
                else if (caminhoIda[i] == 'R')
                {
                    //Verifica se ele anda depois do R ou faz outro R, se fizer outro R ignora
                    if (caminhoIda[i + 1] == 'W')
                    {
                        if (direcaoAtual == Direcao.Esquerda)
                        {
                            salao[eixoX, eixoY].EsquerdaAberta = true;
                            salao[eixoX, eixoY].AbaixoAberta = false;
                            salao[eixoX, eixoY].DireitaAberta = false;
                        }
                        else if (direcaoAtual == Direcao.Direita)
                        {
                            salao[eixoX, eixoY].DireitaAberta = true;
                            salao[eixoX, eixoY].AcimaAberta = false;
                            salao[eixoX, eixoY].EsquerdaAberta = false;
                        }
                        else if (direcaoAtual == Direcao.Cima)
                        {
                            salao[eixoX, eixoY].AcimaAberta = true;
                            salao[eixoX, eixoY].EsquerdaAberta = false;
                            salao[eixoX, eixoY].AbaixoAberta = false;
                        }
                        else if (direcaoAtual == Direcao.Baixo)
                        {
                            salao[eixoX, eixoY].AbaixoAberta = true;
                            salao[eixoX, eixoY].AcimaAberta = false;
                            salao[eixoX, eixoY].DireitaAberta = false;
                        }
                    }
                }

                if (caminhoIda[i] == 'W')
                {
                    if (direcaoAtual == Direcao.Esquerda)
                    {
                        salao[eixoX, eixoY].EsquerdaAberta = true;
                        //Salao[eixoX, eixoY].AbaixoAberta = false;
                        eixoX--;
                    }
                    else if (direcaoAtual == Direcao.Direita)
                    {
                        salao[eixoX, eixoY].DireitaAberta = true;
                        //Salao[eixoX, eixoY].AcimaAberta = false;
                        eixoX++;
                    }
                    else if (direcaoAtual == Direcao.Cima)
                    {
                        salao[eixoX, eixoY].AcimaAberta = true;
                        //Salao[eixoX, eixoY].EsquerdaAberta = false;
                        eixoY--;
                    }
                    else if (direcaoAtual == Direcao.Baixo)
                    {
                        salao[eixoX, eixoY].AbaixoAberta = true;
                        //Salao[eixoX, eixoY].DireitaAberta = false;
                        eixoY++;
                    }
                }
            }

            //Caminho de volta
            //Inverte a direcao
            direcaoAtual = MudaDirecao(direcaoAtual, 'R');
            direcaoAtual = MudaDirecao(direcaoAtual, 'R');

            //Volta pra dentro do labirinto
            if (direcaoAtual == Direcao.Esquerda)
            {
                eixoX--;
            }
            else if (direcaoAtual == Direcao.Direita)
            {
                eixoX++;
            }
            else if (direcaoAtual == Direcao.Cima)
            {
                eixoY--;
            }
            else if (direcaoAtual == Direcao.Baixo)
            {
                eixoY++;
            }

            //Não conta o primeiro e ultimo comando, pois é do lado de fora do labirinto
            for (int i = 1; i < caminhoVolta.Length; i++)
            {
                if (eixoX == 1 && eixoY == 3)
                    Console.Write("");

                if (eixoY > -1 && eixoX > -1)
                {

                    //Inicializacao
                    if (salao[eixoX, eixoY] == null)
                        salao[eixoX, eixoY] = new SalaQuadrada();

                    //Primeiro muda a direcao pra saber qual a direcao que ele esta virando
                    direcaoAtual = MudaDirecao(direcaoAtual, caminhoVolta[i]);

                    if (caminhoVolta[i] == 'L')
                    {
                        if (direcaoAtual == Direcao.Esquerda)
                            salao[eixoX, eixoY].EsquerdaAberta = true;
                        else if (direcaoAtual == Direcao.Direita)
                            salao[eixoX, eixoY].DireitaAberta = true;
                        else if (direcaoAtual == Direcao.Cima)
                            salao[eixoX, eixoY].AcimaAberta = true;
                        else if (direcaoAtual == Direcao.Baixo)
                            salao[eixoX, eixoY].AbaixoAberta = true;
                    }
                    else if (caminhoVolta[i] == 'R')
                    {
                        //Verifica se ele anda depois do R ou faz outro R, se fizer outro R ignora
                        if (caminhoVolta[i + 1] == 'W')
                        {
                            if (direcaoAtual == Direcao.Esquerda)
                            {
                                salao[eixoX, eixoY].EsquerdaAberta = true;
                                salao[eixoX, eixoY].AbaixoAberta = false;
                                salao[eixoX, eixoY].DireitaAberta = false;
                            }
                            else if (direcaoAtual == Direcao.Direita)
                            {
                                salao[eixoX, eixoY].DireitaAberta = true;
                                salao[eixoX, eixoY].AcimaAberta = false;
                                salao[eixoX, eixoY].EsquerdaAberta = false;
                            }
                            else if (direcaoAtual == Direcao.Cima)
                            {
                                salao[eixoX, eixoY].AcimaAberta = true;
                                salao[eixoX, eixoY].EsquerdaAberta = false;
                                salao[eixoX, eixoY].AbaixoAberta = false;
                            }
                            else if (direcaoAtual == Direcao.Baixo)
                            {
                                salao[eixoX, eixoY].AbaixoAberta = true;
                                salao[eixoX, eixoY].AcimaAberta = false;
                                salao[eixoX, eixoY].DireitaAberta = false;
                            }
                        }
                    }

                    if (caminhoVolta[i] == 'W')
                    {
                        if (direcaoAtual == Direcao.Esquerda)
                        {
                            salao[eixoX, eixoY].EsquerdaAberta = true;
                            //Salao[eixoX, eixoY].AbaixoAberta = false;
                            eixoX--;
                        }
                        else if (direcaoAtual == Direcao.Direita)
                        {
                            salao[eixoX, eixoY].DireitaAberta = true;
                            //Salao[eixoX, eixoY].AcimaAberta = false;
                            eixoX++;
                        }
                        else if (direcaoAtual == Direcao.Cima)
                        {
                            salao[eixoX, eixoY].AcimaAberta = true;
                            //Salao[eixoX, eixoY].EsquerdaAberta = false;
                            eixoY--;
                        }
                        else if (direcaoAtual == Direcao.Baixo)
                        {
                            salao[eixoX, eixoY].AbaixoAberta = true;
                            //Salao[eixoX, eixoY].DireitaAberta = false;
                            eixoY++;
                        }
                    }
                }
                else
                {
                    if (eixoY == -1)
                        eixoY = 0;
                    else
                        eixoX = 0;
                }
            }

            return salao;
        }

        private int[] DescobrirTamanho(string caminhoIda, string caminhoVolta)
        {
            Direcao DirecaoAtual = Direcao.Baixo;

            int limiteEsq = 0;
            int limiteDir = 0;
            int eixoX = 0;

            int limiteCima = 0;
            int limiteBaixo = 0;
            int eixoY = 0;

            //Não conta o primeiro e ultimo comando, pois é do lado de fora do labirinto
            for (int i = 1; i < caminhoIda.Length - 1; i++)
            {
                DirecaoAtual = MudaDirecao(DirecaoAtual, caminhoIda[i]);

                //Descobre a largura do labirinto
                if (caminhoIda[i] == 'W')
                {
                    if (DirecaoAtual == Direcao.Esquerda)
                        eixoX--;
                    else if (DirecaoAtual == Direcao.Direita)
                        eixoX++;
                    else if (DirecaoAtual == Direcao.Cima)
                        eixoY--;
                    else if (DirecaoAtual == Direcao.Baixo)
                        eixoY++;
                }

                //Atualiza os limites
                if (eixoX < limiteEsq)
                    limiteEsq = eixoX;

                if (eixoX > limiteDir)
                    limiteDir = eixoX;

                if (eixoY < limiteCima)
                    limiteCima = eixoY;

                if (eixoY > limiteBaixo)
                    limiteBaixo = eixoY;

            }

            //CaminhoVolta
            //Inverte a direcao
            DirecaoAtual = MudaDirecao(DirecaoAtual, 'R');
            DirecaoAtual = MudaDirecao(DirecaoAtual, 'R');

            //Não conta o primeiro e ultimo comando, pois é do lado de fora do labirinto
            for (int i = 1; i < caminhoVolta.Length - 1; i++)
            {
                DirecaoAtual = MudaDirecao(DirecaoAtual, caminhoVolta[i]);

                //Descobre a largura do labirinto
                if (caminhoVolta[i] == 'W')
                {
                    if (DirecaoAtual == Direcao.Esquerda)
                        eixoX--;
                    else if (DirecaoAtual == Direcao.Direita)
                        eixoX++;
                    else if (DirecaoAtual == Direcao.Cima)
                        eixoY--;
                    else if (DirecaoAtual == Direcao.Baixo)
                        eixoY++;
                }

                //Atualiza os limites
                if (eixoX < limiteEsq)
                    limiteEsq = eixoX;

                if (eixoX > limiteDir)
                    limiteDir = eixoX;

                if (eixoY < limiteCima)
                    limiteCima = eixoY;

                if (eixoY > limiteBaixo)
                    limiteBaixo = eixoY;

            }

            //+1 pois nao conta o eixo central
            int TamanhoX = limiteDir - limiteEsq + 1;
            int TamanhoY = limiteBaixo - limiteCima + 1;

            return new int[3] { TamanhoX, TamanhoY, limiteEsq };
        }

        private Direcao MudaDirecao(Direcao atual, char comando)
        {
            Direcao NovaDirecao = atual;

            if (comando == 'R')
                NovaDirecao = (Direcao)(((int)atual + 1) % 4);

            else if (comando == 'L')
                NovaDirecao = (Direcao)((int)atual - 1);


            //Quando atual inicia com Cima e vira pra esquerda, o resultado será -1
            if (NovaDirecao < 0)
                NovaDirecao = Direcao.Esquerda;

            return NovaDirecao;
        }
    }
}