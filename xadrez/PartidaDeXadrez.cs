using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tabuleiro;
using xadrez_console;

namespace xadrez
{
    internal class PartidaDeXadrez
    {
        public Tabuleiro tab { get; protected set; }
        public int turno { get; protected set; }
        public Cor jogadorAtual { get; protected set; }
        public bool terminada {  get; private set; }
        private HashSet<Peca> pecas;
        private HashSet<Peca> capturadas;

        public PartidaDeXadrez()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.branco;
            terminada = false;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            colocarPecas();
        }

        public void executaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.retirarPeca(origem);
            p.IncrementarQteMovimentos();
            Peca pecaCapturada = tab.retirarPeca(destino);
            tab.colocarPeca(p, destino);
            Tela.imprimirTabuleiro(tab);
            if(pecaCapturada != null)
            {
                capturadas.Add(pecaCapturada);
            }
        }

        public void realizaJogada(Posicao origem, Posicao destino)
        {
            executaMovimento(origem, destino);
            turno++;
            mudaJogador();
        }

        public void validarPosicaoDeOrigem(Posicao pos)
        {
            if(tab.peca(pos.linha, pos.coluna) == null)
            {
                throw new TabuleiroException("Não existe peça na posição de origem escolhida");
            }
            if(jogadorAtual != tab.peca(pos.linha, pos.coluna).cor)
            {
                throw new TabuleiroException("A peça de origem escolhida não é sua");
            }
            if(!tab.peca(pos.linha, pos.coluna).existeMovimentosPossiveis())
            {
                throw new TabuleiroException("Não há movimentos possiveis para a peça de origem escolhida");
            }
        }

        public void validarPosicaoDeDestino(Posicao origem, Posicao destino)
        {
            if (!tab.peca(origem.linha, origem.coluna).podeMoverPara(destino))
            {
                throw new TabuleiroException("Posição de destino invalida");
            }
        }

        private void mudaJogador()
        {
            if(jogadorAtual == Cor.branco)
            {
                jogadorAtual = Cor.preto;
            }
            else
            {
                jogadorAtual= Cor.branco;
            }
        }

        public HashSet<Peca> pecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in capturadas)
            {
                if(x.cor == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<Peca> pecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in pecas)
            {
                if (x.cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(pecasCapturadas(cor));
            return aux;
        }

        public void colocarNovaPeca(char coluna, int linha, Peca peca)
        {
            tab.colocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            pecas.Add(peca);
        }

        private void colocarPecas()
        {
            colocarNovaPeca('a', 8, new Torre(tab, Cor.preto));
            colocarNovaPeca('h', 8, new Torre(tab, Cor.preto));
            colocarNovaPeca('a', 1, new Torre(tab, Cor.branco));
            colocarNovaPeca('h', 1, new Torre(tab, Cor.branco));
        }
    }
}
