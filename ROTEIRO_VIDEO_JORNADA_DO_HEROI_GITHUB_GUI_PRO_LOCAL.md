# Roteiro (Storytelling — Jornada do Herói) — “Não mandar o GUI Pro inteiro para o GitHub”

Objetivo do vídeo: contar **por que** versionar um pacote comercial inteiro (Layer Lab / GUI Pro) no Git é uma armadilha, e como a solução foi **manter o asset só na máquina local**, ignorar no Git e **documentar nome e versão no README** — em arco de Jornada do Herói, acessível a indies e pequenas equipas.

Formato sugerido: **4–7 min** + opcional **Short (30–45 s)**  
Estilo: **ecrã + voz** (terminal, Unity, GitHub, README)  
Público: **devs Unity / indies** que compram assets na Asset Store

---

## Pacote YouTube (copiar para a plataforma)

### Títulos (escolher um)
- “Parei de empurrar **134 MB de GUI** para o GitHub — e o projeto respirou”
- “Asset Store no repo? A jornada de **não** versionar o GUI Pro (Layer Lab)”
- “O ‘commit’ que nunca devia existir: pacote comercial e GitHub”

### Gancho (primeiros 25–35 s)
**Voz:**  
“Se você já tentou dar `git commit` e o Git ‘trava’, ou abriu um PR com **milhares de ficheiros**… talvez o culpado não seja o seu código. Talvez seja um **pacote de UI comprado** que entrou no repositório sem querer. Hoje é a história de como a gente **devolveu o pacote para o lugar certo**: na sua máquina — e deixou no Git só o que é **nosso**.”

### Descrição (rascunho)
```
Jornada do Herói (dev): o problema de versionar GUI comercial (Layer Lab) no Git, limites de repo, licenciamento e onboarding da equipa. Solução: .gitignore da pasta, remover do índice (git rm --cached), README com nome exato do asset na Asset Store e versão de referência.

Asset referenciado no projeto: GUI Pro - Casual Game (Layer Lab). Não é tutorial legal; é fluxo de trabalho.

#gamedev #unity #git #github #assetstore #indiedev #layerlab
```

### Capítulos (ajustar tempos após edição)
```
0:00 Mundo comum — repo “com tudo dentro”
0:35 Chamado — o commit que não cabe na cabeça
1:10 Recusa — “mas se eu ignorar, quebra na outra máquina?”
1:45 Mentor — o que o Git deve guardar (e o que não deve)
2:25 Limiar — .gitignore e tirar do índice
3:20 Caverna — GitHub, tamanho e ruído
4:05 Provação — README como “mapa do tesouro”
4:45 Recompensa — clone limpo + import local
5:20 Elixir — CTA
```

### Miniatura (texto)
- “GUI PRO” + riscado ou “FORA DO GIT”
- “README = versão”

---

## Premissa (1 frase)
“**Repositório não é backup de Asset Store** — é o lugar do *seu* jogo; o pacote comprado mora na Unity, com licença e import.”

---

## Jornada do Herói (estrutura + falas)

### 1) Mundo comum (0:00–0:35)
**Ecrã**: Unity com pasta `Assets/Layer Lab/...` cheia de prefabs e PNG; projeto a compilar.  
**Voz:**  
“No dia a dia, a Unity é confortável: você importa um pacote lindo, arrasta prefabs, o jogo ganha cara de produto. Tudo parece **um projeto só**. Só que para o Git… isso é um mundo comum que esconde um problema.”

### 2) Chamado à aventura — o problema (0:35–1:10)
**Ecrã**: `git status` enorme, ou lista de ficheiros staged; ou erro/lentidão ao commitar.  
**Voz:**  
“O chamado chega como uma notificação silenciosa: **milhares de alterações**. Ou um commit que demora, ou um repositório que inchou. E você percebe: não foi ‘só um script’ — foi o **pacote inteiro** do GUI Pro junto.”

### 3) Recusa do chamado — medos reais (1:10–1:45)
**Ecrã**: face de dúvida; pergunta na tela: “E se o colega clonar?”  
**Voz:**  
“A recusa é instintiva: ‘Se eu tirar isso do Git, **alguém vai abrir o projeto e vai faltar UI**’. Exato — e isso é o ponto. O medo certo não é o Git; é **onboarding sem mapa**. A solução não é empurrar o pacote; é **processo**.”

