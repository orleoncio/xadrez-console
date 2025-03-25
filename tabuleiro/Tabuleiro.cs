using System;

namespace tabuleiro
{
    internal class Tabuleiro
    {
        public int linhas {  get; set; }
        public int colunas {  get; set; }
        public Peca[,] pecas;

        public Tabuleiro(int linhas, int colunas)
        {
            this.linhas = linhas;
            this.colunas = colunas;
            this.pecas = new Peca[linhas, colunas];
        }

        public Peca peca(int linha, int coluna)
        {
            return pecas[linha, coluna];
        }

        public bool existePeca(Posicao pos)
        {
            validarPosicao(pos);
            return peca(pos.linha, pos.coluna) != null;
        }

        public void colocarPeca(Peca peca, Posicao pos)
        {
            if (existePeca(pos))
            {
                throw new TabuleiroException("Já existe uma peça nessa posição");
            }
            pecas[pos.linha, pos.coluna] = peca;
            peca.posicao = pos;
        }

        public Peca retirarPeca(Posicao pos)
        {
            if (!existePeca(pos))
            {
                return null;
            }
            Peca aux = peca(pos.linha, pos.coluna);
            aux.posicao = null;
            pecas[pos.linha, pos.coluna] = null;
            return aux;
        }

        public bool posicaoValida(Posicao pos)
        {
            if(pos.linha < 0 || pos.linha >= linhas || pos.coluna < 0 || pos.coluna >= colunas)
            {
                return false;
            }
            return true;
        }

        public void validarPosicao(Posicao pos)
        {
            if (!posicaoValida(pos))
            {
                throw new TabuleiroException("Posição invalida!");
            }
        }
    }
}
