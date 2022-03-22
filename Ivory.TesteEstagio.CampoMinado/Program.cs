using System;
using System.Collections.Generic;
using System.Linq;

namespace Ivory.TesteEstagio.CampoMinado
{
    class Program
    {
        static List<int[]> posFechados = new List<int[]>();
        static int[] posIndex = new int[2];

        static List<int[]> posBombas = new List<int[]>();
        static int[] bombaIndex = new int[2];

        static void Main(string[] args)
        {
            var campoMinado = new CampoMinado();
            Console.WriteLine("Início do jogo\n=========");
            Console.WriteLine(campoMinado.Tabuleiro);

            // Realize sua codificação a partir deste ponto, boa sorte!
            //var matrizJogo = CriarTabulerio(campoMinado.Tabuleiro);
            while (campoMinado.JogoStatus == 0)
            {
                var matrizJogo = CriarTabulerio(campoMinado.Tabuleiro);
                TrazerFechados(matrizJogo);
                CalcularProbabilidade(campoMinado, matrizJogo);
                var compare = campoMinado.Tabuleiro;
            }
            if (campoMinado.JogoStatus == 1)
            {
                Console.WriteLine("FIM\n=========");
                Console.WriteLine(campoMinado.Tabuleiro);
            }
        }
        private static void TrazerFechados(char[,] matrizJogo)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (matrizJogo[i, j] == '-')
                    {
                        posIndex = new int[] { i, j };
                        posFechados.Add(posIndex);
                    }
                }
            }
        }
        private static void CalcularProbabilidade(CampoMinado campoMinado, char[,] matrizJogo)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (matrizJogo[i, j] == '0' || matrizJogo[i, j] == '-' || matrizJogo[i, j] == '*')
                    {
                        continue;
                    }
                    else
                    {
                        List<int[]> listaFechadoTemp = new List<int[]>();

                        int espacosFechados = 0;
                        int valorMinas = Convert.ToInt32(matrizJogo[i, j].ToString());
                        int x = j - 1;
                        int y = i - 1;

                        for (int rep = 0; rep <= 8; rep++)
                        {
                            if (x == j + 2)
                            {
                                x = j - 1;
                                y++;
                            }
                            try
                            {
                                if (matrizJogo[y, x] == '-')
                                {
                                    listaFechadoTemp.Add(posIndex = new int[] { y, x });
                                    espacosFechados++;
                                }
                                else if (matrizJogo[y, x] == '*')
                                {
                                    valorMinas -= 1;
                                }
                            }
                            catch (Exception) { }
                            x++;
                        }

                        if (espacosFechados > valorMinas && valorMinas == 0)
                        {
                            foreach (var abrir in listaFechadoTemp)
                            {
                                campoMinado.Abrir(abrir[0] + 1 , abrir[1] + 1);
                            }
                        }

                        if (espacosFechados == 0 || valorMinas == 0)
                        {
                            continue;
                        }
                        if (valorMinas / espacosFechados == 1f)
                        {
                            foreach (var bomba in listaFechadoTemp)
                            {
                                foreach (var fechado in posFechados.ToArray())
                                {
                                    if (Enumerable.SequenceEqual(fechado, bomba))
                                    {
                                        matrizJogo[bomba[0], bomba[1]] = '*';
                                        posBombas.Add(new int[] { bomba[0], bomba[1] });
                                        posFechados.Remove(fechado);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private static char[,] CriarTabulerio(string tabuleiro)
        {
            int index = 0;
            char[,] tabela = new char[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    tabela[i, j] = tabuleiro[index];
                    index++;
                }
                index += 2;
            }
            if (posBombas.Count > 0)
            {
                foreach (var bomba in posBombas)
                {
                    tabela[bomba[0], bomba[1]] = '*';
                }
            }
            return tabela;
        }
    }
}