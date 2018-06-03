#!/bin/bash

mkdir -p etc

echo "Generating puzzles.redis ..."
./puzzles2redis.sh > etc/puzzles.redis
echo "Done."

echo "Generating words.redis ..."
./words2redis.sh | gzip -9c > etc/words.redis.gz
echo "Done."
