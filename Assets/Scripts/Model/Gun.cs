using System.Globalization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    MoverPlayer movePlayer;
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 500f;
    public GameController gc;

    public GameObject referencia;

    private float nextTimeToFire = 2f;

    void Start()
    {
        movePlayer = GetComponent<MoverPlayer>();
        gc = GameController.Instance;
    }

   void FixedUpdate()
    {
        if (gc.gameHasEnded == false && gc.gameOn == true)
        {
            /*   
            if (Input.GetButtonUp(movePlayer.inputFire1) && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();
            }
            if (Input.GetButtonUp(movePlayer.inputFire2) && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                PularRodada();
            }
            */
            if (Application.platform == RuntimePlatform.Android ||Application. platform == RuntimePlatform.IPhonePlayer)
            {
                if (Input.touchCount > 0 && Input.touchCount < 2)
                {   
                    Touch touch = Input.GetTouch(0);
                    if(touch.phase == TouchPhase.Began )
                    {
                        Debug.Log("began");
                        var ray = Camera.main.ScreenPointToRay(touch.position);
                        RaycastHit hitInfo;       
                        if (Physics.Raycast(ray, out hitInfo) & Time.time >= nextTimeToFire)
                        {
                                Peca peca = hitInfo.transform.GetComponent<Peca>();
                                if (peca != null)
                                {
                                    bool resposta;
                                    // neste trecho verificamos se o status da peca é 0, ou seja, se a peça não esta marcada por algum jogador. caso esteja, não verificar novamente para não contabilizar errado
                                    if (peca.status == 0)
                                    {
                                        resposta = gc.VerificaEscolhaJogador(movePlayer.jogadorDisplay, peca, movePlayer.jogadorDisplay.jogador.a, movePlayer.jogadorDisplay.jogador.b, movePlayer.jogadorDisplay.jogador.c);
                                        peca.VerificaResultado(resposta, movePlayer.jogadorDisplay.jogador);
                                        
                                    }else
                                    {
                                        resposta = false;
                                    }
                                    if (resposta == true)
                                    {
                                        SoundManager.PlaySound(SoundManager.Sound.PieceTrue);
                                        bool verificaMatch = gc.match.VerificaSequencia(ES2.Load<int>("matchPoint"), movePlayer.jogadorDisplay.jogador.getIdPlayer(),ES2.Load<int>("matchPoint"), peca, gc.match.piecesInBoard);
                                       // if (verificaMatch == true)
                                      //  {
                                         //   gc.GameOverForMatch(movePlayer.jogadorDisplay.jogador.getIdPlayer());
                                       // }
                                    }
                                    else
                                    {
                                        SoundManager.PlaySound(SoundManager.Sound.PieceFalse);
                                    }
                                }
                            
                            nextTimeToFire = Time.time + 1f / fireRate;
                        }
                        
                    }
                }
            }
            else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
            {
                if (Input.GetMouseButtonDown(0))
                {
                        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hitInfo;       
                        if (Physics.Raycast(ray, out hitInfo) & Time.time >= nextTimeToFire)
                        {
                                Peca peca = hitInfo.transform.GetComponent<Peca>();
                                if (peca != null)
                                {
                                    bool resposta;
                                    // neste trecho verificamos se o status da peca é 0, ou seja, se a peça não esta marcada por algum jogador. caso esteja, não verificar novamente para não contabilizar errado
                                    if (peca.status == 0)
                                    {
                                        resposta = gc.VerificaEscolhaJogador(movePlayer.jogadorDisplay, peca, movePlayer.jogadorDisplay.jogador.a, movePlayer.jogadorDisplay.jogador.b, movePlayer.jogadorDisplay.jogador.c);
                                        peca.VerificaResultado(resposta, movePlayer.jogadorDisplay.jogador);
                                    }else
                                    {
                                        resposta = false;
                                    }
                                    if (resposta == true)
                                    {
                                        SoundManager.PlaySound(SoundManager.Sound.PieceTrue);
                                        bool verificaMatch = gc.match.VerificaSequencia(ES2.Load<int>("matchPoint"), movePlayer.jogadorDisplay.jogador.getIdPlayer(), ES2.Load<int>("matchPoint"), peca, gc.match.piecesInBoard);
                                     /*   if (verificaMatch == true)
                                        {
                                            gc.GameOverForMatch(movePlayer.jogadorDisplay.jogador.getIdPlayer());
                                        }*/
                                    }
                                    else
                                    {
                                        SoundManager.PlaySound(SoundManager.Sound.PieceFalse);
                                    }
                                
                            }
                            nextTimeToFire = Time.time + 1f / fireRate;
                        }
                        
                }            
            }
        }
            /* 
            if (Input.GetButtonUp(movePlayer.inputFire3) && Time.time >= nextTimeToFire)
            {
            nextTimeToFire = Time.time + 1f / fireRate;
            ToogleHelp();
            }
            */
    }
    
    private void checkTouch(Vector3 pos)
    {
        Vector3 wp = Camera.main.ScreenToWorldPoint(pos);
        Vector2 touchPos = new Vector2(wp.x, wp.y);
        Collider2D hit = Physics2D.OverlapPoint(touchPos);  
        if(hit && hit == gameObject.GetComponent<Collider2D>())
        {
            // do something
        }
    }

    public void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(referencia.transform.position, referencia.transform.forward, out hit, range))
        {
            Peca peca = hit.transform.GetComponent<Peca>();
            if (peca != null)
            {
                bool resposta;
                // neste trecho verificamos se o status da peca é 0, ou seja, se a peça não esta marcada por algum jogador. caso esteja, não verificar novamente para não contabilizar errado
                if (peca.status == 0)
                {
                    resposta = gc.VerificaEscolhaJogador(movePlayer.jogadorDisplay, peca, movePlayer.jogadorDisplay.jogador.a, movePlayer.jogadorDisplay.jogador.b, movePlayer.jogadorDisplay.jogador.c);
                    peca.VerificaResultado(resposta, movePlayer.jogadorDisplay.jogador);
                }else
                {
                    resposta = false;
                }
                if (resposta == true)
                {
                   // SoundManager.PlaySound(SoundManager.Sound.PieceTrue);
                  //  bool verificaMatch = gc.match.VerificaSequencia(ES2.Load<int>("matchPoint"), movePlayer.jogadorDisplay.jogador.getIdPlayer(), ES2.Load<int>("matchPoint"), peca, gc.match.piecesInBoard);
                //    if (verificaMatch == true)
                   // {
                  //      gc.GameOverForMatch(movePlayer.jogadorDisplay.jogador.getIdPlayer());
                   // }
                }
                else
                {
                    SoundManager.PlaySound(SoundManager.Sound.PieceFalse);
                }
            }
        }
    }

    public void PularRodada()
    {
        nextTimeToFire = Time.time + 1f / fireRate;
        bool resposta;
        resposta = gc.VerificaResultadosPossiveis(movePlayer.jogadorDisplay, gc.pecasRestantes, movePlayer.jogadorDisplay.jogador.a, movePlayer.jogadorDisplay.jogador.b, movePlayer.jogadorDisplay.jogador.c);
        if (resposta == true)
        {
            SoundManager.PlaySound(SoundManager.Sound.PieceFalse);   
        }
        else
        {
            SoundManager.PlaySound(SoundManager.Sound.PieceTrue);
        }
    }

    public void ToogleHelp()
    {
        if (movePlayer.jogadorDisplay.panelPlayerHelp.activeSelf)
        {
            movePlayer.jogadorDisplay.panelPlayerHelp.SetActive(false);
        }
        else
        {
            movePlayer.jogadorDisplay.panelPlayerHelp.SetActive(true);
        }    
    }
}
