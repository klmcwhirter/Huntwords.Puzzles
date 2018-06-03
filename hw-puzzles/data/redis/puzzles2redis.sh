#!/bin/bash

JQ='[
    .[] |
        {
            name,
            description,
            type: (
                if (.name == "Random" or .name == "Word") then (.name | ascii_downcase)
                else "static"
                end
            ),
            puzzleWords: (
                if (.name == "Word") then ["word"]
                else [.puzzleWords[].word]
                end
            )
        }
] | sort_by(.name) |
    map( { ("urn:puzzle:" + .name|tostring): . } ) | add |
    keys[] as $k | "\($k) \(.[$k])"
'
jq -M -r "${JQ}" puzzles-all.json |
sed 's/"/\\"/g' |
awk '
{
    key=$1;
    $1="";
    printf("SET \"%s\" \"%s\"\r\n", key, $0);
}
'