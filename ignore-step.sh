#!/bin/bash

allowedChanges=("astro.config.ts" "package.json" "src/" "public/" ".yarnrc.yml" ".yarn/" "tsconfig.json")

for i in "${allowedChanges[@]}"; do
  flag=$(git diff HEAD^ HEAD "$i")
  if [ "$flag" != '' ]; then
        exit 1
  fi
done

exit 0