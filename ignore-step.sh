#!/bin/bash

allowedChanges=("package.json" "docs/" "ignore-step.sh")

for i in "${allowedChanges[@]}"; do
  flag=$(git diff HEAD^ HEAD "$i")
  if [ "$flag" != '' ]; then
        exit 1
  fi
done

exit 0