### 4) Encontro com o mentor — três verdades (1:45–2:25)
**Ecrã**: slide simples com 3 bullets.  
**Voz:**  
“Três verdades que viram mentor:  
**Um** — GitHub não é CDN de asset comercial.  
**Dois** — diff e histórico de PNG/prefab em massa viram ruído.  
**Três** — licença e compra são por conta; o repo público ou partilhado **não substitui** a Asset Store.”

### 5) Travessia do primeiro limiar — cortar o cordão (2:25–3:15)
**Ecrã**: `.gitignore` com `Assets/Layer Lab/` e `Assets/Layer Lab.meta`; terminal com `git rm -r --cached "Assets/Layer Lab"` (ou equivalente).  
**Voz:**  
“Aqui é o limiar: você diz pro Git — **para de rastrear esta pasta**. O `.gitignore` impede o retorno acidental. O `rm --cached` tira do **índice** sem apagar os ficheiros do disco. Na Unity, **continua tudo a funcionar na tua máquina**.”

*Nota de honestidade na voz:* “Quem já commitou o pacote no histórico antigo pode precisar de estratégia mais forte; hoje o foco é **daqui para a frente**.”

### 6) Provações — o ‘vilão’ GitHub (3:15–4:05)
**Ecrã**: página do produto na Asset Store (nome do asset) ou tamanho do package; ou PR ilegível.  
**Voz:**  
“O vilão não é o Git — é a **escala**. Repositório inchado, revisão impossível, clones lentos, e aquela sensação de que o importante (o teu código) desaparece no meio de **centenas de texturas**.”

### 7) Aproximação da caverna — o README certo (4:05–4:45)
**Ecrã**: `README.md` com tabela: fornecedor, **nome exato** na loja, link, **versão** (ex.: 4.1.2), caminho local típico após import.  
**Voz:**  
“A caverna escura vira corredor iluminado quando o README vira **mapa**: nome exato do produto (**GUI Pro - Casual Game**), onde baixar, qual versão a equipa assume. Não é romance — é **contrato interno**: cada máquina importa o que comprou.”

### 8) Recompensa — o clone ‘limpo’ (4:45–5:15)
**Ecrã**: clone fresco; Unity abre; UI quebrada até importar — depois import e tudo encaixa.  
**Voz:**  
“A recompensa é um repo que **respira**: commits sobre jogabilidade, não sobre 200 PNGs. E o momento ‘aha’ da equipa: ‘ah, preciso importar o pacote’ — **sim**, e agora isso está documentado.”

### 9) Caminho de volta — CI e futuro (5:15–5:40)
**Voz:**  
“No caminho de volta, você alinha CI: build numa máquina que tem o asset, ou pipeline que não assume pasta ignorada. Não precisa ser perfeito no primeiro vídeo — precisa ser **consciente**.”

### 10) Ressurreição — o teste final (5:40–5:55)
**Voz:**  
“Teste final: novo clone, README seguido, import feito, cena de Gameplay abre sem referências mortas críticas. Se passou, você renasceu como maintainer.”

### 11) Retorno com o elixir — CTA (5:55–6:30)
**Voz:**  
“O elixir é simples: **separa o que é teu do que é loja**. Se isso te poupou um fim de semana de Git, comenta **‘asset’** — e diz qual pacote te dá mais dor de cabeça no versionamento.”

---

## Short (30–45 s) — versão condensada
**Voz:**  
“Importei GUI Pro e o Git virou um monstro — milhares de ficheiros no commit. A solução foi óbvia depois que doeu: **ignorar a pasta**, tirar do índice, e deixar no README o **nome e a versão** do asset. Repo leve; Unity feliz na minha máquina; equipa com mapa. Repositório não é Asset Store.”

---

## B-roll / lista de capturas
- `git status` antes (pesado) vs depois (só código/artes tuas).
- `.gitignore` destacando `Assets/Layer Lab/`.
- Comando `git rm -r --cached` (sem mostrar tokens/secrets).
- README com nome **GUI Pro - Casual Game** e versão.
- Unity: pasta local a existir; Project window.
- Opcional: Asset Store — página do produto (sem mostrar dados pessoais).

---

## O que não prometer no vídeo
- Não dizer que isto “resolve licenças” por si — só **fluxo** e **higiene de repo**.
- Não garantir remoção de histórico antigo sem ferramentas/fluxo específico (filter-repo, etc.).
