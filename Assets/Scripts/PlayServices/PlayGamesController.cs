using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Stub: Google Play Games e leaderboards foram removidos.
/// Quando configurares um novo jogo na Play Console, podes voltar a integrar o SDK aqui.
/// </summary>
public class PlayGamesController : MonoBehaviour {

    public Text mainText;

    private void Start()
    {
        if (mainText != null)
        {
            mainText.text = "Play Games não está configurado nesta build.";
            mainText.color = Color.gray;
        }
        Debug.Log("PlayGamesController: serviços Google Play Games desativados.");
    }

    public static void PostToLeaderboard(long newScore)
    {
        Debug.Log("PlayGamesController: ignorado (leaderboard desativado). Pontuação: " + newScore);
    }

    public static void ShowLeaderboardUI()
    {
        Debug.Log("PlayGamesController: UI de leaderboard desativada.");
    }
}
