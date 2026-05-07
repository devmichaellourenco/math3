# Roteiro — “Base pronta + IA”: como evoluímos o Math Rush com código e prompts direcionados

**Jogo:** Math Rush: Desafio da Esfinge  
**Público:** intermediário (Unity / mobile / dev curioso)  
**Formato sugerido:** 3–5 min (YouTube) + 1 Short 30s  
**Tom:** honesto e técnico — IA como **acelerador**, não como substituto do design

---

## Promessa do vídeo (1 frase)

“Vou mostrar **como refinamos um jogo já jogável** com **código**, **prompts bem direcionados** e **novas cenas/sistemas** — sem fingir magia.”

---

## Versão principal (≈3:30–4:30) — narração + o que mostrar na tela

### 0:00–0:12 — Hook (tela: gameplay forte + corte pra lista de commits/arquivos/Unity)
**Voz**  
“Você não precisa começar do zero pra evoluir rápido. O **Math Rush** já tinha uma **base pronta** — e dali pra frente a gente **aperfeiçoou com IA**, mas sempre **amarrando no código** e com **prompts bem direcionados**.”

**Tela**  
- 3 cortes rápidos: Gameplay → nova cena (menu/missões/etc.) → Unity com script aberto.

---

### 0:12–0:45 — Contexto (o que já existia)
**Voz**  
“Antes eu já conseguia jogar: loop principal, gameplay 2D funcionando, fluxo básico. Ou seja: **protótipo virou produto em potência**, mas faltava camadas de jogo ‘de verdade’: navegação, progressão, conforto visual, sistemas novos…”

**Tela**  
- Mostrar Home / seleção / gameplay (o mínimo pra provar “base pronta”).

**Frase-chave (overlay opcional)**  
- “Base jogável primeiro. Refino depois.”

---

### 0:45–1:30 — O que mudou (novas cenas + novos sistemas)
**Voz**  
“O que veio depois foi **incremental**: criamos **novas cenas**, expandimos UI, adicionamos/ajustamos sistemas — sempre com foco em **retorno de investimento** (o jogador sente na hora).  
IA ajudou muito em **acelerar implementação**, mas a decisão de ‘o que fazer’ continua sendo **design + arquitetura**.”

**Tela**  
- Mostrar 2–3 exemplos concretos do seu projeto (substitua pelos nomes reais das cenas):
  - cena de missões / coleção / ranking / level select (o que vocês adicionaram)
  - um modal/fluxo novo (ex.: game over / HUD / feedback)

**Frase-chave**  
- “Cena nova não é ‘arte’: é **fluxo + responsabilidade**.”

---

### 1:30–2:50 — Como a IA entrou (processo real, sem romantizar)
**Voz**  
“Quando eu digo IA, eu não estou falando de ‘gerar o jogo inteiro’. Eu falo de **assistente de implementação**:  
ela ajuda quando eu já defini **objetivo**, **restrições**, **onde isso entra no Unity**, e como isso conversa com o código existente.”

**Explique em 4 bullets (mostrar na tela)**  
1) **Objetivo**: “implementar X”  
2) **Restrições**: plataforma, UI Toolkit/UGUI, performance, estilo do projeto  
3) **Contexto**: arquivos/cenas relevantes e padrões existentes  
4) **Critério de pronto**: comportamento observável no jogo

**Voz (continuação)**  
“O segredo não é o prompt bonito. É o prompt **operacional**: ele precisa dizer **o que entrega**, **como validar**, e **o que não pode quebrar**.”

**Tela**  
- Mostrar um exemplo genérico de prompt (pode mascarar trechos sensíveis), destacando as partes: objetivo / restrições / arquivos / definição de pronto.

---

### 2:50–3:40 — Tudo via código (o que a IA NÃO resolve sozinha)
**Voz**  
“No fim, o que vai pra loja é **C#**, cena, prefab, build.  
IA pode sugerir, mas quem garante consistência é você: **revisão**, **integração**, **teste no device**, e **ajuste fino**.”

**Tela**  
- Abrir um script e apontar 2 coisas:
  - onde entrou a regra nova  
  - como isso se conecta com o restante (sem aprofundar demais)

**Frase-chave**  
- “IA acelera linhas; arquitetura evita dívida técnica.”

---

### 3:40–4:10 — Honestidade / limites (curto, fortalece credibilidade)
**Voz**  
“E sim: IA erra, inventa detalhe, propõe API que não existe… por isso eu trato tudo como **rascunho técnico** até passar no teste do Unity e do celular.”

---

### 4:10–4:40 — Gancho da série + CTA
**Voz**  
“E é exatamente isso que vamos mostrar na série: **como o Math Rush foi desenvolvido** — não só ideia, mas **decisões**, **código**, **UI**, **integrações**, e como a IA entra como ferramenta no meio do caminho.  
Comenta **‘SERIE’** se você quer que eu solte a playlist e os próximos temas.”

**Tela**  
- Texto: “Próximos episódios: arquitetura • UI • integrações • build”

---

## Short (≈30s) — versão “gancho + prova”

**Voz**  
“Math Rush já tinha **base jogável**. A evolução veio quando a gente adicionou **novas cenas** e **novos sistemas** — com **código** e **prompts direcionados** pra IA acelerar a implementação.  
IA não substitui design: ela **reduz atrito**. Quer ver o processo completo? segue a série.”

**Tela**  
- Antes: gameplay antigo / base  
- Depois: cena nova + sistema novo (2 segundos cada)

---

## Checklist do que gravar (B-roll)

- [ ] Gameplay “base” (10–20s)  
- [ ] 2–3 cenas novas (cada uma 5–10s)  
- [ ] 1 exemplo de “sistema” visível pro jogador (HUD/modal/missões/ranking)  
- [ ] Unity: Project window mostrando scripts/cenas (rápido)  
- [ ] Editor de código com 1 trecho “ok” (sem expor segredos/credenciais)  
- [ ] Opcional: trecho de prompt (sem dados sensíveis)

---

## Textos para overlay (rápidos)

- “Base pronta → refinamento”  
- “Novas cenas / novos sistemas”  
- “Prompt = objetivo + restrições + validação”  
- “Ship = código + teste no device”

---

## Observação importante (pra você não se complicar)

Se você for mostrar prompts/código, **remova** nomes internos do repositório se quiser manter o vídeo mais “público”, e **nunca** mostre arquivos de credenciais/keystore.
