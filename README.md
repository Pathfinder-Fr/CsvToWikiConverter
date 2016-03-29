# Pathfinder Csv To Wiki Converter

Ce programme permet de convertir un fichier CSV contenant des données Pathfinder en un tableau mise en forme prêt à être utilisé sur le Wiki.

# Installation

Ce programme nécessite d'installer tout d'abord l'outil [dotnet](http://dotnet.github.io/getting-started/).

Une fois l'outil installé, il suffit de lancer le script d'installation `installation.bat` pour restaurer les fichiers nécessaires.
Pour les utilisateurs sous mac ou linux, il suffit de lancer la commande `dotnet restore` depuis le dossier du programme.

Une fois le script d'installation terminé, le programme est disponible pour convertir les fichiers.

# Conversion d'un tableau

Il suffit de lancer la commande `dotnet run [param]` en remplaçant param par le nom de l'onglet à convertir.

Exemple : pour convenir l'onglet des armures, il suffit de lancer `dotnet run armures`.