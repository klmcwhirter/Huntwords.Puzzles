#!/bin/bash

echo "Loading $1 ..."
cat $1 | sqlite3 bin/Debug/netcoreapp2.0/puzzles.sqlite
echo -e "Loading $1 ... DONE."
