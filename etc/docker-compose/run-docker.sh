#!/bin/bash

if [[ ! -d certs ]]
then
    mkdir certs
    cd certs/
    if [[ ! -f localhost.pfx ]]
    then
        dotnet dev-certs https -v -ep localhost.pfx -p e753c15f-c0a3-439c-9769-2ac4fca0aca1 -t
    fi
    cd ../
fi

docker-compose up -d
