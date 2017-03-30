#!/bin/bash
# Parameters: 
#   dockerOptions - the docker run options, including port mapping and image name (ex. '-p 80:80 nginx:latest')
#   imageFile - the docker image file name what is downloaded with the script (ex. nginx.tar)
dockerOptions=$1
imageFile=$2

echo Start_script
# Check is imageFile is downloaded successfully
if [ -f `pwd`/$imageFile ]
then
  echo image download sucessfully to `pwd`/$imageFile
else
  echo image download failed
fi

# wait for docker extension to be installed
COUNTER=0
while ! [ -f /usr/bin/docker ]  && [ $COUNTER -lt 10 ]
do
  sleep 5
  let COUNTER=COUNTER+1
done
echo docker installed

# wait for docker daemon to start
COUNTER=0
while ! docker ps >/dev/null 2>&1 && [ $COUNTER -lt 10 ]
do
  sleep 5
  let COUNTER=COUNTER+1
done
echo docker daemon is running
docker --version

# load the local docker image
if docker load -i `pwd`/$imageFile
then
  echo image is loaded sucessfully
else
  echo image loading failed: exit code: $?
  echo image path is: `pwd`/$imageFile
fi

if docker run -d $dockerOptions
then
  echo running sucessfully
else
  echo run failed
fi

echo End_script
