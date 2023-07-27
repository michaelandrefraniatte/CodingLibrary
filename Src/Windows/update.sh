#!/bin/sh
echo Automatically update current branch from distant repository:
CURRENT_BRANCH=$(git rev-parse --abbrev-ref HEAD)
git checkout master
git pull
echo Updating branch master from distant repository
git checkout $CURRENT_BRANCH
git merge master
echo Merging current branch from branch master
echo Done
