using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;
public class Match : MonoBehaviour
{

    public GameController gc;

    public GameObject bloco;
    public GameObject blocoAux;

    public int idJogador;
    
    [SerializeField]
    public int[,] arrayBoard;// = new int[8,8];

    public GameObject[,] piecesInBoard; // new GameObject[8,8];

    public GameObject[,] CreateBoard(int eixoX, int eixoY)
    {

        int x = eixoX;
        int y = eixoY;

        piecesInBoard = new GameObject[x, y];
        arrayBoard = new int[x, y];// inicia uma matriz bidimensional que define o tamanho da mesma na horizontal (x) e vertical (y)

        int xx;
        int yy;

        for (xx = 0; xx < arrayBoard.GetLength(0); xx++)
        {
            for (yy = 0; yy < arrayBoard.GetLength(1); yy++)
            {
                Vector3 vetor3 = new Vector3(yy-3.5f, -xx+2.5f, 3);

                blocoAux = Instantiate(bloco,vetor3 , Quaternion.identity);// Instanciar o objeto no espaço em relaçao a camera
                blocoAux.name = (xx + yy) + "S " + " X:" + xx + " Y:" + yy;// renomeia o objeto no unity
                AtulizaVetorPeca(blocoAux, xx, yy);// função que adiciona o vetor x e y que a peca ocupa para fazer calculos de matriz com a mesma
                gc.tabuleiroPecasNovas.Add(blocoAux);// adiciona a peca de forma lógica a uma lista de objetos no gamecontroller 

                piecesInBoard[xx, yy] = blocoAux;
            }
        }
        AtulizaStatusPeca(gc.tabuleiroPecasNovas);

        int xxx = 2;
        int yyy = 6;
        for (int i = 0; i <= 5; i++)
        {
            PegaPeca(piecesInBoard, xxx, yyy);
            yyy += 1;
        }
        return piecesInBoard;
    }

    // essa função atuliza os valores dos vetores do objeto peca para poder calcular o 5  em linha
   public void AtulizaVetorPeca(GameObject peca,int x, int y)
   {
       peca.GetComponent<Peca>().x = x;
       peca.GetComponent<Peca>().y = y;
    }
    public void AtulizaStatusPeca(List<GameObject> tabuleiroPecas)
    {
        Board board = new Board();
        List<int> pecasRandom = new List<int>();
        TabuleiroData pecas = new TabuleiroData(board);

        pecasRandom = pecas.ShuffleList<int>(pecas.pecas);

        int indice = 0;
        foreach (GameObject peca in tabuleiroPecas)
        {
            peca.GetComponent<Peca>().valor = pecasRandom[indice];
            peca.GetComponentInChildren<TextMeshProUGUI>().text = pecasRandom[indice].ToString();
            peca.GetComponentInChildren<TextMeshProUGUI>().fontSize = 60;

            indice++;
        }
    }
    /*  Esta lógica pode ser aplicada a qualquer jogo/software que utilize matrizes quadradas, como Candy Rush, etc.

    matriz 8x8

    PROBLEMA

    Após o usuário ecolher um elemento do tabuleiro e o mesmo ser considerado como uma resposta válida, comecará a verificação se aquele elemento forma, com os elementos marcados a sua volta, uma sequências de 5.
    A sequencia pode ser na horizontal, vertical ou diagonal.
    Caso o resultado seja verdadeiro, finaliza o jogo e apresenta o vencedor.
    Caso o resultado seja falso, o jogo continua.
    */

    //DETALHANDO EM ALGORITMO

