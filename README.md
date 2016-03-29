# Pathfinder Csv To Wiki Converter

Ce programme permet de convertir les onglets du [fichier tableau google des équipements Pathfinder](https://docs.google.com/spreadsheets/d/1vl-MbogN6skGg7qnP83qX55zPVXb1KlzZLJo8ahgkqY/edit#gid=1568769969) contenant des données Pathfinder en un tableau mise en forme prêt à être utilisé sur le Wiki.

# Installation

Ce programme nécessite d'installer tout d'abord l'outil [dotnet](http://dotnet.github.io/getting-started/) pour votre système d'exploitation.

Ensuite vous devez télécharger le contenu du projet.
Vous pouvez par exmeple utiliser le bouton "Download ZIP" situé en haut à droite de cette page, puis décompresser les fichiers dans un dossier de votre choix.
Si vous devez mettre à jour le programme, il vous suffit de retélécharger le zip et écraser votre ancienne installation.

Une fois l'outil dotnet installé et le projet décompressé, il faut lancer la commande `dotnet restore` depuis le dossier du projet pour installer les librairies nécessaires.
Sous windows, vous pouvez simplement exécuter le script `installation.bat`.

Cette manipulation n'est à faire qu'une seule fois.

# Conversion d'un tableau

Il suffit de lancer la commande `dotnet run [param]` en remplaçant param par le nom de l'onglet à convertir.
Exemple : pour convenir l'onglet des armures, il suffit de lancer `dotnet run armures`.

Sous windows, vous pouvez simplement exécuter le script `run-param.bat` où `param` correspond au nom d'un onglet à convertir.
Exemple : `run-armures.bat` pour convertir l'onglet des armures.

Si tout se passe bien, le programme génèrera un fichier `.txt` correspondant au résultat de la conversion.

# Insertion sur le wiki

Une fois la conversion terminée, il vous faut modifier la page du wiki.

Retrouvez la page correspondante dans le wiki et faites Modifier.

Le contenu du fichier texte est à insérer entre les deux balises `<!-- Début partie générée -->` et `<!-- Fin partie générée -->`.

Par exemple :

    <!-- Début partie générée -->
    ... Contenu du fichier texte ...
    <!-- Fin partie générée -->

Points à prendre en compte :

* Il faut bien veiller à conserver le retour à la ligne entre les balises début et fin.
* Il ne faut jamais remplacer tout le contenu de la page
* Il faut supprimer l'ancien texte entre les balises avant de mettre le nouveau