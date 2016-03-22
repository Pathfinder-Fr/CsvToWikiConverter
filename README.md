# Pathfinder Csv To Wiki Converter

Ce programme permet de convertir un fichier CSV contenant des données Pathfinder en un tableau mise en forme prêt à être utilisé sur le Wiki.

# Installation

Ce programme nécessite d'installer tout d'abord l'outil [dotnet](http://dotnet.github.io/getting-started/).

Une fois l'outil installé, il suffit de lancer le script d'installation `installation.bat` pour restaurer les fichiers nécessaires.
Pour les utilisateurs sous mac ou linux, il suffit de lancer la commande `dotnet restore` depuis le dossier du programme.

Une fois le script d'installation terminé, le programme est disponible pour convertir les fichiers.

# Conversion d'un tableau

La conversion nécessite tout d'abord de télécharger le tableau au format tsv.

Depuis le tableau sous Google Drive, sélectionnez tout d'abord l'onglet que vous souhaitez exporter.

Puis rendez-vous dans le menu `Fichier`, `Télécharger au format`, puis `Valeurs séparées par des tabulations (.tsv, feuille active)`.

Le site devrait vous faire télécharger un fichier "Equipement Pathfinder - Armures".

Renommez-le en "Armures" puis déplacez le dans le dossier du programme installé.

Lancez ensuite la commande "run-armures.bat", et si tout se passe bien un fichier "Armures.txt" devrait être généré avec le contenu wiki à coller.