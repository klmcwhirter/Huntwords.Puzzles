#!/bin/bash

cat $1 | sqlite3 bin/Debug/netcoreapp2.0/puzzles.sqlite