    /* Sobre o TABULEIRO e suas bordas/limites

    vamos definir mentalmente o tabuleiro como sendo igual a uma tabela ou matriz. Voce visualiza como estiver mais acostumado.

    A partir da definição dos limites de linha e limites de colunas do tabuleiro, serão distribuidos valores x e y para cada posição onde x respesanta a linha (x=linha)
    e y representa a coluna (y = coluna).


    No momento verificar a sequencia de blocos em qualquer direção(horizontal, vertical, diagonal), precisamos, também identificas onde se encontram os últimos blocos que se encontram nos limites do tabuleiro/tabela/matriz. Isso é necessário para que o sistema entenda que, apartir daquele limite não é possível/necessário continuar com a verificação.
    As bordas de um tabuleiro são: borda esquerda, borda de cima(superior), borda direita, borda de baixo(inferior).
    Num tabuleiro podemos verificar as bordas de forma matemática, representando o mesmo por uma matriz bidimensional, como mostra o exemplo a seguir.

    Tabuleiro/Tabela/Matriz (bidimensional)[x,y]

    [0,0]	[0,1]	[0,2]	[0,3]	[0,4]	[0,5]

    [1,0]	[1,1]	[1,2]	[1,3]	[1,4]	[1,5]	

    [2,0]	[2,1]	[2,2]	[2,3]	[2,4]	[2,5]

    [3,0]	[3,1]	[3,2]	[3,3]	[3,4]	[3,5]

    [4,0]	[4,1]	[4,2]	[4,3]	[4,4]	[4,5]

    [5,0]	[5,1]	[5,2]	[5,3]	[5,4]	[5,5]	


    Lógica sobre as bordas 

    A Borda de cima é identificada por todos os x=0;

    A Borda esquerda é identificada por todos os y=0;

    A Borda direita é identificada por todos os y=5;

    A Borda de baixo é identificada por todos os x=5;

    Mostrando cada borda visualmente

    Borda de cima (todo x=0)

    [0,0]	[0,1]	[0,2]	[0,3]	[0,4]	[0,5]	

    Borda esquerda (todo y=0)

    [0,0]	

    [1,0]	

    [2,0]

    [3,0]	

    [4,0]

    [5,0]	

    Borda direira (todo y=5)

                        [0,5]	

                        [1,5]	

                        [2,5]	

                        [3,5]	

                        [4,5]	

                        [5,5]

    Borda de baixo (todo x=5)

    [5,0]	[5,1]	[5,2]	[5,3]	[5,4]	[5,5]	

    fim da explicação das bordas/limites do tabuleiro */

    //execução da lógica em código c#

    /*Após o usuário ecolher um elemento do tabuleiro e o mesmo ser considerado como uma resposta válida, comecará a verificação se aquele elemento forma, com os elementos marcados a sua volta, uma sequências de 5.*/

    public bool VerificaSequencia(int range,int idPlayer,int numMatch, Peca peca, GameObject[,] board){

        bool resultadoFinal;

        bool resultadoHorizontal = VerificaHorizontal(range,peca.status,numMatch, peca, board);
        bool resultadoVertical = VerificaVertical(range, peca.status, numMatch, peca, board);
        bool resultadoDiagonal = VerificaDiagonal(range, peca.status, numMatch, peca, board);

        if (resultadoHorizontal == true || resultadoVertical == true || resultadoDiagonal == true)
        {
            resultadoFinal = true;
            return resultadoFinal;
        }
        else
        {
            resultadoFinal = false;
            return resultadoFinal;
        }
    }

    //A FUNÇAO  a seguir é responsável por pegar ma peça que está localizada numa coordenada de matriz especifica, no caso no tabuleiro/board
    public Peca PegaPeca(GameObject[,] board, int x, int y)
    {
        Peca peca = new Peca();
        if (x >= 0 && x <= board.GetLength(0)-1 && y >= 0 && y <= board.GetLength(1)-1)// delimitando os limites de pesquisa para os limites do tabuleiro/board
        {      
            peca = board[x, y].GetComponent<Peca>();
            return peca;
        }
        else
        {
            return peca = null;
        }
    }

    public Peca PecaAnteriorHorizontal(GameObject[,] board, int x, int y)
    {
        Peca peca = PegaPeca(board, x, y-1);
        if (peca == null) {
            //Debug.Log("PEÇA ANTERIOR: VALOR NULO");
            return peca;        
        }
        else
        {
            //Debug.Log("PEÇA ANTERIOR HORIZONTAL: " + peca.valor);
            return peca;           
        }
    }

    public Peca PecaPosteriorHorizontal(GameObject[,] board, int x, int y)
    {
        Peca peca = PegaPeca(board, x, y +1);
        if (peca == null)
        {
            //Debug.Log("PEÇA POSTERIOR: VALOR NULO");
            return peca;
        }
        else
        {
            //Debug.Log("PEÇA POSTERIOR HORIZONTAL: " + peca.valor);
            return peca;
        }
    }

    public Peca PecaInferiorVertical(GameObject[,] board, int x, int y)
    {
        Peca peca = PegaPeca(board, x+1, y);
        if (peca == null)
        {
            //Debug.Log("PEÇA INFERIOR : VALOR NULO");
            return peca;
        }
        else
        {
            //Debug.Log("PEÇA INFERIOR: " + peca.valor);
            return peca;
        }
    }

    public Peca PecaSuperiorVertical(GameObject[,] board, int x, int y)
    {
        Peca peca = PegaPeca(board, x -1, y);
        if (peca == null)
        {
           // Debug.Log("PEÇA SUPERIOR: VALOR NULO");
            return peca;
        }
        else
        {
           // Debug.Log("PEÇA SUPERIOR: " + peca.valor);
            return peca;
        }
    }

