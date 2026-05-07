# Roteiro — Vídeo 1 (10 min) — Math Rush: Desafio da Esfinge (SEM CÂMERA)

Público: **intermediário (Unity / mobile)**  
Formato: **captura de tela + voz** (sem aparecer na câmera)  
Cenas: **Home → Seleção de Modo → Gameplay2D**

---

## Tema do episódio
**“Por dentro do Math Rush: Home → Seleção de Modo → Gameplay2D (e a organização que permite escalar)”**

---

## Títulos (3 opções)
- **Opção A**: “Por dentro do Math Rush: Home → Seleção de Modo → Gameplay2D (Unity Mobile)”
- **Opção B**: “Arquitetura prática em jogo mobile: Home → Modo → Gameplay (Unity)”
- **Opção C**: “Como organizar menus e gameplay pra escalar (exemplo real: Math Rush)”

---

## Thumbnail (sem rosto)
- Texto grande: **“HOME → GAMEPLAY”**
- Selo pequeno: **“Unity Mobile”**
- Visual: 3 mini prints (Home / Seleção de Modo / Gameplay2D) + setas entre eles

---

## Setup rápido (antes de gravar)
- Capturar **tela em 1080p** (ou 1440p) e exportar o vídeo final em 1080p.
- Ativar **cursor visível** e clique destacado (opcional).
- Separar 3 takes curtos para shorts:
  - **Take 1**: fluxo Home→Modo→Gameplay (10–15s)
  - **Take 2**: 3 regras (10–15s com texto na tela)
  - **Take 3**: “prova visual” (antes/depois de UI/feedback/modal — se tiver)

---

## Roteiro com narração (timecode)

### 0:00–0:15 — Hook (tela: gameplay rodando + corte rápido pra Home)
**Voz**:
“Você já tem um jogo rodando… mas o desafio real é **evoluir sem quebrar o fluxo**. Hoje eu vou te mostrar por dentro do **Math Rush: Desafio da Esfinge**: **Home → Seleção de Modo → Gameplay2D**, e a organização que eu uso pra isso escalar.”

**Tela**:
- 3 cortes rápidos: Gameplay2D → Home → Seleção de Modo → volta pro Gameplay2D (bem curto).

### 0:15–0:45 — Contexto (tela: Home)
**Voz**:
“O objetivo desse episódio não é ensinar o jogo em si. É mostrar **como eu separo responsabilidades** entre cenas e UI pra mexer no projeto com segurança, sem virar um emaranhado de `ifs`.”

**Tela**:
- Home aberta (no jogo ou no editor).

### 0:45–1:20 — Mapa do episódio (tela: uma lista simples no editor/Notion)
**Voz**:
“A gente vai passar por três cenas: **Home**, **Seleção de Modo** e **Gameplay2D**. No final eu deixo um checklist com **3 regras** que deixam o projeto mais escalável.”

**Tela**:
- Uma lista visual rápida (pode ser um overlay de texto).

### 1:20–3:10 — Cena 1: Home (tela: editor + play)
**Voz**:
“A Home tem uma missão: **entrada e navegação**. Regra número um aqui é: **Home não carrega lógica de gameplay**.  
Quanto menos regra na Home, menos risco de você quebrar gameplay quando mexe em UI.”

**Tela**:
- Mostre a hierarquia da Home (rápido).
- Mostre onde ficam botões/rotas (sem aprofundar em código).

### 3:10–5:10 — Cena 2: Seleção de Modo (tela: seleção de modo rodando)
**Voz**:
“Na Seleção de Modo eu trato ‘modo’ como **configuração**. Essa cena **decide parâmetros**: dificuldade, variações, o que muda no tabuleiro…  
E o Gameplay2D **só lê e executa**. Isso evita espalhar decisão pelo projeto inteiro.”

**Tela**:
- Mostre a UI de seleção.
- Mostre 1–2 exemplos de parâmetros (mesmo que seja conceitual).

### 5:10–8:10 — Cena 3: Gameplay2D (tela: gameplay rodando + editor)
**Voz**:
“No Gameplay2D eu separo três coisas: **loop do jogo**, **UI**, e o que é **integração/serviço**.  
Isso permite trocar HUD, modal de Game Over, feedback visual… sem reescrever regra do tabuleiro.”

**Tela**:
- Gameplay2D rodando.
- Abra a hierarquia e mostre rapidamente HUD / modal / tabuleiro (alto nível).
- Se tiver: insira um **antes/depois** de UI/feedback/modal (5–10s).

### 8:10–9:20 — As 3 regras (tela: overlay com bullets)
**Voz**:
“Pra resumir, três regras que eu sigo:
1) **Uma fonte de verdade** pra IDs e configurações.
2) **Cenas com responsabilidade única**: Home navega, Seleção configura, Gameplay executa.
3) **UI com padrões reutilizáveis**: consistência visual e helpers pra não duplicar trabalho.”

**Tela**:
- Texto grande com os 3 itens, aparecendo um a um.

### 9:20–10:00 — Fechamento + CTA (tela: gameplay + texto)
**Voz**:
“No episódio 2 eu vou mostrar como eu defini **pilares, escopo e anti-escopo** pra evoluir o jogo sem desperdiçar semanas.  
Comenta **‘ARQ’** se você quer o meu checklist de organização e me diz qual dessas três cenas você quer que eu destrinche primeiro.”

**Tela**:
- Gameplay2D rodando ao fundo + CTA em texto.

---

## Checklist de gravação (pra não esquecer)
- [ ] Captura do **fluxo**: Home → Modo → Gameplay2D (take curto)
- [ ] Home: hierarquia + navegação (alto nível)
- [ ] Seleção de modo: quais parâmetros define (conceitual + UI)
- [ ] Gameplay2D: loop + UI + (se possível) 1 prova de evolução antes/depois
- [ ] Overlay “3 regras” (texto na tela)
- [ ] Encerramento com CTA (“ARQ”)

---

## Shorts (2 recortes prontos — 30s)

### Short 1 — “Fluxo escalável”
**Voz**:
“Quer evoluir um jogo mobile sem quebrar tudo? Olha o fluxo do Math Rush: **Home → Seleção de Modo → Gameplay2D**.  
A regra é: Home **só navega**, Seleção **só configura**, Gameplay **só executa**. Isso elimina `ifs` espalhados e deixa o projeto escalável.”

**Tela**:
- Cortes rápidos mostrando as 3 cenas + setas/labels.

### Short 2 — “3 regras de organização”
**Voz**:
“Três regras que eu uso no Math Rush pra não virar caos:
1) **uma fonte de verdade**,
2) **responsabilidade única por cena**,
3) **UI com padrões reutilizáveis**.  
Se você fizer só isso, seu projeto já sobe de nível.”

**Tela**:
- Bullet points aparecendo um por um, com gameplay de fundo.

