# Google Play Games Services (GPGS) + Leaderboard (Unity) — Tutorial

Este guia documenta o fluxo **completo** para integrar **Google Play Games Services** (login) e **Tabela de Classificação (Leaderboard)** num jogo Unity Android, baseado nos passos que fizemos neste projeto.

## Pré‑requisitos

- **Unity Android Build Support** instalado.
- **Google Play Console** com a app criada.
- **Google Play Games Services** configurado para a mesma app (mesmo *package name*).
- Conta(s) Google de teste (para track interno/fechado).

## 1) Instalar o plugin do Google Play Games no Unity

1. Importa o plugin **Google Play Games** para Unity (neste projeto está em `Assets/GooglePlayGames/...`).
2. Confirma que o **External Dependency Manager** (EDM4U) está presente (neste projeto está em `Assets/ExternalDependencyManager/...`).

### Erro comum (Editor/Windows): iOS Resolver não carrega

No Windows, o Unity pode acusar:

- `Unable to resolve reference 'UnityEditor.iOS.Extensions.Xcode'`

Isto acontece porque o **iOS Build Support** não está instalado e o EDM inclui um resolver de iOS.

Opções:

- **Recomendado** (se só builds Android no Windows): desativar validação de referências no plugin (Plugin Inspector) ou no `.meta` do DLL.
- Alternativa: instalar **iOS Build Support** (não necessário para Android).

## 2) Rodar o Setup do GPGS no Unity (gerar `GPGSIds.cs`)

1. No menu do plugin GPGS, faz o “Setup” e vincula o **App ID** do Play Games.
2. Gera as constantes. Isso cria/atualiza:
   - `Assets/GPGSIds.cs`

Exemplo de constante (o nome pode variar):

- `GPGSIds.leaderboard_ranking = "CgkI..."`

## 3) Criar e publicar o Leaderboard no Play Console

No Play Console:

1. Vai em **Play Games Services → Tabelas de classificação**.
2. Cria a leaderboard (ex.: “RANKING”).
3. Copia o **ID** (formato `CgkI...`).
4. Marca como **Publicada** (e publica as alterações do Play Games Services, não só da app).

> Nota: Leaderboard “rascunho” pode resultar em `Leaderboard not found`.

## 4) O ponto que mais dá “offline”: SHA‑1 correto no Cliente OAuth Android

Se no jogo ficar **offline** mesmo com tudo configurado, quase sempre é **SHA‑1 errado**.

### Por quê?

O Play Games valida que o app instalado foi assinado por um certificado cujo **SHA‑1** está cadastrado no **Cliente OAuth Android**.

- Se você instala pelo **track interno (Play Store)**, o APK que chega no aparelho costuma ser assinado com o **App signing certificate** (Google Play App Signing).
- Se você instala um APK local (build direto do Unity), ele é assinado pela tua **keystore local** (debug/release).

Cada assinatura requer o SHA‑1 correspondente cadastrado.

### Como configurar corretamente

1. No Play Console, abre **Integridade do app (App integrity)**.
2. Copia o SHA‑1 do:
   - **App signing certificate** (para apps instaladas via Play Store / tracks)
   - (opcional) **Upload certificate** (não é o mesmo da assinatura final que chega no aparelho)
3. Em **Play Games Services → Configuração → Cliente OAuth → Cliente Android**, garante que:
   - **Nome do pacote** é o mesmo do jogo (ex.: `com.seu.estudio.seu_jogo`)
   - **Impressão digital SHA‑1** é a correta (App signing para track interno)
4. Salva e **Publica** a configuração do Play Games Services.

Depois:

- No aparelho, (opcional mas recomendado) limpa dados de:
  - **Google Play Jogos**
  - **Google Play Services**
  - reinicia o aparelho

## 5) Testers: não basta o teste interno da app

Para a conta conseguir autenticar no Play Games:

1. Adiciona o email em **Play Games Services → Testadores**.
2. Também garante que a conta entrou no teste interno/fechado da app e instalou via Play Store.

## 6) Unity: ativar GPGS e autenticar cedo (login silencioso)

### Por que logar cedo?

- Leaderboard depende de autenticação.
- Melhor UX: tentar login **silencioso** ao abrir o jogo/menu; só pede interação se necessário.

### Implementação usada neste projeto

Arquivo principal:

- `Assets/Scripts/PlayServices/PlayGamesController.cs`

Pontos importantes:

- Ativa o provider do GPGS no Android com `PlayGamesPlatform.Activate()`.
- Tenta login silencioso ao iniciar (`TrySignInSilent()`).
- `ResolveLeaderboardId()` usa `GPGSIds.leaderboard_ranking` como fallback.
- Há uma UI de debug (`mainText`) para mostrar:
  - `authenticated`, `userName`, `userId`, `leaderboardId`

## 7) Cena de debug (GPGSAuth) + botão no menu

Criamos/ativamos uma cena de diagnóstico:

- Cena: `Assets/Scenes/GPGSAuth.unity`
- Ela deve estar **habilitada** no Build Settings:
  - `ProjectSettings/EditorBuildSettings.asset` (entrada de `GPGSAuth.unity` com `enabled: 1`)

Também adicionamos um botão no menu de modos (`ContiGo2DLevelSelectUI`) para ir para `GPGSAuth`, e um login silencioso ao abrir o menu.

## 8) Cena do ranking (Top scores) e UI nativa

Para exibir ranking:

- Cena custom: `Assets/Scenes/ContiGoGpgsRanking.unity`
- Script: `Assets/Scripts/PlayServices/ContiGoGpgsRankingSceneUI.cs`

E também é possível abrir a UI nativa:

- `Social.ShowLeaderboardUI()`

## 9) Android resources: **não usar** `Assets/Plugins/Android/res`

Em versões recentes do Unity, isto quebra o build com:

- `OBSOLETE - Providing Android resources in Assets/Plugins/Android/res was removed`

Se precisar de `games-ids.xml`, coloque em:

- **Android Library** (`.androidlib`) com `res/values/...`

Neste projeto:

- `Assets/Plugins/Android/GooglePlayGamesManifest.androidlib/res/values/games-ids.xml`

E certifique-se que **não existe** nenhuma pasta:

- `Assets/Plugins/Android/res` (nem vazia, nem com `.meta`)

## 10) Checklist rápido (quando der problema)

- **Offline no jogo**
  - [ ] Email está em **Play Games Services → Testadores**
  - [ ] App instalada via **Play Store** (track interno/fechado), não APK local
  - [ ] SHA‑1 do **App signing certificate** cadastrado no Cliente OAuth Android do PGS
  - [ ] Config do Play Games Services está **publicada**

- **Leaderboard not found**
  - [ ] ID `CgkI...` correto (ideal: vindo de `GPGSIds`)
  - [ ] Leaderboard está **Publicada**
  - [ ] Provider GPGS foi ativado (`PlayGamesPlatform.Activate()` no Android)

## Referências

- Documentação: “Ajudante do Play Games (Sidekick)” é **opcional** e não é requisito para leaderboards.