    public Peca PecaSuperiorDireitaDiagonal(GameObject[,] board, int x, int y)
    {
        Peca peca = PegaPeca(board, x - 1, y+1);
        if (peca == null)
        {
           // Debug.Log("DEPOIS: VALOR NULO");
            return peca;
        }
        else
        {
           // Debug.Log("PEÇA ANTERIOR HORIZONTAL: " + peca.valor);
            return peca;
        }
    }

    public Peca PecaSuperiorEsquerdaDiagonal(GameObject[,] board, int x, int y)
    {
        Peca peca = PegaPeca(board, x - 1, y - 1);
        if (peca == null)
        {
           // Debug.Log("DEPOIS: VALOR NULO");
            return peca;
        }
        else
        {
            //Debug.Log("PEÇA ANTERIOR HORIZONTAL: " + peca.valor);
            return peca;
        }
    }

    public Peca PecaInferiorDireitaDiagonal(GameObject[,] board, int x, int y)
    {
        Peca peca = PegaPeca(board, x + 1, y + 1);
        if (peca == null)
        {
           // Debug.Log("DEPOIS: VALOR NULO");
            return peca;
        }
        else
        {
           // Debug.Log("PEÇA ANTERIOR HORIZONTAL: " + peca.valor);
            return peca;
        }
    }

    public Peca PecaInferiorEsquerdaDiagonal(GameObject[,] board, int x, int y)
    {
        Peca peca = PegaPeca(board, x + 1, y -1);
        if (peca == null)
        {
           // Debug.Log("DEPOIS: VALOR NULO");
            return peca;
        }
        else
        {
           // Debug.Log("PEÇA ANTERIOR HORIZONTAL: " + peca.valor);
            return peca;
        }
    }

    //esta função verifica se, a partir de uma peca selecionada, existe uma sequenciade 5 da mesma cor a direira ou esquerda.
    public bool VerificaHorizontal(int range, int status, int numMatch, Peca peca,GameObject[,] board){

        //Debug.Log("PEÇA PEGA: " + peca.valor);
        bool resultado=false;

        int numPecasMarcadas = 1;//ja inicializo o numero de peças marcadas com 1 pois a peca que é base desta verificação já foi verificada como verdadeira

        //testando limite esquerdo. verificar  peca-1, peca-2,peca-range
        for (int i = 0; i <= range-1; i++)
        {
           // Debug.Log("VERIFICA ANTERIOR: " + numPecasMarcadas);
            Peca pecaAnterior = PecaAnteriorHorizontal(board, peca.x,peca.y-i);// no caso da horizontal o valor que tem q ser alterado é o da coluna, no caso o y
            if (pecaAnterior!=null && pecaAnterior.x == peca.x && pecaAnterior.y >= 0 && pecaAnterior.status == status)
            {
                numPecasMarcadas += 1;
                if (numPecasMarcadas >= range)
                {
                    resultado = true;
                   // Debug.Log("ENCONTROU MATCH COM "+numPecasMarcadas+" PECAS ANTERIOR");
                    return resultado;
                }
                else
                {
                  //  Debug.Log("PECAS MARCADAS ANTERIOR: " + numPecasMarcadas);
                    
                }
            }
            else { break; }
        }

        //testando limite direito. verificar  peca+1, peca+2,peca+range
        for (int i = 0; i <= range-1; i++)
        {
            //Debug.Log("VERIFICA POSTERIOR: " + numPecasMarcadas);
            Peca pecaPosterior = PecaPosteriorHorizontal(board,peca.x,peca.y+i);

            if (pecaPosterior != null && pecaPosterior.x == peca.x && pecaPosterior.y >= 0 && pecaPosterior.status == status)
            {
                numPecasMarcadas += 1;
                if (numPecasMarcadas >= range)
                {
                    resultado= true;
                    //Debug.Log("ENCONTROU MATCH COM " + numPecasMarcadas + " PECAS POSTERIOR");
                    return resultado;
                }
                else
                {
                   // Debug.Log("PECAS MARCADAS POSTERIOR: " + numPecasMarcadas);
                }

            }
            else { break; }
        }

        if (numPecasMarcadas >= range)
        {
            resultado = true;
        }
        else
        {
            resultado = false;
        } 
        return resultado;
    }

