BEGIN {
  printf("insert or replace into Puzzles(Id,Name,Description) values(%d,'%s','%s');\n",
    3,
    "Fruit",
    "Names of common fruits in the USA");
}
{
  printf("insert or replace into PuzzleWords(PuzzleId,Word) values(%d,'%s');\n", 3, $2);
}

