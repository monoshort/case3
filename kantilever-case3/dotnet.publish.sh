#!/bin/sh

for x in $(ls $(pwd)/src | grep Service)
do 
	dotnet publish "$(pwd)/src/${x}/${x}/${x}.csproj" -c Debug -o "$(pwd)/src/${x}/${x}/obj/Docker"
done
