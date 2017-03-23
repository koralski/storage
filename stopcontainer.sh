#!/bin/bash
# Parameters: 
#   containerName - the docker container name to be stopped
containerName=$1

echo Stop container script

if docker stop $containerName
then
  echo stop sucessfully
else
  echo stop failed
fi

docker rm $containerName

echo End_script
