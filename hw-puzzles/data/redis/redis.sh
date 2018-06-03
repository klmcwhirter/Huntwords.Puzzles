#!/bin/bash

CLIOPTS=""
OCOPTS="-i"
REDISCLI=/opt/rh/rh-redis32/root/usr/bin/redis-cli
REDISPASS=$(oc get secret redis -o jsonpath='{ .data.database-password }' | base64 -d)
REDISPOD=$(oc get po --selector name=redis -o name | sed 's?pods/??')
REDISCMD='oc exec ${OCOPTS} ${REDISPOD} -- ${REDISCLI} -a ${REDISPASS} ${CLIOPTS}'

case $1 in
load)
    CLIOPTS="--pipe"
    echo "Loading puzzles ..."
    cat etc/puzzles.redis | eval ${REDISCMD}
    echo "Done."

    echo "Loading words ..."
    gzip -dc etc/words.redis.gz | eval ${REDISCMD}
    echo "Done."
    ;;
*)
    OCOPTS="-it"
    eval ${REDISCMD} $*
    ;;
esac
