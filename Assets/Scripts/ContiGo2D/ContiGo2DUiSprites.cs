using TMPro;
using UnityEngine;

/// <summary>
/// Sprites e fontes 2D partilhados (carregados de <c>Resources/ContiGo2D</c>).
/// </summary>
[CreateAssetMenu (fileName = "ContiGo2DUiSprites", menuName = "ContiGo/2D UI Sprites", order = 0)]
public class ContiGo2DUiSprites : ScriptableObject
{
    [Tooltip ("GUI PRO: btn_rectangle_01_n_dark — lista de modos / fallback.")]
    [SerializeField] Sprite _modeListButtonSprite;
    [Tooltip ("Layer Lab: Button01_225_Sky — Home: Desbloqueios e Ranking.")]
    [SerializeField] Sprite _homeNavBlueButtonSprite;
    [Tooltip ("Layer Lab: Button01_225_Green — Home: Cartas.")]
    [SerializeField] Sprite _homeNavGreenButtonSprite;
    [Tooltip ("Layer Lab GUI Pro-CasualGame: LilitaOne-Regular Outline 50 SDF — valores nas células do tabuleiro.")]
    [SerializeField] TMP_FontAsset _boardCellFont;
    [Tooltip ("Layer Lab GUI Pro-CasualGame: Button01_145_BlueGray — fundo de cada célula do tabuleiro.")]
    [SerializeField] Sprite _boardCellBackgroundSprite;
    [Tooltip ("Layer Lab: Button01_225_White — level select Iniciante.")]
    [SerializeField] Sprite _levelSelectInicianteRowSprite;
    [Tooltip ("Layer Lab: Button01_195_Blue — level select Profissional.")]
    [SerializeField] Sprite _levelSelectProfissionalRowSprite;
    [Tooltip ("Layer Lab: Button01_195_Orange — level select Erudito.")]
    [SerializeField] Sprite _levelSelectSabioRowSprite;
    [Tooltip ("Layer Lab: Button01_195_Purple — level select Mestre.")]
    [SerializeField] Sprite _levelSelectMestreRowSprite;
    [Tooltip ("Layer Lab: Button01_195_BlueGray — Desbloqueios, Cartas, Ranking GPGS (botões de ação).")]
    [SerializeField] Sprite _sceneActionButtonBackgroundSprite;
    [Tooltip ("Layer Lab: Icon_PictoIcon_Back — ícone voltar/Home no canto superior esquerdo.")]
    [SerializeField] Sprite _sceneHomeBackIconSprite;
    [Tooltip ("Layer Lab: Button_FlushLeft_Gray — fundo do botão Home no canto superior esquerdo.")]
    [SerializeField] Sprite _sceneHomeButtonFlushBackgroundSprite;
    [Tooltip ("Layer Lab: Icon_PictoIcon_Setting02 — ícone do menu (definições) no HUD do gameplay.")]
    [SerializeField] Sprite _gameplaySettingsIconSprite;
    [Tooltip ("Layer Lab: Button_FlushRight_Gray — fundo do botão menu no canto superior direito (gameplay).")]
    [SerializeField] Sprite _gameplaySettingsFlushRightBackgroundSprite;
    [Tooltip ("Layer Lab: Icon_ImageIcon_Info — ícone «como jogar» / ajuda no HUD do gameplay.")]
    [SerializeField] Sprite _gameplayHowToPlayIconSprite;

    public Sprite ModeListButtonSprite => _modeListButtonSprite;
    public Sprite HomeNavBlueButtonSprite => _homeNavBlueButtonSprite;
    public Sprite HomeNavGreenButtonSprite => _homeNavGreenButtonSprite;
    public TMP_FontAsset BoardCellFont => _boardCellFont;
    public Sprite BoardCellBackgroundSprite => _boardCellBackgroundSprite;
    public Sprite LevelSelectInicianteRowSprite => _levelSelectInicianteRowSprite;
    public Sprite LevelSelectProfissionalRowSprite => _levelSelectProfissionalRowSprite;
    public Sprite LevelSelectSabioRowSprite => _levelSelectSabioRowSprite;
    public Sprite LevelSelectMestreRowSprite => _levelSelectMestreRowSprite;
    public Sprite SceneActionButtonBackgroundSprite => _sceneActionButtonBackgroundSprite;
    public Sprite SceneHomeBackIconSprite => _sceneHomeBackIconSprite;
    public Sprite SceneHomeButtonFlushBackgroundSprite => _sceneHomeButtonFlushBackgroundSprite;
    public Sprite GameplaySettingsIconSprite => _gameplaySettingsIconSprite;
    public Sprite GameplaySettingsFlushRightBackgroundSprite => _gameplaySettingsFlushRightBackgroundSprite;
    public Sprite GameplayHowToPlayIconSprite => _gameplayHowToPlayIconSprite;
}
