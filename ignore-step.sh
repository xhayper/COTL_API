#!/bin/bash

allowedChanges=("astro.config.ts" "package.json" "pnpm-lock.yml" "src/" "public/" "tsconfig.json" "ignore-step.sh")

for i in "${allowedChanges[@]}"; do
  flag=$(git diff HEAD^ HEAD "$i")
  if [[ "$flag" != '' ]]; then
        exit 1
  fi
done

exit 0
