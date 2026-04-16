using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

public class CalculoData
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
    public List<float> Formulas(float a, float b, float c)
    {
        float aMaisB = a+b;
        float aMaisC = a+c;
        float bMaisC = b+c;

        float aMenosB = a-b;
        float aMenosC = a-c;
        float bMenosC = b-c;

        float aVezesB = a*b;
        float aVezesC = a*c;
        float bVezesC = b*c;

        float aDivideB = a/b;
        float aDivideC = a/c;
        float bDivideC = b/c;

        List<float> resLista = new List<float>();

        //nr = nr
        //abc * 4 * 4 *4 * 6
        //acb * 4 * 2
        //bac * 4 * 2
        //bca * 4 * 2
        //cab * 4 * 2
        //cba * 4 * 2
        //esquerda a b c
        float nr_1 = new float();
        nr_1 = a + b + c; resLista.Add(nr_1);    
        float nr_2 = new float();
        nr_2 = a + b - c; resLista.Add(nr_2);  
        float nr_3 = new float();
        nr_3 = a + b * c; resLista.Add(nr_3);  
        
        float nr_5 = new float();
        nr_5 = a - b - c; resLista.Add(nr_5);
        float nr_6 = new float();
        nr_6 = a - b + c; resLista.Add(nr_6); 
        float nr_7 = new float();
        nr_7 = a - b * c; resLista.Add(nr_7);   
        
        float nr_9 = new float();
        nr_9 = a * b * c; resLista.Add(nr_9);    
        float nr_10 = new float();
        nr_10 = a * b + c; resLista.Add(nr_10);    
        float nr_11 = new float();
        nr_11 = a * b - c; resLista.Add(nr_11); 

        if(c!=0)
        {
            float nr_4 = new float();
            nr_4 = a + b / c; resLista.Add(nr_4); 
            float nr_8 = new float();
            nr_8 = a - b / c; resLista.Add(nr_8);  
            float nr_12 = new float();
            nr_12 = a * b / c; resLista.Add(nr_12);  
        }
        
        if(b!=0 && c!=0)
        {
            float nr_13 = new float();
            nr_13 = a / b / c; resLista.Add(nr_13); 
            float nr_45 = new float();
            nr_45 = (a / b) / c; resLista.Add(nr_45); 
        }
        
        if(b!=0)
        {
            float nr_14 = new float();
            nr_14 = a / b + c; resLista.Add(nr_14);
            float nr_15 = new float();
            nr_15 = a / b - c; resLista.Add(nr_15); 
            float nr_16 = new float();
            nr_16 = a / b * c; resLista.Add(nr_16);  
        }

        //direita a b c
        float nr_17 = new float();
        nr_17 = a + b + c; resLista.Add(nr_17);  
        float nr_18 = new float();
        nr_18 = a - b + c; resLista.Add(nr_18);   
        float nr_19 = new float();
        nr_19 = a * b + c; resLista.Add(nr_19);    
        float nr_21 = new float();
        nr_21 = a - b - c; resLista.Add(nr_21); 
        float nr_22 = new float();
        nr_22 = a + b - c; resLista.Add(nr_22); 
        float nr_23 = new float();
        nr_23 = a * b - c; resLista.Add(nr_23); 
        float nr_25 = new float();
        nr_25 = a * b * c; resLista.Add(nr_25); 
        float nr_26 = new float();
        nr_26 = a + b * c; resLista.Add(nr_26); 
        float nr_27 = new float();
        nr_27 = a - b * c; resLista.Add(nr_27);
        
        if(c!=0)
        {
            float nr_30 = new float();
            nr_30 = a + b / c; resLista.Add(nr_30); 
            float nr_31 = new float();
            nr_31 = a - b / c; resLista.Add(nr_31); 
            float nr_32 = new float();
            nr_32 = a * b / c; resLista.Add(nr_32); 
            float nr_36 = new float();
            nr_36 = (a + b) / c; resLista.Add(nr_36); 
            float nr_40 = new float();
            nr_40 = (a - b) / c; resLista.Add(nr_40); 
            float nr_44 = new float();
            nr_44 = (a * b) / c; resLista.Add(nr_44); 
        }

        //parenteses esquerda (a b) c
        float nr_33 = new float();
        nr_33 = (a + b) + c; resLista.Add(nr_33); 
        float nr_34 = new float();
        nr_34 = (a + b) - c; resLista.Add(nr_34); 
        float nr_35 = new float();
        nr_35 = (a + b) * c; resLista.Add(nr_35);
        float nr_37 = new float();
        nr_37 = (a - b) - c; resLista.Add(nr_37); 
        float nr_38 = new float();
        nr_38 = (a - b) + c; resLista.Add(nr_38); 
        float nr_39 = new float();
        nr_39 = (a - b) * c; resLista.Add(nr_39); 
        float nr_41 = new float();
        nr_41 = (a * b) * c; resLista.Add(nr_41); 
        float nr_42 = new float();
        nr_42 = (a * b) + c; resLista.Add(nr_42); 
        float nr_43 = new float();
        nr_43 = (a * b) - c; resLista.Add(nr_43); 
        
        if(b!=0)
        {
            float nr_46 = new float();
            nr_46 = (a / b) + c; resLista.Add(nr_46); 
            float nr_47 = new float();
            nr_47 = (a / b) - c; resLista.Add(nr_47); 
            float nr_48 = new float();
            nr_48 = (a / b) * c; resLista.Add(nr_48); 
        }

        //parenteses direita a (b c) 
        float nr_49 = new float();
        nr_49 = a + (b + c); resLista.Add(nr_49);
        float nr_50 = new float();
        nr_50 = a - (b + c); resLista.Add(nr_50);
        float nr_51 = new float();
        nr_51 = a * (b + c); resLista.Add(nr_51);

        if(bMaisC!=0)
        {
            float nr_52 = new float();
            nr_52 = a / (b + c); resLista.Add(nr_52);
        }
        
        float nr_53 = new float();
        nr_53 = a - (b - c); resLista.Add(nr_53);
        float nr_54 = new float();
        nr_54 = a + (b - c); resLista.Add(nr_54);
        float nr_55 = new float();
        nr_55 = a * (b - c); resLista.Add(nr_55);
        if(bMenosC!=0)
        {
            float nr_56 = new float();
            nr_56 = a / (b - c); resLista.Add(nr_56);
        }
        
        float nr_57 = new float();
        nr_57 = a * (b * c); resLista.Add(nr_57);
        float nr_58 = new float();
        nr_58 = a + (b * c); resLista.Add(nr_58);
        float nr_59 = new float();
        nr_59 = a - (b * c); resLista.Add(nr_59);
        if(bVezesC!=0)
        {
            float nr_60 = new float();
            nr_60 = a / (b * c); resLista.Add(nr_60);
        }
        
        if(bDivideC!=0)
        {
            float nr_61 = new float();
            nr_61 = a / (b / c); resLista.Add(nr_61);
        }
        
        if(c!=0)
        {
            float nr_62 = new float();
            nr_62 = a + (b / c); resLista.Add(nr_62);
        }
        
        if(c!=0)
        {
            float nr_63 = new float();
            nr_63 = a - (b / c); resLista.Add(nr_63);
        }
        
        if(c!=0)
        {
            float nr_64 = new float();
            nr_64 = a * (b / c); resLista.Add(nr_64);
        }
        
        return resLista; 
    }

    /* A funcção a seguir
    * utiliza da Função Formulas() para executar todas sas 384 possibilidades de contas alternando os valores a, b , c em tres listas de 
    * float
    * Esta função retorna uma Lista de float     
    * */
    public List<float> ExecutaFormulas(float a, float b, float c)
    {
        List<float> resListaFinal = new List<float>();

        List<float> resLista_1 = new List<float>();
        resLista_1 = Formulas(a, b, c);
        foreach(float dados_1 in resLista_1)
        {
            resListaFinal.Add(dados_1);
        }

        List<float> resLista_2 = new List<float>();
        resLista_2 = Formulas(a, c, b);
        foreach (float dados_2 in resLista_2)
        {
            resListaFinal.Add(dados_2);
        }

        List<float> resLista_3 = new List<float>();
        resLista_3 = Formulas(b, a, c);
        foreach (float dados_3 in resLista_3)
        {
            resListaFinal.Add(dados_3);
        }

        List<float> resLista_4 = new List<float>();
        resLista_4 = Formulas(b, c, a);
        foreach (float dados_4 in resLista_4)
        {
            resListaFinal.Add(dados_4);
        }

        List<float> resLista_5 = new List<float>();
        resLista_5 = Formulas(c, a, b);
        foreach (float dados_5 in resLista_5)
        {
            resListaFinal.Add(dados_5);
        }

        List<float> resLista_6 = new List<float>();
        resLista_6 = Formulas(c, b, a);
        foreach (float dados_6 in resLista_6)
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
    public ValidaContaData ContaValidada(bool confere, int valorcomparado, float a, float b, float c )
    { 
        List<float> resultados;
        resultados = ExecutaFormulas(a, b, c);

        ValidaContaData resultadoValidado = new ValidaContaData();
        resultadoValidado.valorComparado = valorcomparado;
        confere = false;
        foreach (float resposta in resultados)
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
    * verifica toas as contas possiveis dentro de uma lista de números floateiros passados 
    * cada número é comparado com as possibilidades de contas com os parametros a,b,c
    * retorna uma lista de ValidaConta
    */
    public List<ValidaContaData> ContasPossiveis(List<int> valoresParaComparar, float a, float b, float c)
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
    * retorna verdadeiro ou falso para a verificação de uma lista de floatem ValidaConta
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
