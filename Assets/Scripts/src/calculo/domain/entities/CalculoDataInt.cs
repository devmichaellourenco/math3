using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

public class CalculoDataInt
{
    public void FormatarResultado()
    {
    }
/*A função a seguir
 * possúi todas as fórmulas utilizadas para resolver as questões com operações básicas e parentes envovlendo 3 números
 * são 64 possibilidades base
 * também é utilizado o if(!=0)(){}catch{} para as contas com divisão(/) para evitar possíveis erros como divisão por 0(zero)
 * provavelmente pode ser melhorada para ficar menor
 */
    public List<int> Formulas(int a, int b, int c)
    {
        int aMaisB = a+b;
        int aMaisC = a+c;
        int bMaisC = b+c;

        int aMenosB = a-b;
        int aMenosC = a-c;
        int bMenosC = b-c;

        int aVezesB = a*b;
        int aVezesC = a*c;
        int bVezesC = b*c;

        int aDivideB = a/b;
        int aDivideC = a/c;
        int bDivideC = b/c;

        List<int> resLista = new List<int>();

        //nr = nr
        //abc * 4 * 4 *4 * 6
        //acb * 4 * 2
        //bac * 4 * 2
        //bca * 4 * 2
        //cab * 4 * 2
        //cba * 4 * 2
        //esquerda a b c
        int nr_1 = new int();
        nr_1 = a + b + c; resLista.Add(nr_1);    
        int nr_2 = new int();
        nr_2 = a + b - c; resLista.Add(nr_2);  
        int nr_3 = new int();
        nr_3 = a + b * c; resLista.Add(nr_3);  
        
        int nr_5 = new int();
        nr_5 = a - b - c; resLista.Add(nr_5);
        int nr_6 = new int();
        nr_6 = a - b + c; resLista.Add(nr_6); 
        int nr_7 = new int();
        nr_7 = a - b * c; resLista.Add(nr_7);   
        
        int nr_9 = new int();
        nr_9 = a * b * c; resLista.Add(nr_9);    
        int nr_10 = new int();
        nr_10 = a * b + c; resLista.Add(nr_10);    
        int nr_11 = new int();
        nr_11 = a * b - c; resLista.Add(nr_11); 

        if(c!=0)
        {
            int nr_4 = new int();
            nr_4 = a + b / c; resLista.Add(nr_4); 
            int nr_8 = new int();
            nr_8 = a - b / c; resLista.Add(nr_8);  
            int nr_12 = new int();
            nr_12 = a * b / c; resLista.Add(nr_12);  
        }
        
        if(b!=0 && c!=0)
        {
            int nr_13 = new int();
            nr_13 = a / b / c; resLista.Add(nr_13); 
            int nr_45 = new int();
            nr_45 = (a / b) / c; resLista.Add(nr_45); 
        }
        
        if(b!=0)
        {
            int nr_14 = new int();
            nr_14 = a / b + c; resLista.Add(nr_14);
            int nr_15 = new int();
            nr_15 = a / b - c; resLista.Add(nr_15); 
            int nr_16 = new int();
            nr_16 = a / b * c; resLista.Add(nr_16);  
        }

        //direita a b c
        int nr_17 = new int();
        nr_17 = a + b + c; resLista.Add(nr_17);  
        int nr_18 = new int();
        nr_18 = a - b + c; resLista.Add(nr_18);   
        int nr_19 = new int();
        nr_19 = a * b + c; resLista.Add(nr_19);    
        int nr_21 = new int();
        nr_21 = a - b - c; resLista.Add(nr_21); 
        int nr_22 = new int();
        nr_22 = a + b - c; resLista.Add(nr_22); 
        int nr_23 = new int();
        nr_23 = a * b - c; resLista.Add(nr_23); 
        int nr_25 = new int();
        nr_25 = a * b * c; resLista.Add(nr_25); 
        int nr_26 = new int();
        nr_26 = a + b * c; resLista.Add(nr_26); 
        int nr_27 = new int();
        nr_27 = a - b * c; resLista.Add(nr_27);
        
        if(c!=0)
        {
            int nr_30 = new int();
            nr_30 = a + b / c; resLista.Add(nr_30); 
            int nr_31 = new int();
            nr_31 = a - b / c; resLista.Add(nr_31); 
            int nr_32 = new int();
            nr_32 = a * b / c; resLista.Add(nr_32); 
            int nr_36 = new int();
            nr_36 = (a + b) / c; resLista.Add(nr_36); 
            int nr_40 = new int();
            nr_40 = (a - b) / c; resLista.Add(nr_40); 
            int nr_44 = new int();
            nr_44 = (a * b) / c; resLista.Add(nr_44); 
        }

        //parenteses esquerda (a b) c
        int nr_33 = new int();
        nr_33 = (a + b) + c; resLista.Add(nr_33); 
        int nr_34 = new int();
        nr_34 = (a + b) - c; resLista.Add(nr_34); 
        int nr_35 = new int();
        nr_35 = (a + b) * c; resLista.Add(nr_35);
        int nr_37 = new int();
        nr_37 = (a - b) - c; resLista.Add(nr_37); 
        int nr_38 = new int();
        nr_38 = (a - b) + c; resLista.Add(nr_38); 
        int nr_39 = new int();
        nr_39 = (a - b) * c; resLista.Add(nr_39); 
        int nr_41 = new int();
        nr_41 = (a * b) * c; resLista.Add(nr_41); 
        int nr_42 = new int();
        nr_42 = (a * b) + c; resLista.Add(nr_42); 
        int nr_43 = new int();
        nr_43 = (a * b) - c; resLista.Add(nr_43); 
        
        if(b!=0)
        {
            int nr_46 = new int();
            nr_46 = (a / b) + c; resLista.Add(nr_46); 
            int nr_47 = new int();
            nr_47 = (a / b) - c; resLista.Add(nr_47); 
            int nr_48 = new int();
            nr_48 = (a / b) * c; resLista.Add(nr_48); 
        }

        //parenteses direita a (b c) 
        int nr_49 = new int();
        nr_49 = a + (b + c); resLista.Add(nr_49);
        int nr_50 = new int();
        nr_50 = a - (b + c); resLista.Add(nr_50);
        int nr_51 = new int();
        nr_51 = a * (b + c); resLista.Add(nr_51);

        if(bMaisC!=0)
        {
            int nr_52 = new int();
            nr_52 = a / (b + c); resLista.Add(nr_52);
        }
        
        int nr_53 = new int();
        nr_53 = a - (b - c); resLista.Add(nr_53);
        int nr_54 = new int();
        nr_54 = a + (b - c); resLista.Add(nr_54);
        int nr_55 = new int();
        nr_55 = a * (b - c); resLista.Add(nr_55);
        if(bMenosC!=0)
        {
            int nr_56 = new int();
            nr_56 = a / (b - c); resLista.Add(nr_56);
        }
        
        int nr_57 = new int();
        nr_57 = a * (b * c); resLista.Add(nr_57);
        int nr_58 = new int();
        nr_58 = a + (b * c); resLista.Add(nr_58);
        int nr_59 = new int();
        nr_59 = a - (b * c); resLista.Add(nr_59);
        if(bVezesC!=0)
        {
            int nr_60 = new int();
            nr_60 = a / (b * c); resLista.Add(nr_60);
        }
        
        if(bDivideC!=0)
        {
            int nr_61 = new int();
            nr_61 = a / (b / c); resLista.Add(nr_61);
        }
        
        if(c!=0)
        {
            int nr_62 = new int();
            nr_62 = a + (b / c); resLista.Add(nr_62);
        }
        
        if(c!=0)
        {
            int nr_63 = new int();
            nr_63 = a - (b / c); resLista.Add(nr_63);
        }
        
        if(c!=0)
        {
            int nr_64 = new int();
            nr_64 = a * (b / c); resLista.Add(nr_64);
        }
        
        return resLista; 
    }

