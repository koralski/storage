#!/bin/bash
# Parameters: 
#   dockerOptions - the docker run options, including port mapping and image name (ex. '-p 80:80 nginx:latest')
dockerOptions=$1

echo Start_script

docker --version

if docker run -d $dockerOptions
then
  echo running sucessfully
else
  echo run failed
fi

docker ps --format '{{json .}}'

echo End_script
