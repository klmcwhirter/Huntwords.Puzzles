#!/bin/bash

gzip -dc words-all.gz | 
awk '
length($1) >= 3 && length($1) <= 15 && $1 ~ "^[A-Za-z0-9]+$" { printf("LPUSH \"urn:words\" \"%s\"\r\n", $1); }
' | sort -ru --ignore-case | (echo -en "LTRIM \"urn:words\" -1 0\r\n"; cat -)