    public bool VerificaVertical(int range, int status, int numMatch, Peca peca, GameObject[,] board)
        {
       // Debug.Log("PEÇA PEGA: " + peca.valor);
        bool resultado = false;

        int numPecasMarcadas = 1;//ja inicializo o numero de peças marcadas com 1 pois a peca que é base desta verificação já foi verificada como verdadeira

        //testando limite SUPERIOR. verificar  peca-1, peca-2,peca-range
        for (int i = 0; i <= range - 1; i++)
        {
           // Debug.Log("VERIFICA SUPERIOR: " + numPecasMarcadas);
            Peca pecaAnterior = PecaSuperiorVertical(board, peca.x-i, peca.y);// no caso da horizontal o valor que tem q ser alterado é o da coluna, no caso o y
            if (pecaAnterior != null && pecaAnterior.y == peca.y && pecaAnterior.x >= 0 && pecaAnterior.status == status)
            {
                numPecasMarcadas += 1;
                if (numPecasMarcadas >= range)
                {
                    resultado = true;
                    //Debug.Log("ENCONTROU MATCH COM " + numPecasMarcadas + " PECAS ACIMA");
                    return resultado;
                }
                else
                {
                    //Debug.Log("PECAS MARCADAS ACIMA: " + numPecasMarcadas);
                }
            }
            else { break; }
        }

        //testando limite INFERIOR. verificar  peca+1, peca+2,peca+range
        for (int i = 0; i <= range - 1; i++)
        {
            //Debug.Log("VERIFICA ABAIXO: " + numPecasMarcadas);
            Peca pecaPosterior = PecaInferiorVertical(board, peca.x+i, peca.y);

            if (pecaPosterior != null && pecaPosterior.y == peca.y && pecaPosterior.x >= 0 && pecaPosterior.status == status)
            {
                numPecasMarcadas += 1;
                if (numPecasMarcadas >= range)
                {
                    resultado = true;
                    //Debug.Log("ENCONTROU MATCH COM " + numPecasMarcadas + " PECAS ABAIXO");
                    return resultado;
                }
                else
                {
                    //Debug.Log("PECAS MARCADAS ABAIXO: " + numPecasMarcadas);
                }

            }
            else { break; }
        }

        if (numPecasMarcadas >= range)
        {
            //Debug.Log("ENCONTROU MATCH COM " + numPecasMarcadas);
            resultado = true;
        }
        else
        {
            //Debug.Log("NÃO ENCONTROU MATCH COM " + numPecasMarcadas);
            resultado = false;
        }
        return resultado;
    }

