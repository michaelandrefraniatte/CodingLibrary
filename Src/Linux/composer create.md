# Comment sont crées les archives ?

Récupération depuis le Git,

Création d'un nouveau projet pour chaque site institutionnel :

```
composer create-project drupal/recommended-project <SITE>
```

Installation des composants drupal utilisés dans le nouveau projet à jour, avec adaptation du composer (stability requirement ou demander à Romeu pour MMC pour drupal/linkit ou drupal/sitemap, les trucs peu maintenus). Copie des web/\*/custom des anciens sites vers les neufs. Copie des web/sites/<SITE>/files des anciens vers les nouveaux. Copie des web/libraries aussi (qui nécessiteraient certainement des mises à jour également).

Installation en dev de php_codesniffer, phpstan, drush, drupal/coder. Drush j'hésite : en dev ou en prod ?\
Si on met en dev, un `composer install --no-dev` installera le minimum, mais il faudrait passer par update.php pour la maj de la bdd.

Passage de `phpcbf --standard=Drupal` pour corriger mettre à jour le code, et vérification.

Test des sites locaux, suppression du multisite, retests.

Compactage :

```
for i in MMC MUTEST PREVALOIS SPRIMUT; do 
  pushd $i
  tar czf ../$i.tgz --exclude='web/sites/*/files' --exclude='web/*/contrib' --exclude='vendor' --exclude='.git' --exclude='web/core' .
  popd
done
```

# Les soucis

Si on supprime le multisite, les liens, en dur, dans le contenu, sont pourris : il faudrait les refaire.\
Normalement on insère un id de l'image, et pas l'url, mais bon... ça a été fait comme ça parfois.\
Une petite moulinette sur les body du contenu pourrait remettre ça propre.

## Mutest

Pour le cas Mutest, j'ai renommé le bartik en mutest. Je ne l'ai pas fait pour un autre, il faudrait sans doute le faire : le README de bartik dit de : soit utiliser bartik comme thème parent, soit le cloner sous un nouveau nom. La troisième voie, celle choisie, je ne pense pas qu'elle soit plus intelligente, enfin je ne vois pas de raison de procéder comme cela.

Renommer le thème fait merdouiller les blocs : un export de la config et un renommage de thème fait l'affaire. C'est pourquoi je mets la config exportée dans config/sync, en dehors du site comme préconisé (et ce n'est pas partout le cas).

Le contenu dépend du nom du thème !!! Les uuid des fichiers sont parfois vides... Ce serait à corriger.

## MMC 

Deux choses à expliquer au Market :

* Les largeur et hauteurs des images sont en dur : ce serait moeux de le faire en css (max-width: 100%).
* Les liens internes de menu, au lieu de pointer vers le noeud, pointent vers son url qui peut changer (pathauto...). 

# Le runtime

Pour faire tourner les 4 nouveaux sites plus les 4 anciens, j'ai utilisé nginx et des vhosts localement.\
Je branche les 4 nginx sur un pool de serveur php-fpm.

J'insère le dump de la base dans un mariadb local et je modifie le settings.php pour faire le lien dans les anciens (du git) et les nouveaux.

Dans les anciens, pour les paquets obsolète, je désactive les modules `drush pmu` (linkit, sitemap)

Une fois iso au niveau des modules, dans les nouveaux je lance `drush updb`

Et tests...