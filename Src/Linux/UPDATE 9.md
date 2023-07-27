UPDATE 9 

Procédure de mise à hour jours version Drupal 9.1

# Nettoyage : suppression de ce qui gêne.

On suit les indiactions de field_encrypt et on enlève les modules non supportées.

```
systemctl restart apache2
drush updb
```

## Déchiffrer les champs, field_encrypt ne permet pas l'upgrade.
```
# drush php
$enc=[];
$enc['field_bic']= 'node'; 	
$enc['field_da_organization_number']= 'node'; 	
$enc['field_da_rum']= 'node'; 	
$enc['field_da_rum_prelevement']= 'node'; 	
$enc['field_da_ss_number']= 'node'; 	
$enc['field_iban']= 'node'; 	
$enc['field_qs_affection']= 'node'; 	
$enc['field_qs_arret']= 'node'; 	
$enc['field_qs_examen']= 'node'; 	
$enc['field_qs_future']= 'node'; 	
$enc['field_qs_height']= 'node'; 	
$enc['field_qs_hospitalisation']= 'node'; 	
$enc['field_qs_invalidite']= 'node'; 	
$enc['field_qs_traitement']= 'node'; 	
$enc['field_qs_virus']= 'node'; 	
$enc['field_qs_weight']= 'node';

$storage = \Drupal::entityTypeManager()->getStorage('field_storage_config');

foreach ($enc as $f => $n) {$field_storage_config = $storage->load('node.'.$f); $field_storage_config->unsetThirdPartySetting('field_encrypt', 'encrypt'); $field_storage_config->unsetThirdPartySetting('field_encrypt', 'properties');  $field_storage_config->unsetThirdPartySetting('field_encrypt', 'encryption_profile'); $field_storage_config->save();};
exit;
```

## Voir un peu le nombre de fichiers référencés dans Drupal, il me semble qu'on a des fichiers sans lien.

Ça peut permettre de voir aussi qu'un hook ne supprimerait pas des trucs, après la maj...
```
# drush php
$f = \Drupal::entityTypeManager()->getStorage('file');
$fs = \Drupal::service('file_system');

$files = $f->loadMultiple();
echo 'Il y a ' . count($files) . ' fichiers référencés dans Drupal';
exit;
```

## Nettoyage des modules

On vide les queues avant.

```
drush queue:run cron_encrypted_field_update ;
drush php ;
$storage = Drupal::entityTypeManager ()->getStorage('encrypted_field_value');
$storage->delete($storage->loadMultiple());
```

```
# déchiffrement des champs en batch, vérifier /admin/config/system/field-encrypt/field-overview ;
drush cron ;
drush pmu conditional_fields externalauth field_encrypt field_layout file_entity flag menu_trail_by_path session_limit upgrade_status wysiwyg_template wysiwyg_template_core ; 
drush theme:uninstall bartik ;
```

# Passage en D9

```
git checkout maj/d9_suite
COMPOSER_MEMORY_LIMIT=-1 composer  install
drush updb
```

Remettre des modules : layout qui doit remplacer le vieux, field_encryt à reconfigurer, prae_affiliations qui est utilisé ailleurs (erreur 500 si pas là)
```
drush en prae_affiliations layout_builder field_encrypt
drush cr
```

# @todo

- reconfigurer les champs chiffrés.
- utiliser media (pour le market)
- voir les histoires de layout (pour le market)
- voir comment gérer  *session_limit* (malgré les correctifs automatisés, l'auteur ne fait rien)


Et **tester encore** ! 