    public bool VerificaDiagonal(int range, int status, int numMatch, Peca peca, GameObject[,] board)
        {
        //Debug.Log("PEÇA PEGA: " + peca.valor);
        bool resultado = false;
        /*IMPORTANTE
         no caso das diagonais, são feitas duas verificações separadas, pois tem 4 possibilidades.
         As verificações corretas são:
         1 - superior direita e inferior esquerda
                    [x+1,y+1]
                [x,y]
            [x-1,y-1]

         2 - superior esquerda e inferior direita
            [x+1,y-1]
                [x,y]
                    [x-1,y+1]

        no caso serão usadas duas variaveis para contar os acertos ceparadamenta que foram definidas como:
        int numPecasMarcadas
        int numPecasMarcadasNova
         */
        //Peca pecaAtual = tabuleiroPecas.Find(peca);

        int numPecasMarcadas = 1;//ja inicializo o numero de peças marcadas com 1 pois a peca que é base desta verificação já foi verificada como verdadeira

        //testando limite SUPERIOR. verificar  peca-1, peca-2,peca-range
        for (int i = 0; i <= range - 1; i++)
        {
            //Debug.Log("VERIFICA SUPERIOR DIREITA DIAGONAL : " + numPecasMarcadas);
            Peca pecaAnterior = PecaSuperiorDireitaDiagonal(board, peca.x - i, peca.y+i);// no caso da horizontal o valor que tem q ser alterado é o da coluna, no caso o y
            if (pecaAnterior != null && pecaAnterior.x >= 0 && pecaAnterior.y>=0 && pecaAnterior.x <= board.GetLength(0) - 1 && pecaAnterior.y <= board.GetLength(1) - 1 && pecaAnterior.status == status)
            {
                numPecasMarcadas += 1;
                if (numPecasMarcadas >= range)
                {
                    resultado = true;
                    //Debug.Log("ENCONTROU MATCH COM " + numPecasMarcadas + " PECAS SUPERIOR DIREITA DIAGONAL");
                    return resultado;
                }
                else
                {
                   // Debug.Log("PECAS MARCADAS SUPERIOR DIREITA DIAGONAL: " + numPecasMarcadas);
                }
            }
            else { break; }
        }

        //testando limite INFERIOR. verificar  peca+1, peca+2,peca+range
        for (int i = 0; i <= range - 1; i++)
        {
            //Debug.Log("VERIFICA INFERIOR ESQUERDA DIAGONAL: " + numPecasMarcadas);
            Peca pecaPosterior = PecaInferiorEsquerdaDiagonal(board, peca.x + i, peca.y - i);

            if (pecaPosterior != null && pecaPosterior.x >= 0 && pecaPosterior.y >= 0 && pecaPosterior.x <= board.GetLength(0) - 1 && pecaPosterior.y <= board.GetLength(1) - 1 && pecaPosterior.status == status)
            {
                numPecasMarcadas += 1;
                if (numPecasMarcadas >= range)
                {
                    resultado = true;
                    //Debug.Log("ENCONTROU MATCH COM " + numPecasMarcadas + " INFERIOR ESQUERDA DIAGONAL");
                    return resultado;
                }
                else
                {
                    //Debug.Log("PECAS MARCADAS INFERIOR ESQUERDA DIAGONAL: " + numPecasMarcadas);
                }
            }
            else { break; }
        }

        // obs: crio uma nova variável para não contar como os casos verificados acima
        int numPecasMarcadasNova = 1;//ja inicializo o numero de peças marcadas com 1 pois a peca que é base desta verificação já foi verificada como verdadeira
        //testando limite SUPERIOR ESQUERDO. verificar  peca+1, peca+2,peca+range
        for (int i = 0; i <= range - 1; i++)
        {
            //Debug.Log("VERIFICA SUPERIOR ESQUERDA DIAGONAL: " + numPecasMarcadasNova);
            Peca pecaPosterior = PecaSuperiorEsquerdaDiagonal(board, peca.x - i, peca.y -i);

            if (pecaPosterior != null && pecaPosterior.x >= 0 && pecaPosterior.y >= 0 && pecaPosterior.x <= board.GetLength(0) - 1 && pecaPosterior.y <= board.GetLength(1) - 1 && pecaPosterior.status == status)
            {
                numPecasMarcadasNova += 1;
                if (numPecasMarcadasNova >= range)
                {
                    resultado = true;
                    //Debug.Log("ENCONTROU MATCH COM " + numPecasMarcadasNova + " SUPERIOR ESQUERDA DIAGONAL");
                    return resultado;
                }
                else
                {
                    //Debug.Log("PECAS MARCADAS SUPERIOR ESQUERDA DIAGONAL: " + numPecasMarcadasNova);
                }

            }
            else { break; }
        }

        //testando limite INFERIOR DIREITO. verificar  peca-1, peca-2,peca-range
        for (int i = 0; i <= range - 1; i++)
        {
            //Debug.Log("VERIFICA INFERIOR DIREITA DIAGONAL : " + numPecasMarcadasNova);
            Peca pecaAnterior = PecaInferiorDireitaDiagonal(board, peca.x + i, peca.y + i);// no caso da horizontal o valor que tem q ser alterado é o da coluna, no caso o y
            if (pecaAnterior != null && pecaAnterior.x >= 0 && pecaAnterior.y >= 0 && pecaAnterior.x <= board.GetLength(0) - 1 && pecaAnterior.y <= board.GetLength(1) - 1 && pecaAnterior.status == status)
            {
                numPecasMarcadasNova += 1;
                if (numPecasMarcadasNova >= range)
                {
                    resultado = true;
                    //Debug.Log("ENCONTROU MATCH COM " + numPecasMarcadasNova + " PECAS INFERIOR DIREITA DIAGONAL");
                    return resultado;
                }
                else
                {
                    //Debug.Log("PECAS MARCADAS INFERIOR DIREITA DIAGONAL: " + numPecasMarcadasNova);
                }
            }
            else { break; }
        }

        if (numPecasMarcadas >= range)
        {
            //Debug.Log("ENCONTROU MATCH COM " + numPecasMarcadas);
            resultado = true;
        }
        else if (numPecasMarcadasNova >= range)
        {
            //Debug.Log("ENCONTROU MATCH COM " + numPecasMarcadasNova);
            resultado = true;
        }
        else
        {
            //Debug.Log("NÃO ENCONTROU MATCH COM " + numPecasMarcadas);
            resultado = false;
        }
        return resultado;
    }
    
    /*  A sequencia pode ser na horizontal, vertical ou diagonal.
      Caso o resultado seja verdadeiro, finaliza o jogo e apresenta o vencedor.
      Caso o resultado seja falso, o jogo continua.*/
}
