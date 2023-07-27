# Dépot de modules drupal custom

## Prérequis
- Avoir une clé API (Créée en éditant son profil par exemple)
- Avoir un projet utilisant composer
- le type de projet doit être **drupal-custom-module** pour aller dans **web/modules/custom**
- pour ajouter une dépendance vers un autre module : 
```
composer config repositories.drupal composer https://packages.drupal.org/8
composer require drupal/XXX 
```
et ne pas pousser le composer.lock ni le vendor, mais seulement le .json.

### Example de composer.json :
```
{
    "name": "sima/rgpd",
    "description": "Module pour permettre d'insérer des scripts dans différentes catégories RGPD de cookies.",
    "type": "drupal-custom-module",
    "authors": [
        {
            "name": "Bruno",
            "email": "b.mathieu@giesima.fr"
        }
    ],
    "repositories": {
        "drupal": {
            "type": "composer",
            "url": "https://packages.drupal.org/8"
        }
    },
    "require": {
        "drupal/eu_cookie_compliance": "^1.14"
    }
}
```

## Publication
Aprés avoir poussé un tag, un appel API le fait apparaître dans la registry.
```
 git tag v1.0.0
 gt push --tag
 curl --data tag=v1.0.0 "https://__token__:<TOKEN_API>@git.giesima.fr/api/v4/projects/<ID_PROJET>/packages/composer"
```

## Récupération des modules custom dans un Drupal
- Créer un projet Drupal avec composer (exemple: praeconis)
```
composer create-project drupal/recommended-project praeconis
cd praeconis
composer require drush/drush
drush si --db-url=sqlite://sites/praeconis/files/.ht.sqlite
```
### Créer le fichier auth.json pour le domaine giesima.fr :
- Ajouter le domaine 
```
composer config gitlab-domains git.giesima.fr
```
- créer un fichier d'authentification auth.json
```
composer config gitlab-token.git.giesima.fr <TOKEN_ACCES_REPOSITORY>
```
### Ajouter le dépôt :
```
Ajouter un group gitlab comme dépôt :
```
composer config repositories.sima composer https://git.giesima.fr/api/v4/group/<group_id>/-/packages/composer/
```
Et tester :
```
- rechercher module pour vérifier
``` 
composer search <MODULE>
```

## Mises à jour et retour arrière

Mise à jour / installation classiques :
```
composer update # tout ce qui n'est pas 'boqué' dans le composer.json
composer require <MODULE>:<VERSION> # mettre une version spécifique d'un module
```
Si un module altère la base de données, attention la rétrogradation n'est pas prévue.
C'est typiquement la chose à bien tester. Ça touche les données. 
``` 
drush updb
```
Et su un module touche une route ou une implémentation de hook :
```
drush cr
```

## Avantages par rapport au déploiement git
- Déploiement modulaire
- séparation des choses : modules pour les dév, thèmes pour le market, sites pour les admin sys.
- retour arrière plus aisé (sauf si modif en bdd, mais on sait quoi regarder : le code du module)


