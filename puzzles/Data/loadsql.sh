#!/bin/bash

echo "Loading $1 ..."
cat $1 | sqlite3 Data/puzzles.sqlite
echo -e "Loading $1 ... DONE."
