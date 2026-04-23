# Google Play Games Services (Leaderboards) + Easy Save (offline)

Este documento descreve como **manter o ranking local (Easy Save / ES2)** e **adicionar sincronização com o Google Play Games Services** usando **Tabelas de Classificação (Leaderboards)**.

## Objetivo

- **Offline-first**: o ranking local continua funcionando sem internet.
- **Online**: quando o jogador estiver autenticado, o jogo **envia o melhor resultado** para o leaderboard do Google.
- **Sem perder dados**: o estado local continua sendo a “fonte de verdade” do jogo; o Google Play Games vira o “espelho social”.

## 1) Console (Google Play Console / Play Games Services)

- Criar o jogo no **Play Games Services** e vincular ao app.
- Criar pelo menos **1 Leaderboard** (ex.: “Melhor tempo”, “Maior pontuação”, etc.).
- Colocar o serviço em **Teste** (com testers) ou **Publicado**.
- Garantir **SHA‑1** do keystore correto (debug vs release) no OAuth/Play Games.

## 2) Unity: instalar e configurar o Play Games (GPGS)

- Instalar o **Google Play Games plugin for Unity**.
- Configurar o plugin com o **Client ID** e gerar/usar os IDs do serviço.
  - Normalmente o plugin gera uma classe com constantes, ex.: `GPGSIds.leaderboard_xxx`.
- Garantir que o build Android está usando o **mesmo package name** e assinatura que o Play Games espera.

## 3) Definir a métrica do leaderboard (o que é “ranking”)

O leaderboard recebe **um número**. Escolha uma métrica clara:

- **Maior é melhor**: pontos/score.
- **Menor é melhor (tempo)**:
  - enviar **tempo em milissegundos** como “score” (você pode nomear no console como “Menor tempo”)
  - ou enviar uma pontuação invertida (ex.: \(score = max(0, limite - tempo)\)) — só use se você tiver um limite bem definido.

Se o jogo tiver modos/dificuldades (2×2 / 4×4 / 6×6 / 8×8):
- **Recomendado**: 1 leaderboard **por modo** (ex.: Mestre 8×8).
- Alternativa: 1 leaderboard geral (perde granularidade).

## 4) Arquitetura no código (adapter)

Criar uma camada única para isolar o Play Games do resto do jogo:

- `PlayGamesAuthService`
  - `SignInSilent()`
  - `SignInInteractive()` (fallback)
  - `IsAuthenticated`
- `PlayGamesLeaderboardService`
  - `SubmitScore(leaderboardId, score)`
  - `ShowLeaderboardUI(leaderboardId)` (UI nativa)
  - (opcional) `LoadScores(...)` se quiser montar UI custom

O jogo chama essa camada; ela chama a API do GPGS.

## 5) Persistência híbrida: ES2 + Google

### 5.1 Chaves sugeridas no Easy Save

Guardar localmente:

- `bestScoreLocal_<mode>`: melhor resultado local (por modo, se aplicável)
- `bestScoreReported_<mode>`: último resultado que você **já reportou com sucesso** ao Google

Exemplo de `<mode>`:
- `iniciante`, `profissional`, `sabio`, `mestre`

### 5.2 Regra de sincronização (simples e robusta)

Leaderboard é “melhor de todos”. Então, na prática, você não precisa re-enviar tudo:

- Ao terminar uma partida:
  - atualiza `bestScoreLocal_<mode>` no ES2
  - se autenticado:
    - se `bestScoreLocal_<mode>` for “melhor” que `bestScoreReported_<mode>`, envia pro Google e atualiza `bestScoreReported_<mode>`
- Ao autenticar (startup / quando o jogador faz login):
  - compara `bestScoreLocal_<mode>` vs `bestScoreReported_<mode>`
  - se tiver diferença, faz `SubmitScore` e atualiza o `bestScoreReported_<mode>`

Isso mantém o offline intacto e sincroniza quando der.

## 6) Onde enviar (pontos de integração no ContiGo2D)

Pontos típicos:

- **Fim da partida / vitória**: quando você detecta que o tabuleiro foi concluído ou quando computa o resultado final
  - atualizar ES2 (já existe)
  - chamar `SubmitScore(...)` (novo)
- **Tela/botão “Ranking”**:
  - chamar `ShowLeaderboardUI(leaderboardId)` para abrir o ranking do Google (mais simples)
- **No boot (Home / controller 2D)**:
  - tentar `SignInSilent()` e, se autenticado, sincronizar `bestScoreLocal` → `bestScoreReported`

## 7) Receber informações do jogador (mostrar o ranking)

Opções:

- **UI nativa do Google (recomendado)**:
  - `ShowLeaderboardUI(leaderboardId)`
- **UI própria** (opcional):
  - carregar scores com API do plugin (depende da versão) e renderizar numa cena/scroll própria

## 8) Checklist de testes

- Testar com conta Google adicionada como **tester** no Play Games.
- Testar em device real:
  - offline: ranking local continua funcionando
  - online: autentica e envia score
  - reinstalação: local zera, Google mantém ranking (o que é esperado para leaderboard)
- Verificar se o score aparece na tabela correta.

## 9) Próximas decisões (para finalizar o mapeamento)

Para fechar a implementação sem ambiguidades, defina:

- O leaderboard é de **tempo** ou **pontos**?
- É **1 leaderboard geral** ou **1 por modo (2×2/4×4/6×6/8×8)**?
- Qual o **ID** do leaderboard no Unity (ex.: `GPGSIds.leaderboard_...`)?

