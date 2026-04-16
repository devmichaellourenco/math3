using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameController : MonoBehaviour {
    public bool gameOn = false;
    public bool gameStrategicTimer = false;

    public string language;
    // TIMER que é acionado logo no inicio do jogo, que serve para os jogadores observarem o tabuleiro. após terminar este timer, os jogadores podem comecar a jogar
    public TextMeshProUGUI txtStrategicTimer;
    public GameObject strategicTimerScreen;
    public float strategicTimer;
    private bool canCountStrategic = true;
    private bool doOnceStrategic = false;

    public GameObject btnPause;
    public GameObject btnPularRodada;
    // TIMER principal do jogo e seus atributos
    public TextMeshProUGUI txtMainTimer;
    public float mainTimer;
    public long currentMainTimer;

    public float timer;
    private bool canCount = true;
    private bool doOnce = false;
    // FIM TIMER principal e seus atirubos

    public float timerRodada; // esta variavel define o tempo de rodada de cada jogador, que no caso, é igual

    // TIMER jogador 1 do jogo e seus atributos
    //public TextMeshProUGUI txtJ1Timer; no lugar deste campo de texto colocar o campo tempoRodada do jogador 1
    //public float timer1;
    private bool canCount1 = true;
    private bool doOnce1 = false;
    // FIM TIMER jogador 1 e seus atirubos

    public JogadorDisplay j1;

    public GameObject gameOverScreen;
    public static GameController Instance;

    public Array[, ] Tabuleiro = new Array[8, 8];

    public List<GameObject> tabuleiroPecasNovas;

    public List<int> pecasRestantes = new List<int> () { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 44, 45, 48, 50, 54, 55, 60, 64, 66, 72, 75, 80, 90, 96, 100, 108, 120, 125, 144, 150, 180 };

    public bool gameHasEnded = false;

    public Match match;

    void Awake () {
        if (Instance == null)
            Instance = this;
        SoundManager.Initialize ();
    }

    void Start () {
       if (!ES2.Exists("language"))
       {        
           ES2.Save("portuguese", "language");
            this.language = ES2.Load<string>("language");
       } else{
            this.language = ES2.Load<string>("language");
       }
     
     
    }

    void Update () {
        if (gameOn) {
            //ATUALIZA O TIMER PRINCIPAL
            if (timer >= 0.0f && canCount) {
                timer -= Time.deltaTime;
                txtMainTimer.text = TimeConverter (timer);
            } else if (timer <= 0.0f && !doOnce) {
                canCount = false;
                doOnce = true;
                txtMainTimer.text = "0.00";
                timer = 0.0f;
                GameoverForMainTime ();
            }
            //FIM DO ATULIZA TIMER PRINCIPAL
            //ATUALIZA O TIMER 1
            /*if (j1.jogador.tempoRodada >= 0.0f && canCount1) {
                j1.jogador.tempoRodada -= Time.deltaTime;

                j1.txtTempoRodada.text = TimeConverter (j1.jogador.tempoRodada);
            } else if (j1.jogador.tempoRodada <= 0.0f && !doOnce1) {
                j1.jogador.vidas -= 1; //reduz vida do jogador em 1
                SoundManager.PlaySound (SoundManager.Sound.PieceFalse);
                j1.jogador.ResultadoApresentado = "o tempo da rodada acabou";
                NextTurn (j1); // passa para o próximo turno deste jogador
            }*/
            //FIM DO ATULIZA TIMER 1
        } else if (gameStrategicTimer) {
            //ATUALIZA O TIMER STRATEGIC
            if (strategicTimer >= 0.0f && canCountStrategic) {
                strategicTimer -= Time.deltaTime;
                txtStrategicTimer.text = TimeConverterSeconds (strategicTimer);
            } else if (strategicTimer <= 0.0f) //(strategicTimer <= 0.0f && !doOnceStrategic)
            {
                canCountStrategic = false;
                txtStrategicTimer.text = "";
                strategicTimer = 0.0f;
                strategicTimerScreen.SetActive (false);
                gameStrategicTimer = false;
                btnPause.SetActive (true);
                btnPularRodada.SetActive(true);
                PlayGame ();
            }
            //FIM DO ATULIZA TIMER STRATEGIC
        }
    }
    
    public void InitializeGame () {
        ES2.Save (300, "mainTime");
        match.CreateBoard (8, 8);

        //width.text = ES2.Load<int>("width").ToString();
        // height.text = ES2.Load<int>("height").ToString();
        EnsureTimerSettingsExist (); // mesmos defaults que GameData, caso a cena não tenha passado por lá
        mainTimer = ES2.Load<int> ("mainTime"); // pega o tempo do jogo através da configuração salva
        timerRodada = ES2.Load<int> ("playerTime"); // pega o tempo da jogada do jogador atrvés da configuração salva
        strategicTimer = ES2.Load<int> ("strategicTime"); // pega o tempo de inicio estratégico do game
        gameHasEnded = false;

        timer = mainTimer; // inicializa o TIMER PRINCIPAL
        canCount = true; // seta a possibilidade de comecar a contagem de tempo para true

        canCount1 = true; // seta a possibilidade de comecar a contagem de tempo para true

        Dado dado1 = new Dado ();
        int v1 = dado1.RolarDado (1, 7); //valores de 1 até 6.o 7 não é incluso

        int v2 = dado1.RolarDado (1, 7);

        int v3 = dado1.RolarDado (1, 7);

        j1.jogador.AtualizarDadosJogador (1, 3, 0, v1, v2, v3, 1, timerRodada, 0, j1.jogador.ResultadoApresentado);
        j1.AtualizarDisplayJogador (j1.jogador.getIdPlayer (), j1.jogador.vidas, j1.jogador.a, j1.jogador.b, j1.jogador.c, j1.jogador.rodada, j1.jogador.tempoRodada, j1.jogador.pontos, "");

        canCountStrategic = false; // faz com que o counter inicial não começe imediatamente
        PauseGame (); // pausa o jogo
    }

    /*
     * A função a seguir é uma das principais do gameplay
     * A partir dos dados do jogador j, do valor escolhido pelo mesmo no tabuleiro e os valores que sairam nos seus dados
     * esta função retornará se sua escolha é verdadeira ou falsa
     * caso encontre resposta, o botão da  peca escolhida será pintado com a cor do jogador j e marcado como indisponivel.
     * será iniciada uma nova rodada.
     * caso não encontre resposta, o jogador j perderá uma chance/vida e será iniciada uma nova rodada.
     */
    public bool VerificaEscolhaJogador (JogadorDisplay j, Peca peca, int a, int b, int c) {
        bool resposta = false;
        int adicionaRodadaJogador = j.jogador.rodada + 1; // a rodada do jogador será adicionada independente do resultado 

        CalculoData calculo = new CalculoData ();
        List<float> resultadosComOsValoresABC =  calculo.ExecutaFormulas(a,b,c);
        //List<int> resultadosComOsValoresABC = new List<int> () { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35};

        ValidaContaData calculoResultado = new ValidaContaData ();
        calculoResultado.ContaValidada (false, resultadosComOsValoresABC, peca.valor);
        
        string resultadoApresentado;

        if (calculoResultado.confere == true) // se encontrou resultado do valor que o jogador escolheu
        {
            //Atualizando valor dos dados para o novo turno
            Dado dado = new Dado (); // a classe Dado tem a função RolarDado. Esta função retorna um valor inteiro entre dois numeros.
            int v1 = dado.RolarDado (1, 7); //valores de 1 até 6.o 7 não é incluso

            int v2 = dado.RolarDado (1, 7);

            int v3 = dado.RolarDado (1, 7);

            int i = 1;
            if(language=="portuguese"){
                resultadoApresentado = "acertou!";
            }
            else{
                resultadoApresentado = "correct!";  
            }
            j.jogador.AtualizarDadosJogador (j.jogador.getIdPlayer (), j.jogador.vidas, 0, v1, v2, v3, adicionaRodadaJogador, timerRodada, j.jogador.pontos + 1, j.jogador.ResultadoApresentado); // esta função está na classe jogador, neste caso representado pela variável j.
            j.AtualizarDisplayJogador (j.jogador.getIdPlayer (), j.jogador.vidas, v1, v2, v3, j.jogador.rodada, timerRodada, j.jogador.pontos, resultadoApresentado);
            j.GetComponentInChildren<TweenDados> ().OnClose ();
            j.GetComponentInChildren<TweenResultados> ().OnClose ();
            j.GetComponentInChildren<TweenPontos> ().OnClose ();

            TiraItemdaLista (pecasRestantes, peca.valor); // retira a peca da lista de pecas restantes 

            resposta = true;
            return resposta;
        } else // se não encontrou valor que o jogador escolheu
        {
            j.jogador.vidas -= 1;
            if(language=="portuguese"){
                j.jogador.ResultadoApresentado = "errou!";
            }
            else{
                j.jogador.ResultadoApresentado = "fail!";    
            }
            j.GetComponentInChildren<TweenDados> ().OnClose ();
            j.GetComponentInChildren<TweenResultados> ().OnClose ();
            j.GetComponentInChildren<TweenChances> ().OnClose ();

            NextTurn (j);

            int i = 1;
            resposta = false;
            return resposta;
        }
    }

    /* 
     * A função a seguir verifica os resultados possíveis a partir das peças restantes no tabuleiro
     * ela utiliza outras duas funções da o arquivo CalculoData.cs :
     * ContasPossíveis() => retorna uma lista de resultados ValidaConta 
     * VerificaValidade() => retorna verdadeiro caso tenha pelo menos 1 valor encontrardo entre os números restantes do tabuleiro 
     * e os numeros avaliados dos dados. Caso contrario, ou seja, nao encontre resultado valido, retorna falso
     * se for verdadeiro, retira um ponto de chance/vida do jogador j e passa para a próxima rodada
     * caso falso, passa para a próxima rodada sem retirar chance/vida do jogador j
     */
    public bool VerificaResultadosPossiveis (JogadorDisplay j, List<int> pecasRest, float a, float b, float c) {
        int adicionaRodadaJogador = j.jogador.rodada + 1; // a rodada do jogador será adicionada independente do resultado 
        j.jogador.tempoRodada = timerRodada; //reiniciando o contador para próxima rodada

        string resultadoApresentado = "";

        CalculoData contas = new CalculoData ();
        List<ValidaContaData> valeounaoData = contas.ContasPossiveis (pecasRest, (float)a, (float)b, (float)c);

        bool existePeloMenosUmResultadoPossivel = contas.VerificaValidade (valeounaoData);

        if (existePeloMenosUmResultadoPossivel == true) {
            if(language=="portuguese"){
                j.jogador.ResultadoApresentado = "errou!";
            }
            else{
                j.jogador.ResultadoApresentado = "fail!";    
            }
            
            j.GetComponentInChildren<TweenDados> ().OnClose ();
            j.GetComponentInChildren<TweenResultados> ().OnClose ();
            j.GetComponentInChildren<TweenChances> ().OnClose ();
            Dado dado = new Dado (); // a classe Dado tem a função RolarDado. Esta função retorna um valor inteiro entre dois numeros.
            int v1 = dado.RolarDado (1, 7); //valores de 1 até 6.o 7 não é incluso

            int v2 = dado.RolarDado (1, 7);

            int v3 = dado.RolarDado (1, 7);

            j.jogador.vidas -= 1;

            j.jogador.AtualizarDadosJogador (j.jogador.getIdPlayer (), j.jogador.vidas, 0, v1, v2, v3, adicionaRodadaJogador, timerRodada, j.jogador.pontos, j.jogador.ResultadoApresentado); // esta função está na classe jogador, neste caso representado pela variável j.
            j.AtualizarDisplayJogador (j.jogador.getIdPlayer (), j.jogador.vidas, v1, v2, v3, j.jogador.rodada, timerRodada, j.jogador.pontos, resultadoApresentado);

            NextTurn (j);

            return existePeloMenosUmResultadoPossivel;
        } else {
            if(language=="portuguese"){
                j.jogador.ResultadoApresentado = "acertou";
            }
            else{
                j.jogador.ResultadoApresentado = "correct!";
            }

            j.GetComponentInChildren<TweenDados> ().OnClose ();
            j.GetComponentInChildren<TweenResultados> ().OnClose ();

            SoundManager.PlaySound (SoundManager.Sound.PieceTrue);
            Dado dado = new Dado (); // a classe Dado tem a função RolarDado. Esta função retorna um valor inteiro entre dois numeros.
            int v1 = dado.RolarDado (1, 7); //valores de 1 até 6.o 7 não é incluso

            int v2 = dado.RolarDado (1, 7);

            int v3 = dado.RolarDado (1, 7);

            j.jogador.AtualizarDadosJogador (j.jogador.getIdPlayer (), j.jogador.vidas, 0, v1, v2, v3, adicionaRodadaJogador, timerRodada, j.jogador.pontos, j.jogador.ResultadoApresentado); // esta função está na classe jogador, neste caso representado pela variável j.
            j.AtualizarDisplayJogador (j.jogador.getIdPlayer (), j.jogador.vidas, v1, v2, v3, j.jogador.rodada, timerRodada, j.jogador.pontos, resultadoApresentado);

            NextTurn (j);
            return existePeloMenosUmResultadoPossivel;
        }
    }

    //a funçao a seguir retira uma peca da lista de pecas restantes
    public void TiraItemdaLista (List<int> pecas, int peca) {
        foreach (int item in pecas) {
            if (item == peca) {
                pecas.Remove (item);
                break;
            }
        }
    }

    // a função a seguir serve para resetar o game
    public void ResetBtn () {
        timer = mainTimer;
        canCount = true;
        doOnce = false;
    }

    /* A função a seguir é responsável por finalizar o game 
    ela recebe como parâmetro o id do player perdedor e executará uma série de cálculos para obter os resultados
    */
    public void Finalizar () {
        Debug.Log ("Acabou o tempo");
    }

    public void GameOver () {
        gameOn = false;
        if (gameHasEnded == false) {
            gameHasEnded = true;

            btnPause.SetActive (false);

            //Exibe Tela Game Over
            gameOverScreen.SetActive (true);
            RankingData rd = new RankingData ();
            List<RankingEntry> rankingPego = rd.LoadRankingData ();
            int pts = (int)j1.jogador.pontos;
            int timeRemaining = Mathf.CeilToInt (Mathf.Max (0f, timer));
            int timeSpent = Mathf.CeilToInt (Mathf.Max (0f, mainTimer - Mathf.Max (0f, timer)));
            RankingEntry entry = new RankingEntry (pts, timeSpent);
            entry.timeRemainingSeconds = timeRemaining;
            rankingPego.Add (entry);
            List<RankingEntry> rankingAtualizado = rankingPego
                .OrderBy (e => e, Comparer<RankingEntry>.Create (RankingEntry.Compare))
                .Take (10)
                .ToList ();
            rd.SaveRankingData (rankingAtualizado);
            if(language=="portuguese"){
            gameOverScreen.GetComponentInChildren<TextMeshProUGUI> ().text = j1.jogador.pontos+" Pontos!";

            }
            else{
            gameOverScreen.GetComponentInChildren<TextMeshProUGUI> ().text = j1.jogador.pontos+" Points!";
            }
          //  }
        }
        //ResetGame ();
    }

    
    public void GameoverForMainTime () {
        gameOn = false;
        gameHasEnded = true;
        

        btnPause.SetActive (false);
        //exibir vencedor
        gameOverScreen.SetActive (true);

            
        RankingData rd = new RankingData ();
        List<RankingEntry> rankingPego = rd.LoadRankingData ();
        int pts = (int)j1.jogador.pontos;
        int timeRemaining = Mathf.CeilToInt (Mathf.Max (0f, timer));
        int timeSpent = Mathf.CeilToInt (Mathf.Max (0f, mainTimer - Mathf.Max (0f, timer)));
        RankingEntry entry = new RankingEntry (pts, timeSpent);
        entry.timeRemainingSeconds = timeRemaining;
        rankingPego.Add (entry);
        List<RankingEntry> rankingAtualizado = rankingPego
            .OrderBy (e => e, Comparer<RankingEntry>.Create (RankingEntry.Compare))
            .Take (10)
            .ToList ();
        rd.SaveRankingData (rankingAtualizado);

        if(language=="portuguese"){
            gameOverScreen.GetComponentInChildren<TextMeshProUGUI> ().text = j1.jogador.pontos+" Pontos!";
        }
        else{
            gameOverScreen.GetComponentInChildren<TextMeshProUGUI> ().text = j1.jogador.pontos+" Points!";
        }
       
    }
    public void PularRodada()
    {
        
        bool resposta;
        resposta = VerificaResultadosPossiveis(j1, pecasRestantes,j1.jogador.a, j1.jogador.b, j1.jogador.c);
        if (resposta == true)
        {
            SoundManager.PlaySound(SoundManager.Sound.PieceFalse);   
        }
        else
        {
            SoundManager.PlaySound(SoundManager.Sound.PieceTrue);
        }
    }
    public void NextTurn (JogadorDisplay jNew) {
        int adicionaRodadaJogador = jNew.jogador.rodada + 1;

        Dado dado = new Dado (); // a classe Dado tem a função RolarDado. Esta função retorna um valor inteiro entre dois numeros.
        int v1 = dado.RolarDado (1, 7); //valores de 1 até 6.o 7 não é incluso

        int v2 = dado.RolarDado (1, 7);

        int v3 = dado.RolarDado (1, 7);

        jNew.txtTempoRodada.text = "0.00";
        jNew.jogador.tempoRodada = timerRodada;

        jNew.jogador.AtualizarDadosJogador (jNew.jogador.getIdPlayer (), jNew.jogador.vidas, 0, v1, v2, v3, adicionaRodadaJogador, jNew.jogador.tempoRodada, jNew.jogador.pontos, jNew.jogador.ResultadoApresentado); // esta função está na classe jogador, neste caso representado pela variável j.
        jNew.AtualizarDisplayJogador (jNew.jogador.getIdPlayer (), jNew.jogador.vidas, v1, v2, v3, jNew.jogador.rodada, jNew.jogador.tempoRodada, jNew.jogador.pontos, jNew.jogador.ResultadoApresentado);

        if (jNew.jogador.vidas <= 0) {
            GameOver ();
        }
    }

    //FUNÇÕES UTILITÁRIAS

    /// <summary> Garante ficheiros ES2 dos temporizadores (primeira execução ou cena sem GameData). </summary>
    private void EnsureTimerSettingsExist () {
        if (!ES2.Exists ("mainTime")) {
            ES2.Save (300, "mainTime");
        }
        if (!ES2.Exists ("playerTime")) {
            ES2.Save (30, "playerTime");
        }
        if (!ES2.Exists ("strategicTime")) {
            ES2.Save (5, "strategicTime");
        }
    }

    // A função a seguir transforma um número passado em formato de minutos e segundos e retorna uma string com este texto.
    public string TimeConverter (float num) {
        string timeConverted;
        TimeSpan spanTime = TimeSpan.FromSeconds (num);
        timeConverted = spanTime.Minutes + ":" + spanTime.Seconds;
        return timeConverted;
    }

    public string TimeConverterSeconds (float num) {
        string timeConverted;
        TimeSpan spanTime = TimeSpan.FromSeconds (num);
        timeConverted = spanTime.Seconds.ToString ();
        return timeConverted;
    }

    public void PauseGame () {
        gameOn = false;
    }

    public void PlayGame () {
        gameOn = true;
        SoundManager.PlaySound (SoundManager.Sound.UIConfimation);
    }

    // esta função faz com que, quando o jogo inicie, primeiro seja contado o tempo para os jogadores analisarem o tabuleiro, só depois comeca o jogo.
    public void PlayStrategicTimer () {
        gameOn = false;
        strategicTimerScreen.SetActive (true);
        gameStrategicTimer = true;
        canCountStrategic = true;
        //SoundManager.PlaySound(SoundManager.Sound.UIConfimation);
    }

    public void CancelChange () {
        SoundManager.PlaySound (SoundManager.Sound.UICancel);
    }

    public void SaveChange () {
        gameOn = true;
        SoundManager.PlaySound (SoundManager.Sound.UIConfimation);
    }

    public void ResetGame () {
        foreach (GameObject peca in tabuleiroPecasNovas) {
            Destroy (peca);
        }
        tabuleiroPecasNovas.Clear ();
        pecasRestantes = new List<int> () { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 44, 45, 48, 50, 54, 55, 60, 64, 66, 72, 75, 80, 90, 96, 100, 108, 120, 125, 144, 150, 180 };

        EnsureTimerSettingsExist ();
        mainTimer = ES2.Load<int> ("mainTime"); // pega o tempo do jogo através da configuração salva
        timerRodada = ES2.Load<int> ("playerTime"); // pega o tempo da jogada do jogador atrvés da configuração salva
        strategicTimer = ES2.Load<int> ("strategicTime"); // pega o tempo de inicio estratégico do game
        gameHasEnded = false;

        timer = mainTimer; // inicializa o TIMER PRINCIPAL
        canCount = true; // seta a possibilidade de comecar a contagem de tempo para true
        doOnce = false; // seta doOnce para false

        canCount1 = true; // seta a possibilidade de comecar a contagem de tempo para true

        Dado dado1 = new Dado ();
        int v1 = dado1.RolarDado (1, 7); //valores de 1 até 6.o 7 não é incluso

        int v2 = dado1.RolarDado (1, 7);

        int v3 = dado1.RolarDado (1, 7);

        j1.jogador.AtualizarDadosJogador (1, 3, 0, v1, v2, v3, 1, timerRodada, 0, j1.jogador.ResultadoApresentado);
        j1.AtualizarDisplayJogador (j1.jogador.getIdPlayer (), j1.jogador.vidas, j1.jogador.a, j1.jogador.b, j1.jogador.c, j1.jogador.rodada, j1.jogador.tempoRodada, j1.jogador.pontos, "");

        canCountStrategic = false; // faz com que o counter inicial não começe imediatamente
        PauseGame (); // pausa o jogo
    }
    public void OnApplicationPause(bool pauseStatus){
        Debug.Log("paused: "+pauseStatus);
    }
    public void OnApplicationFocus(bool focusStatus) {
        if(gameOn)
        {
            if(focusStatus==false){
                Debug.Log("your app is NO LONGER in the background");
                DateTime horarioQuePerdeuFoco = DateTime.UtcNow;
                ES2.Save(horarioQuePerdeuFoco.ToBinary(), "currentMainTime");
                Debug.Log("FOI PARA O BACKGROUND EM: " + horarioQuePerdeuFoco.ToString());
                this.j1.jogador.ResultadoApresentado = "FOI PARA O BACKGROUND EM: " + horarioQuePerdeuFoco.ToString();
            }
            else if(focusStatus==true){
                currentMainTimer = ES2.Load<long> ("currentMainTime");
                DateTime recebeHorarioQuePerdeuFoco = DateTime.FromBinary(currentMainTimer);
                DateTime horarioQueGanhouFoco = DateTime.UtcNow;
                TimeSpan nowInSeconds = horarioQueGanhouFoco.Subtract(recebeHorarioQuePerdeuFoco); 
                float timeLapsed = timer - (float)nowInSeconds.TotalSeconds;        
                Debug.Log("O timer era:"+ timer.ToString()+"Segundos. O tempo que passou é:"+ (float)nowInSeconds.TotalSeconds+" SEGUNDOS dando uma diferença de " +timeLapsed.ToString() + " SEGUNDOS");
                Debug.Log("SAIU DO BACKGROUND EM: " +horarioQueGanhouFoco.ToString());
                if(timeLapsed<=0){
                    GameOver();
                }else{
                    timer = timeLapsed;
                }     
                j1.jogador.ResultadoApresentado = "SAIU EM "+ timeLapsed.ToString() + " SEGUNDOS";
            }
        }
    }
}