    /* A funcção a seguir
    * utiliza da Função Formulas() para executar todas sas 384 possibilidades de contas alternando os valores a, b , c em tres listas de 
    * int
    * Esta função retorna uma Lista de int     
    * */
    public List<int> ExecutaFormulas(int a, int b, int c)
    {
        List<int> resListaFinal = new List<int>();

        List<int> resLista_1 = new List<int>();
        resLista_1 = Formulas(a, b, c);
        foreach(int dados_1 in resLista_1)
        {
            resListaFinal.Add(dados_1);
        }

        List<int> resLista_2 = new List<int>();
        resLista_2 = Formulas(a, c, b);
        foreach (int dados_2 in resLista_2)
        {
            resListaFinal.Add(dados_2);
        }

        List<int> resLista_3 = new List<int>();
        resLista_3 = Formulas(b, a, c);
        foreach (int dados_3 in resLista_3)
        {
            resListaFinal.Add(dados_3);
        }

        List<int> resLista_4 = new List<int>();
        resLista_4 = Formulas(b, c, a);
        foreach (int dados_4 in resLista_4)
        {
            resListaFinal.Add(dados_4);
        }

        List<int> resLista_5 = new List<int>();
        resLista_5 = Formulas(c, a, b);
        foreach (int dados_5 in resLista_5)
        {
            resListaFinal.Add(dados_5);
        }

        List<int> resLista_6 = new List<int>();
        resLista_6 = Formulas(c, b, a);
        foreach (int dados_6 in resLista_6)
        {
            resListaFinal.Add(dados_6);
        }

        return resListaFinal;

    }

