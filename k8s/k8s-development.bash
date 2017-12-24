#!/bin/bash

if [ -z "$1" ]
then
    echo "USAGE: $0 kubectl-cmd # e.g., create or delete"
    exit 2
fi

kubectl $1 --namespace=development -f puzzle-service.yml
