# Roteiro (Storytelling — Jornada do Herói) — “Relatório de erros no Game Over” (Math Rush)

Objetivo do vídeo: explicar a melhoria do **relatório de erros** no final da partida (Game Over), por que ela era necessária e como a solução foi construída com UI correta (sem sobreposição), deixando o jogo mais justo e compreensível.

Formato sugerido: **3–6 min** (YouTube) + **1 Short (30s)**  
Estilo: **captura de tela + voz** (sem câmera)  
Público: **intermediário** (Unity / mobile / UI)

---

## Premissa (1 frase)
“Quando o jogador erra, ele precisa entender **por quê** — e o Game Over é o lugar perfeito para mostrar isso de um jeito limpo e bonito.”

---

## Jornada do Herói (estrutura + falas)

### 1) Mundo comum (0:00–0:20)
**O que mostrar na tela**: gameplay normal, HUD, jogador tocando nas células.  
**Voz**:
“No Math Rush, o jogo é rápido e a pressão é constante. Você toma decisões, erra, acerta… e quando a partida termina, o jogador quer uma coisa simples: **entender o que aconteceu**.”

### 2) Chamado à aventura — o problema (0:20–0:45)
**Tela**: Game Over antigo (sem relatório), ou Game Over com feedback insuficiente.  
**Voz**:
“Só que existia um buraco: quando o jogador errava, ele tinha som e um flash… mas no final, não tinha um resumo claro. E aí nasce a pergunta:  
‘Eu errei por quê? Quais eram os valores? O que eu escolhi?’”

### 3) Recusa do chamado — o “não é tão grave” (0:45–1:05)
**Tela**: mostra o Game Over simples com apenas pontos.  
**Voz**:
“A primeira reação é pensar: ‘ah, não precisa… o jogador já entendeu na hora’.  
Mas na prática isso vira frustração — principalmente em partidas rápidas, onde você erra três vezes e nem sabe quais decisões te derrubaram.”

### 4) Encontro com o mentor — a regra do design (1:05–1:30)
**Tela**: slide/overlay “Regra: feedback precisa explicar a falha”.  
**Voz**:
“A regra que guiou a solução foi: **feedback bom explica, não só informa**.  
E como só existem 3 erros possíveis, dá pra fazer um relatório curto e direto.”

### 5) Travessia do primeiro limiar — primeira tentativa (1:30–2:05)
**Tela**: mostra o relatório em texto ‘ERROS 1: valores…’.  
**Voz**:
“A primeira versão foi o caminho mais óbvio: montar uma string e colocar no texto do Game Over.  
Funcionou… mas ficou feio, ‘uma coisa em cima da outra’ e difícil de ler.”

### 6) Testes, aliados e inimigos — o verdadeiro vilão (2:05–2:55)
**Tela**: UI quebrando/itens sobrepondo header.  
**Voz**:
“E aí apareceu o inimigo real: **layout**.  
Quando você tenta empilhar header + lista numa UI dinâmica, se o componente pai não controla altura dos filhos… você ganha sobreposição, truncamento e caos visual.”

**Punchline técnica**:
“O bug não era o dado. Não era o texto. Era o **comportamento do layout**.”

### 7) Aproximação da caverna oculta — a solução certa (2:55–3:40)
**Tela**: Unity hierarchy do `GameOverPopupCasual/Content` + highlight em `Control Child Size (Height)`.  
**Voz**:
“A virada foi tratar o relatório como **componente de UI**, não como texto.  
E garantir que o container do popup respeitasse a altura real dos filhos: marcando **Control Child Size: Height** no `Content`.”

### 8) Provação — refazer como ‘componentes’ (3:40–4:30)
**Tela**: aparece o bloco “ERROS” com frame roxo + tabela (valores | sua escolha).  
**Voz**:
“Aí sim: dois componentes claros.  
Um **header** fixo com ‘ERROS’ e as colunas ‘valores’ e ‘sua escolha’.  
E uma **lista** com até 3 linhas — cada uma encapsulada com background, no estilo Layer Lab.  
O resultado é um Game Over que parece de jogo publicado, não protótipo.”

### 9) Recompensa — antes/depois (4:30–5:10)
**Tela**: comparativo antes/depois (lado a lado ou sequência rápida).  
**Voz**:
“Antes: o jogador só via pontos — e saía sem entender onde falhou.  
Depois: ele vê exatamente quais foram os valores do turno e qual escolha fez.  
Isso muda a sensação de justiça e acelera aprendizado.”

### 10) Caminho de volta — impacto no projeto (5:10–5:35)
**Tela**: código rápido (registro dos erros + render da lista).  
**Voz**:
“E o melhor: isso foi feito de forma simples.  
Registramos até 3 tentativas erradas (valores A-B-C + escolha) e renderizamos no Game Over.  
Sem inventar sistema gigante.”

### 11) Ressurreição — validação final (5:35–5:55)
**Tela**: simula 2 erros, game over, lista aparece bonita, restart, lista limpa.  
**Voz**:
“O teste final é: erra, aparece; reinicia, limpa; volta pra home, não leva lixo junto.  
Agora o feedback fecha o ciclo da partida.”

### 12) Retorno com o elixir — CTA (5:55–6:10)
**Tela**: gameplay + texto “próximo: X”.  
**Voz**:
“Se você quer ver mais melhorias assim — pequenas no código, grandes no feeling — comenta **‘UI’** que eu mostro a próxima evolução do Math Rush.”

---

## Short (30s) — Jornada do herói condensada
**Voz**:
“No Math Rush, o jogador errava… e no fim só via pontos. Isso gerava frustração: ‘onde eu errei?’  
A solução foi criar um **relatório de erros** no Game Over: valores do turno e a escolha do jogador, com UI bonita e sem sobreposição.  
Pequena mudança, grande diferença no game feel.”

---

## Lista de cenas (B-roll) para gravar
- Gameplay com 1 erro (mostra os valores/dados e a escolha).
- Game Over antigo (sem relatório) / ou versão bugada sobrepondo.
- Game Over final com bloco roxo + tabela.
- Unity `Content` mostrando `Control Child Size: Height`.
- Restart limpando o relatório.