    /* A função a seguir
    * é responsável por retornar um ValidaConta, que tem os dados do resultado de um valor escolhido pelo jogador 
    * comparados pelas possibilidades de contas com os valores a,b,c
    * também retorna o valor de verdadeiro ou falso, ou seja, se existe resultado ou não nesta comparação.
    * Utiliza a função ExecutaFormulas para fazer os calculos entre os tres números passados como parãmetro.
    * em seguida insere estes resultados numa variável ValidaConta que é retornada no final
    */
    public ValidaContaData ContaValidada(bool confere, int valorcomparado, int a, int b, int c )
    { 
        List<int> resultados;
        resultados = ExecutaFormulas(a, b, c);

        ValidaContaData resultadoValidado = new ValidaContaData();
        resultadoValidado.valorComparado = valorcomparado;
        confere = false;
        foreach (int resposta in resultados)
        {
            if( valorcomparado == resposta)
            {
                resultadoValidado.confere = true;
                resultadoValidado.resultadosValidados.Add(resposta);
            }

        }
        return resultadoValidado;
    }

    /* A função a seguir
    * verifica toas as contas possiveis dentro de uma lista de números inteiros passados 
    * cada número é comparado com as possibilidades de contas com os parametros a,b,c
    * retorna uma lista de ValidaConta
    */
    public List<ValidaContaData> ContasPossiveis(List<int> valoresParaComparar, int a, int b, int c)
    {
        List<ValidaContaData> resultado = new List<ValidaContaData>();

        foreach(int validar in valoresParaComparar)
        {
            ValidaContaData validaAqui = ContaValidada(false, validar, a, b, c);
            resultado.Add(validaAqui);
        }
        return resultado;
    }
    
    /*A função a seguir
    * retorna verdadeiro ou falso para a verificação de uma lista de intem ValidaConta
    * caso pelo menos 1 dos resultados ValidaConta seja verdadeiro, a função retorna verdadeiro/true
    * senao retorna falso/false
    * essa função deve ser utilizada parar verificar se existia resutado possível quando o jogador decidir passar para a próxima rodada
    * sem escolher um numero no tabuleiro
    */
    public bool VerificaValidade(List<ValidaContaData> contas)
    {
        bool resultado = false;
        foreach(ValidaContaData conta in contas)
        {
            if(conta.confere == true) {
                resultado = true;
                break;
            }
        }
        return resultado;
    }
}
