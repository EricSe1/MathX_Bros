# MathX-Bros

Ce jeu s'inspire de Mario Bros, mais en version aventure avec des défis mathématiques.

## Applications disponibles

Les versions pour Mac, Windows et Linux sont disponibles dans le répertoire `application`.

## Instructions pour accéder à l'éditeur Unity

- Assurez-vous d'utiliser une **version Unity 6000.0.34f1 ou ultérieure** et ouvrez le sous-dossier `MathX_Bros`.
- Vous y trouverez des répertoires d'assets tels que `skins`, `animations`, `script`, etc.
- Les scripts sont écrits en C# (.cs).
- Les scènes du jeu sont divisées en 4 parties :
  - Menu
  - Niveau 1
  - Niveau 2
  - Niveau 3

## Description du gameplay

### Objectif des niveaux

Chaque niveau commence avec un certain nombre de clés :
- Niveau 1 : 1 clé
- Chaque niveau suivant : une clé supplémentaire

L'objectif est de sortir par une porte verrouillée. Pour cela, vous devez trouver des clés cachées dans des coffres. Un indicateur en haut à gauche de l'écran affiche le nombre de clés obtenues.

### Fonctionnement des coffres

Chaque coffre déclenche 4 questions basées sur des concepts mathématiques (équations, expressions ou expressions complexes). 
- Une réponse correcte vous donne une clé.
- Une mauvaise réponse vous fait perdre un cœur.
- Si vous perdez 3 cœurs, le joueur meurt et un menu s'affiche pour recommencer ou retourner au menu principal. Cela bloque le niveau, et vous devrez recommencer depuis le niveau 1.

### Ennemis (PNJ)

Des ennemis PNJ sont présents dans les niveaux :
- Si un ennemi vous touche, vous mourrez instantanément, même si vous avez encore des cœurs.
- Pour éliminer un ennemi, sautez sur sa tête.